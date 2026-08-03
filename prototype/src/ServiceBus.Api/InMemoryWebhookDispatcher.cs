using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using ServiceBus.Contracts;
using ServiceBus.Messaging;

namespace ServiceBus.Api;

/// <summary>
/// In-process webhook dispatcher for local dev (pairs with InMemoryMessageBus).
/// </summary>
public sealed class InMemoryWebhookDispatcher : BackgroundService
{
    private readonly InMemoryMessageBus _bus;
    private readonly IDataStore _data;
    private readonly ILogger<InMemoryWebhookDispatcher> _logger;
    private readonly HttpClient _httpClient = new() { Timeout = TimeSpan.FromSeconds(30) };
    private List<WebhookRegistration> _webhooks = new();
    private static readonly JsonSerializerOptions JsonOptions = new(JsonSerializerDefaults.Web);

    public InMemoryWebhookDispatcher(InMemoryMessageBus bus, IDataStore data, ILogger<InMemoryWebhookDispatcher> logger)
    {
        _bus = bus;
        _data = data;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await RefreshWebhooksAsync();
        _logger.LogInformation("In-memory webhook dispatcher started (no RabbitMQ)");

        var deliveryTask = ConsumeDeliveryAsync(stoppingToken);
        var mgmtTask = ConsumeManagementAsync(stoppingToken);
        await Task.WhenAll(deliveryTask, mgmtTask);
    }

    private async Task ConsumeManagementAsync(CancellationToken ct)
    {
        await foreach (var msg in _bus.ManagementReader.ReadAllAsync(ct))
        {
            await RefreshWebhooksAsync();
            _logger.LogInformation("Webhook registry refreshed (management message)");
        }
    }

    private async Task ConsumeDeliveryAsync(CancellationToken ct)
    {
        await foreach (var msg in _bus.DeliveryReader.ReadAllAsync(ct))
        {
            await HandleDeliveryAsync(msg, ct);
        }
    }

    private async Task HandleDeliveryAsync(BusMessage msg, CancellationToken ct)
    {
        EventEnvelope? envelope = null;
        try
        {
            envelope = JsonSerializer.Deserialize<EventEnvelope>(msg.Json, JsonOptions);
            if (envelope is null)
                throw new InvalidOperationException("Empty envelope");

            var matches = _webhooks.Where(w => WebhookFilter.Matches(w, envelope)).ToList();
            if (matches.Count == 0)
            {
                _logger.LogInformation("No webhook match for {MessageId}", envelope.MessageId);
                return;
            }

            foreach (var webhook in matches
                .GroupBy(w => w.TargetUrl, StringComparer.OrdinalIgnoreCase)
                .Select(g => g.First()))
                await PostWebhookAsync(envelope, webhook, ct);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Delivery failed");
            if (msg.RetryCount >= 4)
                await _data.InsertDlqAsync(envelope?.MessageId ?? msg.MessageId, msg.Json, ex.Message);
            else
            {
                await Task.Delay(TimeSpan.FromSeconds(Math.Pow(2, msg.RetryCount + 1)), ct);
                await _bus.RequeueDeliveryAsync(msg, ct);
            }
        }
    }

    private async Task PostWebhookAsync(EventEnvelope envelope, WebhookRegistration webhook, CancellationToken ct)
    {
        var body = JsonSerializer.Serialize(envelope, JsonOptions);
        using var request = new HttpRequestMessage(HttpMethod.Post, webhook.TargetUrl)
        {
            Content = new StringContent(body, Encoding.UTF8, "application/json")
        };
        request.Headers.TryAddWithoutValidation("X-Rambase-EventID", envelope.MessageId);
        if (!string.IsNullOrEmpty(webhook.HmacSecret))
            request.Headers.TryAddWithoutValidation("X-Rambase-Signature", ComputeHmac(body, webhook.HmacSecret));

        var response = await _httpClient.SendAsync(request, ct);
        if (!response.IsSuccessStatusCode)
            throw new HttpRequestException($"Webhook POST returned {response.StatusCode}");

        _logger.LogInformation("Delivered {MessageId} to {Url}", envelope.MessageId, webhook.TargetUrl);
    }

    private static string ComputeHmac(string body, string secret)
    {
        using var hmac = new HMACSHA256(Encoding.UTF8.GetBytes(secret));
        return Convert.ToHexString(hmac.ComputeHash(Encoding.UTF8.GetBytes(body))).ToLowerInvariant();
    }

    private async Task RefreshWebhooksAsync() => _webhooks = await _data.GetWebhooksAsync();
}

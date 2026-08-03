using System.Net.Http.Json;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Npgsql;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using ServiceBus.Contracts;
using ServiceBus.Messaging;

namespace ServiceBus.Dispatcher;

public sealed class DispatcherWorker : BackgroundService
{
    private readonly ILogger<DispatcherWorker> _logger;
    private readonly RabbitMqOptions _rabbitOptions;
    private readonly string _postgresConnection;
    private readonly HttpClient _httpClient;
    private List<WebhookRegistration> _webhooks = new();
    private static readonly JsonSerializerOptions JsonOptions = new(JsonSerializerDefaults.Web);

    public DispatcherWorker(ILogger<DispatcherWorker> logger, RabbitMqOptions rabbitOptions, string postgresConnection)
    {
        _logger = logger;
        _rabbitOptions = rabbitOptions;
        _postgresConnection = postgresConnection;
        _httpClient = new HttpClient { Timeout = TimeSpan.FromSeconds(30) };
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await RefreshWebhooksAsync();

        var connection = await RabbitMqTopology.ConnectWithRetryAsync(_rabbitOptions, stoppingToken);
        var channel = await connection.CreateChannelAsync(cancellationToken: stoppingToken);
        await RabbitMqTopology.InitializeAsync(channel);

        await channel.BasicQosAsync(0, 1, false, stoppingToken);

        var deliveryConsumer = new AsyncEventingBasicConsumer(channel);
        deliveryConsumer.ReceivedAsync += async (_, ea) =>
        {
            await HandleDeliveryAsync(channel, ea, stoppingToken);
        };

        var mgmtChannel = await connection.CreateChannelAsync(cancellationToken: stoppingToken);
        var mgmtConsumer = new AsyncEventingBasicConsumer(mgmtChannel);
        mgmtConsumer.ReceivedAsync += async (_, ea) =>
        {
            await mgmtChannel.BasicAckAsync(ea.DeliveryTag, false, stoppingToken);
            await RefreshWebhooksAsync();
            _logger.LogInformation("Webhook registry refreshed after management message");
        };

        await channel.BasicConsumeAsync(MessageTopology.DeliveryQueue, autoAck: false, deliveryConsumer, stoppingToken);
        await mgmtChannel.BasicConsumeAsync(MessageTopology.ManagementQueue, autoAck: false, mgmtConsumer, stoppingToken);

        _logger.LogInformation("Dispatcher consuming {Delivery} and {Mgmt}", MessageTopology.DeliveryQueue, MessageTopology.ManagementQueue);

        try
        {
            await Task.Delay(Timeout.Infinite, stoppingToken);
        }
        catch (OperationCanceledException)
        {
            // shutdown
        }
    }

    private async Task HandleDeliveryAsync(IChannel channel, BasicDeliverEventArgs ea, CancellationToken ct)
    {
        var json = Encoding.UTF8.GetString(ea.Body.ToArray());
        EventEnvelope? envelope = null;
        try
        {
            envelope = JsonSerializer.Deserialize<EventEnvelope>(json, JsonOptions);
            if (envelope is null)
                throw new InvalidOperationException("Empty envelope");

            var matches = _webhooks.Where(w => WebhookFilter.Matches(w, envelope)).ToList();
            if (matches.Count == 0)
            {
                _logger.LogInformation("No webhook match for {MessageId} type={Type}", envelope.MessageId, envelope.EventType);
                await channel.BasicAckAsync(ea.DeliveryTag, false, ct);
                return;
            }

            // One POST per target URL (duplicate registrations should not multiply delivery)
            var distinctTargets = matches
                .GroupBy(w => w.TargetUrl, StringComparer.OrdinalIgnoreCase)
                .Select(g => g.First())
                .ToList();

            foreach (var webhook in distinctTargets)
            {
                await PostWebhookAsync(envelope, webhook, ct);
            }

            await channel.BasicAckAsync(ea.DeliveryTag, false, ct);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Delivery failed for message {Tag}", ea.DeliveryTag);
            var attempt = ea.BasicProperties.Headers?.ContainsKey("x-retry-count") == true
                ? Convert.ToInt32(ea.BasicProperties.Headers["x-retry-count"])
                : 0;

            if (attempt >= 4)
            {
                await InsertDlqAsync(envelope?.MessageId, json, ex.Message);
                await channel.BasicAckAsync(ea.DeliveryTag, false, ct);
            }
            else
            {
                await Task.Delay(TimeSpan.FromSeconds(Math.Pow(2, attempt + 1)), ct);
                await channel.BasicNackAsync(ea.DeliveryTag, false, true, ct);
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
        {
            var sig = ComputeHmac(body, webhook.HmacSecret);
            request.Headers.TryAddWithoutValidation("X-Rambase-Signature", sig);
        }

        var response = await _httpClient.SendAsync(request, ct);
        if (!response.IsSuccessStatusCode)
            throw new HttpRequestException($"Webhook POST {webhook.TargetUrl} returned {response.StatusCode}");

        _logger.LogInformation("Delivered {MessageId} to {Url}", envelope.MessageId, webhook.TargetUrl);
    }

    private static string ComputeHmac(string body, string secret)
    {
        using var hmac = new HMACSHA256(Encoding.UTF8.GetBytes(secret));
        return Convert.ToHexString(hmac.ComputeHash(Encoding.UTF8.GetBytes(body))).ToLowerInvariant();
    }

    private async Task RefreshWebhooksAsync()
    {
        await using var conn = new NpgsqlConnection(_postgresConnection);
        await conn.OpenAsync();
        await using var cmd = conn.CreateCommand();
        cmd.CommandText = "SELECT id, partner_id, target_url, event_type, database, hmac_secret, active, created_at FROM webhooks WHERE active = TRUE;";
        var list = new List<WebhookRegistration>();
        await using var reader = await cmd.ExecuteReaderAsync();
        while (await reader.ReadAsync())
        {
            list.Add(new WebhookRegistration
            {
                Id = reader.GetGuid(0),
                PartnerId = reader.GetString(1),
                TargetUrl = reader.GetString(2),
                EventType = reader.GetString(3),
                Database = reader.GetString(4),
                HmacSecret = reader.IsDBNull(5) ? null : reader.GetString(5),
                Active = reader.GetBoolean(6),
                CreatedAt = reader.GetDateTime(7)
            });
        }
        _webhooks = list;
    }

    private async Task InsertDlqAsync(string? messageId, string payload, string error)
    {
        await using var conn = new NpgsqlConnection(_postgresConnection);
        await conn.OpenAsync();
        await using var cmd = conn.CreateCommand();
        cmd.CommandText = "INSERT INTO dlq_entries (id, message_id, payload, error) VALUES ($1, $2, $3::jsonb, $4);";
        cmd.Parameters.AddWithValue(Guid.NewGuid());
        cmd.Parameters.AddWithValue((object?)messageId ?? DBNull.Value);
        cmd.Parameters.AddWithValue(payload);
        cmd.Parameters.AddWithValue(error);
        await cmd.ExecuteNonQueryAsync();
    }
}

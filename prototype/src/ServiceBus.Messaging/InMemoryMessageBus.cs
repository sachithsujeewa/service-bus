using System.Text.Json;
using System.Threading.Channels;
using ServiceBus.Contracts;

namespace ServiceBus.Messaging;

/// <summary>
/// Local dev transport — no RabbitMQ/Docker required. API + dispatcher must run in same process.
/// </summary>
public sealed class InMemoryMessageBus : IMessageBus
{
    private readonly Channel<BusMessage> _delivery = Channel.CreateUnbounded<BusMessage>();
    private readonly Channel<BusMessage> _management = Channel.CreateUnbounded<BusMessage>();
    private static readonly JsonSerializerOptions JsonOptions = new(JsonSerializerDefaults.Web);

    public ChannelReader<BusMessage> DeliveryReader => _delivery.Reader;
    public ChannelReader<BusMessage> ManagementReader => _management.Reader;

    public async Task PublishEventAsync(EventEnvelope envelope)
    {
        var json = JsonSerializer.Serialize(envelope, JsonOptions);
        await _delivery.Writer.WriteAsync(new BusMessage(json, envelope.MessageId));
    }

    public async Task PublishManagementAsync(ManagementMessage message)
    {
        var json = JsonSerializer.Serialize(message, JsonOptions);
        await _management.Writer.WriteAsync(new BusMessage(json, null));
    }

    public async Task RequeueDeliveryAsync(BusMessage message, IReadOnlyList<string> deliveredUrls, CancellationToken ct = default)
    {
        await _delivery.Writer.WriteAsync(
            message with { RetryCount = message.RetryCount + 1, DeliveredUrls = deliveredUrls },
            ct);
    }
}

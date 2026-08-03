using System.Text;
using System.Text.Json;
using RabbitMQ.Client;
using ServiceBus.Contracts;

namespace ServiceBus.Messaging;

public sealed class EventPublisher : IAsyncDisposable
{
    private readonly IConnection _connection;
    private readonly IChannel _channel;
    private static readonly JsonSerializerOptions JsonOptions = new(JsonSerializerDefaults.Web);

    private EventPublisher(IConnection connection, IChannel channel)
    {
        _connection = connection;
        _channel = channel;
    }

    public static async Task<EventPublisher> CreateAsync(RabbitMqOptions options, CancellationToken cancellationToken = default)
    {
        var connection = await RabbitMqTopology.ConnectWithRetryAsync(options, cancellationToken);
        var channel = await connection.CreateChannelAsync(cancellationToken: cancellationToken);
        await RabbitMqTopology.InitializeAsync(channel);
        return new EventPublisher(connection, channel);
    }

    public async Task PublishEventAsync(EventEnvelope envelope)
    {
        var body = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(envelope, JsonOptions));
        var props = new BasicProperties
        {
            ContentType = "application/json",
            MessageId = envelope.MessageId,
            DeliveryMode = DeliveryModes.Persistent
        };

        var routingKey = $"{MessageTopology.EventRoutingKeyPrefix}{envelope.SystemId}";
        await _channel.BasicPublishAsync(MessageTopology.EventsExchange, routingKey, false, props, body);
    }

    public async Task PublishManagementAsync(ManagementMessage message)
    {
        var body = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(message, JsonOptions));
        var props = new BasicProperties
        {
            ContentType = "application/json",
            DeliveryMode = DeliveryModes.Persistent
        };
        await _channel.BasicPublishAsync(MessageTopology.ManagementExchange, "", false, props, body);
    }

    public async ValueTask DisposeAsync()
    {
        if (_channel.IsOpen)
            await _channel.CloseAsync();
        if (_connection.IsOpen)
            await _connection.CloseAsync();
        _channel.Dispose();
        _connection.Dispose();
    }
}

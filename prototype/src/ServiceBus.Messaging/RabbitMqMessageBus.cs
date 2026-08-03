using System.Text;
using System.Text.Json;
using RabbitMQ.Client;
using ServiceBus.Contracts;

namespace ServiceBus.Messaging;

public sealed class RabbitMqMessageBus : IMessageBus, IAsyncDisposable
{
    private readonly EventPublisher _publisher;
    private static readonly JsonSerializerOptions JsonOptions = new(JsonSerializerDefaults.Web);

    private RabbitMqMessageBus(EventPublisher publisher) => _publisher = publisher;

    public static async Task<RabbitMqMessageBus> CreateAsync(RabbitMqOptions options)
    {
        var publisher = await EventPublisher.CreateAsync(options);
        return new RabbitMqMessageBus(publisher);
    }

    public Task PublishEventAsync(EventEnvelope envelope) => _publisher.PublishEventAsync(envelope);

    public Task PublishManagementAsync(ManagementMessage message) => _publisher.PublishManagementAsync(message);

    public async ValueTask DisposeAsync() => await _publisher.DisposeAsync();
}

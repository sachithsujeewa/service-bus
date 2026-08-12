using ServiceBus.Contracts;

namespace ServiceBus.Messaging;

public interface IMessageBus
{
    Task PublishEventAsync(EventEnvelope envelope);
    Task PublishManagementAsync(ManagementMessage message);
}

public sealed record BusMessage(
    string Json,
    string? MessageId,
    int RetryCount = 0,
    IReadOnlyList<string>? DeliveredUrls = null);

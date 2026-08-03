using RabbitMQ.Client;
using ServiceBus.Contracts;

namespace ServiceBus.Messaging;

public static class RabbitMqTopology
{
    public static async Task InitializeAsync(IChannel channel)
    {
        await channel.ExchangeDeclareAsync(MessageTopology.EventsExchange, ExchangeType.Topic, durable: true);
        await channel.ExchangeDeclareAsync(MessageTopology.ManagementExchange, ExchangeType.Fanout, durable: true);

        var deliveryArgs = new Dictionary<string, object?>
        {
            { "x-dead-letter-exchange", "" },
            { "x-dead-letter-routing-key", MessageTopology.DeadLetterQueue }
        };

        await channel.QueueDeclareAsync(MessageTopology.DeliveryQueue, durable: true, exclusive: false, autoDelete: false, arguments: deliveryArgs);
        await channel.QueueDeclareAsync(MessageTopology.ManagementQueue, durable: true, exclusive: false, autoDelete: false);
        await channel.QueueDeclareAsync(MessageTopology.DeadLetterQueue, durable: true, exclusive: false, autoDelete: false);

        await channel.QueueBindAsync(MessageTopology.DeliveryQueue, MessageTopology.EventsExchange, routingKey: "event.*");
        await channel.QueueBindAsync(MessageTopology.ManagementQueue, MessageTopology.ManagementExchange, routingKey: "");
    }

    public static ConnectionFactory CreateFactory(RabbitMqOptions options)
    {
        return new ConnectionFactory
        {
            HostName = options.Host,
            Port = options.Port,
            UserName = options.User,
            Password = options.Password,
            VirtualHost = options.VHost
        };
    }

    public static async Task<IConnection> ConnectWithRetryAsync(
        RabbitMqOptions options,
        CancellationToken cancellationToken = default,
        int maxAttempts = 30,
        TimeSpan? delay = null)
    {
        var wait = delay ?? TimeSpan.FromSeconds(2);
        var factory = CreateFactory(options);
        Exception? last = null;

        for (var attempt = 1; attempt <= maxAttempts; attempt++)
        {
            cancellationToken.ThrowIfCancellationRequested();
            try
            {
                return await factory.CreateConnectionAsync(cancellationToken);
            }
            catch (Exception ex) when (ex is not OperationCanceledException)
            {
                last = ex;
                await Task.Delay(wait, cancellationToken);
            }
        }

        throw new InvalidOperationException(
            $"Could not connect to RabbitMQ at {options.Host}:{options.Port} after {maxAttempts} attempts.",
            last);
    }
}

namespace ServiceBus.Contracts;

public static class MessageTopology
{
    public const string VHost = "rambase";
    public const string EventsExchange = "rambase.events";
    public const string ManagementExchange = "bus.management";
    public const string DeliveryQueue = "rambase.delivery";
    public const string ManagementQueue = "bus.management";
    public const string DeadLetterQueue = "rambase.dlq";
    public const string EventRoutingKeyPrefix = "event.";
}

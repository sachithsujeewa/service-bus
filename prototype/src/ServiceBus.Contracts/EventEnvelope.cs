namespace ServiceBus.Contracts;

public sealed class EventEnvelope
{
    public string SystemId { get; set; } = "DEMO";
    public string EventType { get; set; } = "";
    public string Database { get; set; } = "ALL";
    public long RamBaseEventId { get; set; }
    public DateTime RegisterTime { get; set; } = DateTime.UtcNow;
    public string? ObjectDetailsUri { get; set; }
    public Dictionary<string, string> Parameters { get; set; } = new();

    public string MessageId => $"{SystemId}-{RamBaseEventId}";
}

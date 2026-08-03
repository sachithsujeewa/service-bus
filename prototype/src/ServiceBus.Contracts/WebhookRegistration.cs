namespace ServiceBus.Contracts;

public sealed class WebhookRegistration
{
    public Guid Id { get; set; }
    public string PartnerId { get; set; } = "";
    public string TargetUrl { get; set; } = "";
    public string EventType { get; set; } = "";
    public string Database { get; set; } = "ALL";
    public string? HmacSecret { get; set; }
    public bool Active { get; set; } = true;
    public DateTime CreatedAt { get; set; }
}

public sealed class CreateWebhookRequest
{
    public string TargetUrl { get; set; } = "";
    public string EventType { get; set; } = "";
    public string Database { get; set; } = "ALL";
    public string? HmacSecret { get; set; }
}

public sealed class PublishEventRequest
{
    public string SystemId { get; set; } = "DEMO";
    public string EventType { get; set; } = "";
    public string Database { get; set; } = "ALL";
    public Dictionary<string, string>? Parameters { get; set; }
    public string? ObjectDetailsUri { get; set; }
}

public enum ManagementAction
{
    WebHookCreated,
    WebHookUpdated,
    WebHookDeleted
}

public sealed class ManagementMessage
{
    public ManagementAction Action { get; set; }
    public Guid WebhookId { get; set; }
    public string PartnerId { get; set; } = "";
}

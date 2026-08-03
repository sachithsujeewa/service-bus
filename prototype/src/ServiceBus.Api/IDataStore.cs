using ServiceBus.Contracts;

namespace ServiceBus.Api;

public interface IDataStore
{
    Task InitializeAsync();
    Task<long> NextEventIdAsync(string systemId);
    Task<(WebhookRegistration Webhook, bool Created)> CreateWebhookAsync(string partnerId, CreateWebhookRequest request);
    Task<List<WebhookRegistration>> GetWebhooksAsync(string? partnerId = null);
    Task<bool> DeleteWebhookAsync(Guid id, string partnerId);
    Task<List<object>> GetDlqAsync();
    Task InsertDlqAsync(string? messageId, string payload, string error);
}

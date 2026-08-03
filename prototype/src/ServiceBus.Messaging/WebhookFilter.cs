using ServiceBus.Contracts;

namespace ServiceBus.Messaging;

public static class WebhookFilter
{
    public static bool Matches(WebhookRegistration webhook, EventEnvelope eventEnvelope)
    {
        if (!webhook.Active)
            return false;

        if (!string.Equals(webhook.EventType, eventEnvelope.EventType, StringComparison.OrdinalIgnoreCase))
            return false;

        if (!string.Equals(webhook.Database, "ALL", StringComparison.OrdinalIgnoreCase)
            && !string.Equals(webhook.Database, eventEnvelope.Database, StringComparison.OrdinalIgnoreCase))
            return false;

        return true;
    }
}

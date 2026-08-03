---
type: process
id: OPS-004
name: Webhook Lifecycle
status: active
---

# Webhook Lifecycle

## States

```text
Draft/Configured → Active → (Updated)* → Deleted
                      │
                      └── Degraded (delivery failures)
```

## Create

1. Partner or admin registers webhook via RamBase API/UI
2. WHA/WHT records created
3. `WebHookCreated` meta event published
4. Manager updates subscription filters on bus
5. Subscriber begins delivering matching business events

## Update

Changes to URL or event type set:

1. WHA updated
2. `WebHookUpdated` event
3. Manager reconciles filters
4. Brief overlap window possible—subscriber should handle duplicate types

## Delete

1. Registration removed or marked deleted
2. `WebHookDeleted` event
3. Manager removes filters / subscription binding
4. No further HTTP for that registration

## Operational monitoring

| Signal | Action |
|--------|--------|
| Rising 5xx to URL | Partner outage; expect retries |
| DLQ growth | Schema or auth mismatch |
| Filter miss | Compare EventType registration vs. webhook |

## Partner responsibilities

- Maintain HTTPS endpoint availability
- Implement idempotent handlers
- Rotate URL auth without orphan registrations

## Relationships

- [[webhook-lifecycle]] --managed_by--> [[subscriber-and-manager-design]]
- [[webhook-lifecycle]] --requires--> [[webhooks-concept]]
- [[webhook-lifecycle]] --produces--> [[routing-and-filter-design]]
- [[webhook-lifecycle]] --part_of--> [[stakeholders-and-use-cases]]

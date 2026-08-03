---
type: architecture
id: ARC-003
name: Event Delivery Flow
status: active
---

# Event Delivery Flow

## Happy path (business event)

```text
1. User/action in RamBase commits business change
2. ERP writes event record (archive tables)
3. Publisher polls/sleeps loop detects new EVR rows
4. Publisher builds broker message (properties + body)
5. Publisher sends to MainTopic
6. Broker evaluates subscription filters per EventType
7. Matching subscription delivers to Subscriber handler
8. Subscriber OnMessageReceive invoked
9. Subscriber HTTP POST to webhook TargetUrl
10. Partner returns 2xx → message completed
```

Timing: dominated by `PublisherSleepInterval` polling granularity plus HTTP latency—not instantaneous with user click.

## Branch: manager path

```text
7b. If EventType in {WebHookCreated, WebHookUpdated, WebHookDeleted}
    → Manager handles (not standard subscriber business path)
    → Updates subscription filters / webhook topology
```

## Branch: deploy path

```text
Deploy publisher sends to DeployTopic
→ Deploy subscriber coordinates version rollout steps
(Separate from MainTopic business filters)
```

## State transitions (message)

```text
Published → Active on subscription → Delivered to handler
    │              │
    │              └──► Retry (backoff) on handler/HTTP failure
    └──► Dead-letter after max deliveries
```

Exact retry counts and backoff: [[retry-error-and-sequence-handling]].

## Sequence considerations

- Events for same **entity** may need order preservation on subscriber side.
- Global order across event types is **not** guaranteed.
- Concurrent publishers for different systems are independent streams.

## Observability checkpoints

| Step | What to log |
|------|-------------|
| Publish | EventType, EventId, system, MessageId |
| Filter miss | Rare—debug registration mismatch |
| HTTP | Status code, duration, URL host (not secrets) |
| Retry | Attempt count, sleep interval |

## Relationships

- [[event-delivery-flow]] --part_of--> [[architecture-overview]]
- [[event-delivery-flow]] --routes_through--> [[service-bus-topics-and-subscriptions]]
- [[event-delivery-flow]] --implemented_by--> [[publisher-design]]
- [[event-delivery-flow]] --implemented_by--> [[subscriber-and-manager-design]]
- [[event-delivery-flow]] --affects--> [[retry-error-and-sequence-handling]]

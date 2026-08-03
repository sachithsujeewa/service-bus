---
type: concept
id: CON-006
name: Event-Driven Integration Patterns
domain: service-bus
status: active
---

# Event-Driven Integration Patterns

## Pattern catalog for this platform

### Notification pattern

RamBase emits **notifications** about state that already committed in ERP. Subscribers react asynchronously. This is the core webhook pattern—not a command bus.

### Event-carried state transfer (partial)

Some COFs carry enough fields that subscribers update local state without a follow-up API call. Risk: payload may be stale if concurrent ERP edits occur. **Prefer notification + selective fetch** for financial-critical data.

### Idempotent consumer

Every subscriber should implement:

```text
receive(event) → dedupe key → apply side effect → return 200
```

Dedupe keys: `EventId`, `MessageId`, or composite `(EventType, EntityId, Version)`.

### Competing consumers (not default here)

Standard webhook design uses **one subscriber per subscription URL**, not competing workers on the same subscription. Scale out by splitting event types or URLs.

### Saga / process manager (external)

Long workflows span multiple event types. RamBase does not orchestrate sagas; partners correlate events in their own process managers.

### Outbox alignment

Publisher reads ERP event store (EVR path) and publishes to broker—conceptually an **outbox** pattern bridging DB commits to messaging.

## Code hooks vs. push events

**Code hooks** execute custom logic inside RamBase request path (synchronous). **Push events** decouple notification from user transaction latency. Choosing between them:

| Need | Prefer |
|------|--------|
| Block until external system acks | Hook (careful with timeouts) |
| Notify many partners | Push events |
| Heavy integration logic off ERP | Push events |
| Enforce invariant before commit | Hook |

Both can coexist; they are not interchangeable.

## Anti-patterns

- **Dual writes** — subscriber updates ERP via API and assumes webhook order guarantees global consistency
- **Chatty webhook** — handler triggers 10 APIs per event without batching
- **Poison ack** — returning 200 before durable local processing (lost events on crash)

## Relationships

- [[event-driven-integration-patterns]] --explains--> [[events-concept]]
- [[event-driven-integration-patterns]] --compared_with--> [[webhooks-concept]]
- [[event-driven-integration-patterns]] --supports--> [[push-events-business-goal]]
- [[event-driven-integration-patterns]] --implemented_by--> [[subscriber-and-manager-design]]

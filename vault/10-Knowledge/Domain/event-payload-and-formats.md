---
type: concept
id: CON-005
name: Event Payload and Formats
domain: service-bus
status: active
---

# Event Payload and Formats

## Layers of the payload

A delivered webhook body is assembled from multiple layers:

```text
┌─────────────────────────────────────┐
│ Transport envelope (broker message) │
│  - MessageId, correlation props     │
├─────────────────────────────────────┤
│ Event identity                      │
│  - EventType, EventId, timestamps   │
├─────────────────────────────────────┤
│ Business parameters (COFs)          │
│  - typed fields per event definition│
├─────────────────────────────────────┤
│ Serialization (JSON typical)        │
└─────────────────────────────────────┘
```

Subscribers should parse **identity** separately from **business fields** for logging and deduplication.

## Event type parameters (COFs)

**COF** (conceptually: column-oriented or configured fields) define the parameter schema for an event type. Registration of a new event type specifies which COFs appear in the payload. Changing COFs is a **contract change** — downstream version handlers required.

## Format stability rules

1. **Additive changes** — new optional fields are safest.
2. **Renames** — break filters and parsers; require new event type or version.
3. **Type changes** — dangerous for strict deserializers; document migration.
4. **Null vs. missing** — subscribers should treat unset COFs consistently.

## Headers vs. body

HTTP webhooks may carry:

- **Body** — primary JSON payload
- **Headers** — content type, optional authentication tokens, correlation ids

Exact header catalog is operational documentation; this note establishes that **correlation** should flow from ERP event id through broker to HTTP logs.

## Special format cases

### Manager events

Webhook CRUD events carry metadata needed to alter subscriptions—not the same shape as business events. Handlers must branch early on event type.

### Deploy events

Deploy-topic messages use a distinct schema oriented around package version, target role, and activation flags—not business COFs.

## Validation on receive

Subscriber-side checklist:

- [ ] EventType recognized
- [ ] Required COFs present
- [ ] Timestamp within skew tolerance
- [ ] Duplicate MessageId / EventId handled
- [ ] HTTP response codes align with retry policy (2xx = ack)

## Relationships

- [[event-payload-and-formats]] --part_of--> [[events-concept]]
- [[event-payload-and-formats]] --requires--> [[creating-an-event]]
- [[event-payload-and-formats]] --validated_by--> [[retry-error-and-sequence-handling]]
- [[event-payload-and-formats]] --explained_by--> [[routing-and-filter-design]]

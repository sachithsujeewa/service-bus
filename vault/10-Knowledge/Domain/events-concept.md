---
type: concept
id: CON-001
name: RamBase Events Concept
domain: service-bus
status: active
approval_required: false
---

# RamBase Events Concept

## Definition

A **RamBase event** is a persistent record that describes a business-meaningful activity or state change inside the ERP. Events are not arbitrary log lines; they carry typed identifiers, timestamps, and parameters that integrators can filter and interpret.

When a monitored value changes or a business operation completes, the platform creates an event that captures **what happened**, **to which entity**, and **with which parameters**.

## Mental model

```text
Business operation in RamBase
        │
        ▼
Event record materialized (typed, parameterized)
        │
        ├──► Historical query / audit (archives)
        └──► Push path → Publisher → Service Bus → Subscriber
```

Events sit between **OLTP state** (tables users edit) and **integration contracts** (what partners subscribe to).

## Event vs. message

| Aspect | RamBase event | Service Bus message |
|--------|---------------|---------------------|
| Authority | ERP semantic source | Transport envelope |
| Lifetime | Archived in ERP stores | Transient on broker |
| Typing | Event type + COF parameters | Broker properties + body |
| Consumers | APIs, archives, publisher | Subscriptions, subscribers |

Confusing the two leads to treating broker duplicates as new business facts — subscribers must deduplicate.

## Event history flow

Integrators monitor streams of homogeneous event types to build **live information flows**. Example: all hold-related events for a warehouse system paint a continuous picture of inventory movement without querying every hold record individually.

## Relationship to webhooks

Webhooks are **delivery contracts** bound to event types. The webhook layer does not redefine event semantics; it selects which event types reach which HTTPS endpoints.

## Archive concepts (high level)

- **EVR** — event header / registration context in archive model
- **VET** — event type dimension and versioning
- **WHT / WHA** — webhook header and archive tables for subscription metadata

Detailed persistence patterns: [[data-architecture]].

## Design implications

1. **Event types must be stable** — renaming breaks subscription filters.
2. **Parameters must be documented** — filters and payloads depend on COF fields.
3. **Ordering is partial** — subscribers should design for per-entity sequence, not global total order.
4. **Idempotency is subscriber responsibility** — redelivery is expected.

## Common confusions

- **"Any database change is an event"** — only registered, typed business events enter the push pipeline.
- **"Events replace APIs"** — events notify; APIs still authoritative for fetch and command.
- **"One event = one HTTP call"** — batching, retries, and manager routing can alter call patterns.

## Relationships

- [[events-concept]] --requires--> [[push-events-business-goal]]
- [[events-concept]] --produces--> [[event-payload-and-formats]]
- [[events-concept]] --published_to--> [[publisher-design]]
- [[events-concept]] --stored_in--> [[data-architecture]]
- [[events-concept]] --explained_by--> [[event-driven-integration-patterns]]

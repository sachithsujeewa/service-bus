---
type: architecture
id: ARC-004
name: Data Architecture
status: active
---

# Data Architecture

## Persistence zones

```text
┌─────────────────────────────────────────────────────────┐
│ RamBase ERP databases                                   │
│  EVR — event archive headers / event instances          │
│  VET — event type definitions                           │
│  WHT — webhook header records                           │
│  WHA — webhook archive / registration detail            │
│  Event COFs — parameter columns per type                │
│  Event Resources — API exposure of event streams        │
└─────────────────────────────────────────────────────────┘
┌─────────────────────────────────────────────────────────┐
│ Repository database (NGSystem, client credential store) │
│  NGSystem — system ↔ bus mapping + API credentials      │
│  Client DB — login credentials for systems              │
└─────────────────────────────────────────────────────────┘
┌─────────────────────────────────────────────────────────┐
│ RambaseServiceBus database                              │
│  PublishedEvent tracking / bus-side publish state       │
└─────────────────────────────────────────────────────────┘
┌─────────────────────────────────────────────────────────┐
│ Service Bus broker (non-SQL message store)              │
│  Topics, subscriptions, DLQ                             │
└─────────────────────────────────────────────────────────┘
```

## EVR / VET roles

- **VET** defines what event types exist and their versioning semantics.
- **EVR** stores occurrences—what actually fired, when, with references to business keys.

Publisher logic reads the **occurrence stream** and must respect **ready flags** or tolerance windows so partially committed events are not published prematurely.

## WHT / WHA roles

Webhook registration splits header vs. archive much like events:

- Registration metadata (URL, system, status)
- Historical webhook configuration changes for audit

Manager events align webhook changes with broker subscription filters.

## NGSystem

Central **routing table** for operations:

- Which `ServiceBusId` owns a system
- Whether API credentials exist or must be auto-generated
- Polling detects changes → hot activation path

## PublishedEvent store

Bus-side tracking prevents duplicate publish or supports reconciliation between ERP event cursor and broker. Treat as **operational state**, not business source of truth.

## Data flow summary

```text
Business tables → EVR (+VET type) → Publisher → Broker → (no long-term ERP copy of message)
Webhook UI/API → WHA/WHT → Manager → Subscription filters
Ops → NGSystem → Service host activation
```

## Consistency model

- ERP event write is **transactionally consistent** with business commit (ERP concern).
- Broker publish is **at-least-once** relative to publisher cursor.
- HTTP delivery is **at-least-once** with retries.

End-to-end **exactly-once** is not a platform guarantee.

## Relationships

- [[data-architecture]] --part_of--> [[architecture-overview]]
- [[data-architecture]] --stores_in--> [[events-concept]]
- [[data-architecture]] --stores_in--> [[webhooks-concept]]
- [[data-architecture]] --used_by--> [[publisher-design]]
- [[data-architecture]] --used_by--> [[creating-an-event]]

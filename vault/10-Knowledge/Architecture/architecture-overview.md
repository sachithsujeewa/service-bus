---
type: architecture
id: ARC-001
name: Architecture Overview
status: active
---

# Service Bus Architecture Overview

## Canonical pipeline

The end-to-end push path is expressed as:

```text
EVR → Publisher → Topic → Subscription → Subscriber → Target URL
```

Each arrow is a distinct component with its own failure domain and scaling knob.

## Component responsibilities

| Component | Responsibility |
|-----------|----------------|
| **EVR** (event archive reader path) | Source of new events in ERP context |
| **Publisher** | Poll/read events, map to broker messages, publish to topic |
| **Topic** | Fan-out bus channel (MainTopic / DeployTopic) |
| **Subscription** | Filtered view per delivery group |
| **Subscriber** | Consume messages, HTTP POST to webhook URL |
| **Manager** | Handle webhook CRUD control events |
| **ServiceBusService host** | Windows service hosting publishers/subscribers per system |

## Multi-system on one bus instance

A single Service Bus **instance** (unique `ServiceBusId`) serves **multiple RamBase systems** (e.g. RIC, SQLRIC). Mapping lives in **NGSystem** repository data—not in broker config alone.

```text
ServiceBusService (Id=1)
   ├── System RIC     → Publisher_RIC + Subscribers...
   └── System SQLRIC  → Publisher_SQLRIC + Subscribers...
```

Each system has:

- One **publisher** (ERP → topic)
- One **subscriber process per subscription** (topic → URL)

## Control vs. data plane

```text
Data plane:  MainTopic + business event filters + webhook delivery
Control plane: DeployTopic + manager subscription + webhook meta-events
```

Isolation prevents deploy storms from starving business delivery retry queues.

## External dependencies

- RamBase API (credentials per system)
- SQL repository DB (NGSystem, client credential store)
- Service Bus namespace (broker)
- Network path to subscriber HTTPS endpoints

## Quality attributes

| Attribute | Mechanism |
|-----------|-----------|
| Availability | Retry with backoff; multiple bus instances possible |
| Scalability | Horizontal systems per instance; subscription per URL group |
| Security | Segregated credentials; TLS to partners |
| Observability | Event log source, broker DLQ, HTTP status trails |
| Maintainability | Versioned deploy topic rollouts |

## Relationships

- [[architecture-overview]] --includes--> [[event-delivery-flow]]
- [[architecture-overview]] --includes--> [[system-context-and-boundaries]]
- [[architecture-overview]] --includes--> [[data-architecture]]
- [[architecture-overview]] --includes--> [[deployment-architecture]]
- [[architecture-overview]] --supports--> [[push-events-business-goal]]
- [[architecture-overview]] --documented_in--> [[02-architecture-map]]

---
type: map
name: Architecture Map
status: active
---

# Architecture Map

## Pipeline (center)

```text
                    ┌─────────────┐
                    │    EVR      │
                    └──────┬──────┘
                           │
                    ┌──────▼──────┐
                    │  Publisher  │
                    └──────┬──────┘
                           │
              ┌────────────┼────────────┐
              ▼            ▼            ▼
         MainTopic    DeployTopic   (ManagerSub)
              │            │            │
              ▼            ▼            ▼
         Subscription  DeploySub   Manager
              │            │            │
              ▼            ▼            ▼
          Subscriber   DeployHandler Manager
              │                         │
              ▼                         ▼
         Target URL              Filter updates
```

## Component notes

| Node | Note |
|------|------|
| EVR | [[events-concept]] / [[data-architecture]] |
| Publisher | [[publisher-design]] |
| MainTopic | [[service-bus-topics-and-subscriptions]] |
| Subscription | [[event-subscription-concept]] |
| Subscriber | [[subscriber-and-manager-design]] |
| Manager | [[subscriber-and-manager-design]] |
| NGSystem | [[system-context-and-boundaries]] |

## Data stores

```text
ERP (EVR,VET,WHA,WHT) ──► Publisher
Repository (NGSystem) ──► Service host
Bus DB (PublishedEvent) ──► Publisher state
Broker ──► Subscriptions
```

## Related maps

- [[01-master-knowledge-map]]
- [[03-traceability-map]]

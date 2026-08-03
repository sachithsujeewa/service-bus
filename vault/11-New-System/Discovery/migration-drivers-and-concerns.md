---
type: discovery
name: Migration Drivers and Concerns
status: active
chapter: 11-New-System
---

# Migration Drivers and Architectural Drawbacks

Synthesized from migration considerations document and architecture contract red-team Q&A. Explains **why change is discussed** — not which broker wins.

## TL;DR

The system is a **SQL poller → broker → HTTP webhook** bridge built on **EOL on-prem Service Bus** and historical choices (broker-side SQL routing, SQL leader election, infinite retry, property-bag messages). It works at current load but accumulates **platform risk**, **operational blind spots**, and **design debt** hard to fix in place.

**Target broker: not decided.** RabbitMQ analyzed as one candidate only.

## Primary drivers

| Driver | Summary |
|--------|---------|
| Platform EOL | MS Service Bus 1.1 + .NET Framework 4.6–4.7 on Windows |
| Broker coupling | Routing, dedup, ordering, locks are vendor features |
| Operational pain | No DLQ discipline, silent loss paths, ~60s failover, SQL leadership SPOF |
| Modernization window | Secrets in config, static globals, no metrics/tracing |
| Strategic flexibility | Blocks K8s, Linux, managed messaging, standard observability |

## Architectural decisions and drawbacks

### Poll SQL then publish (CDC-ish)

- Latency floor = poll interval; TCP wake unreliable
- Sequential EVR — blocks on gaps
- ReadyFlag timeout **skips** events (intentional loss)
- Dual cursor truth (EVR vs `PublishedEvent`)
- Batch size hardcoded (~26) — not configurable

### Broker-side SQL filters

- Rules opaque, not unit-testable in CI
- Runtime `AddRule`/`RemoveRule` on every webhook change
- `ParameterFilter` injection risk
- Wrong-filter messages **completed without HTTP** — silent drop
- Zero portability

### Payload in properties, dummy body

- Property size limits; non-standard for tooling
- HTTP layer rebuilds JSON/XML anyway — broker step adds cost without consumer benefit

### Active/passive HA via SQL heartbeat

- ~60s RTO gap on host failure
- Control DB SPOF for entire cloud
- Publisher/subscriber asymmetry on passive hosts
- Not horizontal scale — one active consumer host

### Infinite retry, no DLQ

- Poison URLs retry forever
- No operator inspection queue
- Partner outage → backlog + duplicate storms on recovery

### Management on same broker

- Admin and business traffic share fate
- WHA vs broker rule drift if management event lost

### Monolithic Windows Service

- Static `ServiceBusManager`, hardcoded 12-thread pool
- Hard to test, side-by-side versions, component scale-out

## Pitfall matrix

| Pitfall | Symptom | Root cause |
|---------|---------|------------|
| Silent event loss | Integrator never sees event | ReadyFlag skip, wrong-filter complete |
| Stuck pipeline | All webhooks stop for system | EVR gap, cursor wait |
| Failover gap | ~1 min no delivery | CSM + lock semantics |
| Cluster-wide stop | All systems idle | RambaseServiceBus SQL down |
| Infinite retry | One bad URL clogs sub | No DLQ |
| Duplicate webhooks | Same EventID many POSTs | At-least-once + failover |
| Config drift | WHA ≠ broker rules | Missed management event |
| Credential exposure | Leak from config/repo | Plaintext App.config |

## Cost of staying

Continued unsupported infra; no forcing function for DLQ/metrics/HA; migration under incident pressure when broker finally breaks.

## Relationships

- [[migration-drivers-and-concerns]] --produces--> [[architectural-concerns-register]]
- [[migration-drivers-and-concerns]] --supports--> [[modernization-charter]]
- [[migration-drivers-and-concerns]] --contrasts_with--> [[10-Knowledge/Design/retry-error-and-sequence-handling]]

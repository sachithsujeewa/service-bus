---
type: use-case-seed
id: ASUC-01
name: Event Relay Happy Path
status: seed
chapter: 11-New-System
---

# ASUC-01: Event Relay Happy Path

## Intent

Validate end-to-end latency and correctness when RamBase commits a business event and an integrator receives exactly one **business-meaningful** webhook (duplicates may exist at transport layer).

## Actors

- RamBase ERP (writer)
- Service Bus host (publisher + dispatcher)
- Integrator HTTPS endpoint

## Trigger

Business operation commits → EVR row inserted → (optional TCP wake) → publish → filter match → HTTP POST.

## Main flow

1. Event appears in EVR with ReadyFlag true within tolerance
2. Publisher advances cursor sequentially
3. Message published with correct properties / future JSON body
4. Matching webhook subscription receives message
5. HTTP 200 within timeout
6. Message acknowledged

## Quality scenarios

| Scenario | Measure |
|----------|---------|
| Steady-state latency | p50/p99 EVR insert → POST complete (M3 baseline) |
| Payload fidelity | All COFs present per contract |
| Correlation | `X-Rambase-EventID` matches `RamBaseEventId` |

## Concerns

Baseline for detecting regression during migration.

## Expand with

- Concrete event type (e.g. order shipped)
- Load level (events/sec)
- Acceptance thresholds from M3 baseline report

## Relationships

- [[uc-event-relay-happy-path]] --validates--> [[preserve-vs-replace-contract]]
- [[uc-event-relay-happy-path]] --explained_by--> [[delivery-guarantees-analysis]]

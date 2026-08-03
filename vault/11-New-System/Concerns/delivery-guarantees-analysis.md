---
type: concern-analysis
name: Delivery Guarantees Analysis
status: active
chapter: 11-New-System
---

# Delivery Guarantees Analysis

Honest delivery semantics for integrators and ASUC authors. Contrasts with simplified legacy chapter statements.

## What integrators can rely on today

| Guarantee | Supported? | Notes |
|-----------|------------|-------|
| At-least-once webhook delivery | **Yes** | Expect duplicates |
| Per-system ordering (happy path) | **Mostly** | Sequential publish + ordered topic + `MaxConcurrentCalls=1` |
| Exactly-once | **No** | Not enforced downstream |
| Every EVR row → webhook | **No** | ReadyFlag skip, filter mismatch, cursor sync |
| Bounded end-to-end latency | **No** | No SLA; poll + failover dominate |
| Delivery during control DB outage | **No** | Passive cluster |

## Loss paths (by design)

1. **ReadyFlag timeout** — cursor advances, event never published
2. **Wrong-filter complete** — message removed, no HTTP POST
3. **Cursor sync-forward** — past purged EVR history

## Retry paths

- HTTP failure → message not completed → broker redelivery
- `MaxDeliveryCount = int.MaxValue` → no count-based DLQ
- Exponential backoff with jitter + lock renewal during wait

## Target state (program intent)

Charter promises **dead-letter queues** and enhanced failure handling — explicit policy TBD:

| Policy area | Open question |
|-------------|---------------|
| Max retries before DLQ | Per URL? Per event type? |
| DLQ operator workflow | Replay tool? |
| Skip vs block on ReadyFlag | Keep or change? |
| Duplicate handling | Broker dedup vs app store |

## ASUC examples

- [[uc-event-relay-happy-path]] — baseline SLO measurement
- Partner receives duplicate POSTs after failover — dedupe requirement
- Event skipped due to ReadyFlag — integrator escalation

## Relationships

- [[delivery-guarantees-analysis]] --supports--> [[preserve-vs-replace-contract]]
- [[delivery-guarantees-analysis]] --contrasts_with--> [[10-Knowledge/Domain/event-driven-integration-patterns]]
- [[delivery-guarantees-analysis]] --validates--> [[CONC-005]], [[CONC-008]]

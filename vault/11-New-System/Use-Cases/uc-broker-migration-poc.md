---
type: use-case-seed
id: ASUC-04
name: Broker Migration POC
status: seed
chapter: 11-New-System
---

# ASUC-04: Broker Migration POC (M4)

## Intent

Prove replacement broker can satisfy ordering, delivery, and recovery tests before M5 build.

## Scope

M4 deliverables: POC implementation + test matrix.

## Test matrix (from program)

| Test | Pass criteria (define in ASUC) |
|------|-------------------------------|
| Event ordering | Per-system monotonic delivery order |
| Guaranteed delivery | No message loss under normal failure |
| Duplicate handling | Dedup or documented duplicate rate |
| Broker restart | Recovery without manual cursor edit |
| Subscriber failure | Backoff + eventual delivery or DLQ |
| Service host restart | Cursor integrity |
| Backlog recovery | Drain after outage within SLO |

## Concerns

[[CONC-015]], routing strategy, [[migration-pitfalls-matrix]]

## Relationships

- [[uc-broker-migration-poc]] --belongs_to--> [[milestone-roadmap-m1-m5]]
- [[uc-broker-migration-poc]] --requires--> [[broker-decision-pending]]

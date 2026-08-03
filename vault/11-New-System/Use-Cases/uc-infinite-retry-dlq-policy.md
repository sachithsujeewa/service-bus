---
type: use-case-seed
id: ASUC-07
name: Infinite Retry DLQ Policy
status: seed
chapter: 11-New-System
---

# ASUC-07: Retry and DLQ Policy (Target)

## Intent

Define target behavior replacing `MaxDeliveryCount = int.MaxValue` infinite retry.

## Scenarios

| Scenario | Target behavior |
|----------|-----------------|
| Partner 503 transient | Retry with backoff, succeed |
| Partner 400 permanent | Move to DLQ, alert ops |
| Poison payload | DLQ, no infinite loop |
| Partner outage 24h | Backlog cap? circuit breaker? |

## Charter alignment

"Enhanced failure handling and recovery" + dead-letter queues.

## Concerns

[[CONC-008]]

## Relationships

- [[uc-infinite-retry-dlq-policy]] --targets_replacement_of--> [[10-Knowledge/Design/retry-error-and-sequence-handling]]
- [[uc-infinite-retry-dlq-policy]] --explained_by--> [[delivery-guarantees-analysis]]

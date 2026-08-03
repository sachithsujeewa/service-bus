---
type: use-case
id: ASUC-MVP-05
name: MVP Retry and DLQ
chapter: 11-New-System
scope: mvp
status: active
---

# ASUC-MVP-05: Retry and Dead-Letter Queue

## Statement

Transient partner failures trigger bounded retries; permanent failures land in DLQ for operator inspection (target behavior vs legacy infinite retry).

## Configuration (MVP)

- Max attempts: 5
- Backoff: exponential from 2s
- DLQ: `rambase.dlq` queue

## Flow

1. Partner returns 503 or timeout
2. Dispatcher nack with requeue until max attempts
3. Message routed to DLQ
4. Admin GET `/api/dlq` lists payload + error

## Acceptance criteria

- [ ] No infinite loop on bad URL
- [ ] DLQ entry after max retries
- [ ] Metrics `dispatch_failures_total` increment

## Relationships

- [[mvp-asuc-05-retry-dlq]] --targets_replacement_of--> [[10-Knowledge/Design/retry-error-and-sequence-handling]]

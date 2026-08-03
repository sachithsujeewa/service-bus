---
type: design
id: DES-004
name: Retry Error and Sequence Handling
status: active
---

# Retry, Error, and Sequence Handling

## Retry policy (bus host)

Configurable parameters (names conceptual):

- `BaseSleepTimeOnErrorInSeconds` — initial backoff after handler/HTTP failure
- `NumberOfTimesToSleepBeforeExponentialIncrease` — linear phase before exponential growth

Pattern:

```text
attempt 1 → sleep base
attempt 2 → sleep base
...
attempt N → sleep base * 2^k  (exponential phase)
```

Goal: survive transient partner outages without hammering endpoints.

## Broker-level retry

When subscriber **abandons** a message, Service Bus returns it to the subscription with increasing delivery count. After max deliveries → **dead-letter queue (DLQ)**.

Operators must monitor DLQ for poison messages (schema breaks, permanent 4xx).

## HTTP errors vs. application errors

| Partner behavior | Interpretation |
|------------------|----------------|
| 503 / timeout | Transient → retry |
| 400 with bad payload | Likely publisher mapping bug → fix forward, DLQ message |
| 200 before async failure | Partner bug; bus considers delivered |

## Sequence handling

**Per-entity ordering:** If `OrdUpdated` for order 123 must follow `OrdCreated`, subscriber should:

- Buffer by entity key, or
- Use version/sequence COF if provided, or
- Re-fetch authoritative state from API on any order event

**Cross-entity:** No ordering guarantee—design workflows accordingly.

## Ready flag and timing

Publisher respects ready flags and tolerance window—reduces subscribers seeing events before ERP COFs stabilize.

## Error logging

Windows **Event Log** source (configured name for bus service) captures host-level failures. Correlate with broker MessageId in centralized logging where available.

## Idempotency contract

Platform delivers **at-least-once**. Subscribers must implement idempotent handlers—retries are normal, not exceptional.

## Relationships

- [[retry-error-and-sequence-handling]] --affects--> [[subscriber-and-manager-design]]
- [[retry-error-and-sequence-handling]] --validates--> [[event-payload-and-formats]]
- [[retry-error-and-sequence-handling]] --uses--> [[service-bus-technology]]
- [[retry-error-and-sequence-handling]] --related_to--> [[event-driven-integration-patterns]]

---
type: use-case-seed
id: ASUC-02
name: Failover Active Passive
status: seed
chapter: 11-New-System
---

# ASUC-02: Active Host Failover

## Intent

Define acceptable **RTO** and duplicate behavior when the active Service Bus host fails mid-delivery.

## Trigger

Active host process crash or VM failure while messages in-flight.

## Key behaviors (as-is)

1. Heartbeats stop in `CloudHostSync`
2. Passive host waits ~60s
3. Passive promotes, starts subscribers
4. In-flight messages may be redelivered → duplicate POSTs

## Quality scenarios

| Scenario | Question for ASUC |
|----------|-------------------|
| RTO | Max gap without webhook delivery ≤ ? seconds |
| Duplicates | Max duplicate POSTs per EventId during failover |
| Backlog | Unacked messages recovered without loss |

## Target state questions

- Can K8s reduce gap below 60s?
- Is active/active ever acceptable with idempotency store?

## Concerns

[[CONC-003]], [[CONC-006]], [[CONC-004]]

## Relationships

- [[uc-failover-active-passive]] --explained_by--> [[ha-dr-concerns]]
- [[uc-failover-active-passive]] --contrasts_with--> [[10-Knowledge/Architecture/event-delivery-flow]]

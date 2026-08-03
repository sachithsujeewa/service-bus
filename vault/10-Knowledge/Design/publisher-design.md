---
type: design
id: DES-001
name: Publisher Design
status: active
---

# Publisher Design

## Role

The **Publisher** is the bridge from RamBase event archives to the Service Bus **MainTopic** (and deploy path where applicable). One publisher instance exists **per active system** on a bus host.

## Control loop

```text
while service running:
    read new events from EVR (cursor / ready flag logic)
    for each eligible event:
        map to broker message
        publish to MainTopic
    sleep PublisherSleepInterval
```

Polling interval trades **latency vs. load**. Sub-second intervals increase SQL read pressure; multi-second intervals add notification delay.

## Ready flag tolerance

Events may carry a **ready** state indicating ERP-side completion. `ReadyFlagToleranceTimeInMilliSeconds` allows a grace window before treating an event as publishable—prevents subscribers seeing partially populated COFs.

## Message mapping

Publisher responsibilities:

1. Set **EventType** user property for subscription filters
2. Serialize COF parameters into body (JSON/XML per contract)
3. Assign **MessageId** for broker deduplication scope
4. Attach correlation properties for logging

Mapping errors should **not** publish malformed messages—log and skip or dead-letter per policy.

## Credential usage

Publisher uses system-specific API credentials from NGSystem (or auto-provisioned pair) to read event data from RamBase—not the management client used by operators.

## Failure handling

| Failure | Behavior |
|---------|----------|
| SQL read error | Log, sleep, retry loop |
| Broker send error | Retry with backoff; do not advance cursor until safe |
| Mapping error | Skip event with alert; investigate COF mismatch |

## Scaling

Adding systems scales **publisher count** on an instance, not threads per publisher. Each publisher is logically single-threaded around its cursor.

## Relationships

- [[publisher-design]] --part_of--> [[event-delivery-flow]]
- [[publisher-design]] --publishes_to--> [[service-bus-topics-and-subscriptions]]
- [[publisher-design]] --consumes--> [[events-concept]]
- [[publisher-design]] --uses--> [[rambase-api-integration]]
- [[publisher-design]] --depends_on--> [[data-architecture]]

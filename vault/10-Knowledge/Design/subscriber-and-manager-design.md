---
type: design
id: DES-002
name: Subscriber and Manager Design
status: active
---

# Subscriber and Manager Design

## Subscriber role

The **Subscriber** consumes messages from a **single subscription** (grouped webhooks sharing a target URL). Delivery is **event-driven**: `OnMessageReceive` fires per broker message.

```text
OnMessageReceive(message):
    if EventType in WebHookMetaTypes:
        delegate to Manager
    else:
        build HTTP request from message body
        POST to TargetUrl
        interpret status → Complete / Abandon / DeadLetter
```

## Manager role

The **Manager** handles webhook lifecycle event types:

- WebHookCreated
- WebHookUpdated  
- WebHookDeleted

It updates subscription **filters**, webhook archives alignment, and internal registration state—without requiring a full service restart for each webhook change.

## HTTP delivery

Subscriber maintains HTTP client configuration for the target URL:

- Timeouts aligned with broker lock duration
- TLS certificate validation
- Optional auth headers per webhook registration

Return code mapping:

| Partner response | Subscriber action |
|------------------|-------------------|
| 2xx | Complete message |
| 5xx / timeout | Abandon → broker retry |
| 4xx (persistent) | Policy choice: DLQ vs. retry |

## Client database access

Subscriber and Manager read **client credential database** for `ClientId` / `ClientSecret` per system—distinct from NGSystem provisioning flow.

## One subscriber per subscription

Architecture assigns **one subscriber process** per subscription to preserve ordering assumptions per URL and simplify retry state.

## Concurrency

`OnMessageReceive` may be invoked concurrently unless configured for single concurrency—partners must handle parallel posts unless they enforce serial processing.

## Relationships

- [[subscriber-and-manager-design]] --part_of--> [[event-delivery-flow]]
- [[subscriber-and-manager-design]] --delivers_to--> [[webhooks-concept]]
- [[subscriber-and-manager-design]] --managed_by--> [[webhook-lifecycle]]
- [[subscriber-and-manager-design]] --uses--> [[retry-error-and-sequence-handling]]
- [[subscriber-and-manager-design]] --filtered_by--> [[routing-and-filter-design]]

---
type: concept
id: CON-003
name: Event Subscription Concept
domain: service-bus
status: active
---

# Event Subscription Concept

## Definition

An **event subscription** is the broker-side binding that determines which messages published to a **topic** are visible to a **subscriber instance**. In this platform, subscriptions aggregate webhooks that share delivery characteristics—typically the same **target URL** and compatible filter rules.

## Topic → subscription → subscriber chain

```text
Topic (e.g. MainTopic)
   │
   ├── Subscription A  (filters: EventType=HldCreated, ...)
   │        └── Subscriber instance → POST url-a
   │
   └── Subscription B  (filters: EventType=OrdShipped, ...)
            └── Subscriber instance → POST url-b
```

## Filter rules

Service Bus subscriptions use **filter rules** so not every message on a topic reaches every subscriber. For webhook event types, filters commonly encode:

```text
EventType = "<RegisteredEventTypeName>"
```

When a webhook registers interest in `HldCreated`, the corresponding filter expression is derived and attached to the subscription. The broker performs filter evaluation **before** the subscriber's `OnMessageReceive` handler runs.

## Grouping by target URL

**Design decision:** webhooks that share the same remote URL are grouped under one subscription. Rationale:

- One HTTP client pipeline per endpoint reduces connection churn
- Retry state is per subscription / URL
- Filter union must still discriminate event types at handler level

See [[subscription-grouping-by-target-url]].

## Manager subscription

A separate **manager subscription** listens for control-plane event types (webhook CRUD). This isolates topology changes from data-plane traffic on the main topic.

## Dynamic updates

When webhook metadata changes, manager-handled events update filters or subscription bindings. Subscribers should tolerate **filter changes** without assuming static broker configuration across months of operation.

## Comparison with ERP-side registration

| Layer | What is registered |
|-------|---------------------|
| RamBase ERP | Event type, COFs, archives |
| Webhook (WHA) | URL + event type binding |
| Service Bus | Subscription + filter rules |
| NGSystem | Which bus instance serves a system |

All four must align for end-to-end delivery.

## Relationships

- [[event-subscription-concept]] --part_of--> [[service-bus-topics-and-subscriptions]]
- [[event-subscription-concept]] --filtered_by--> [[routing-and-filter-design]]
- [[event-subscription-concept]] --requires--> [[webhooks-concept]]
- [[event-subscription-concept]] --managed_by--> [[subscriber-and-manager-design]]

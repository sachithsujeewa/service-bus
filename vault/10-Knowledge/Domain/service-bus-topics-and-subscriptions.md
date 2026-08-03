---
type: concept
id: CON-004
name: Service Bus Topics and Subscriptions
domain: service-bus
status: active
---

# Service Bus Topics and Subscriptions

## Broker primitives

Microsoft Service Bus (Windows / Azure lineage) provides **topics** for publish-subscribe and **subscriptions** as named views with optional filters.

### Topic

A **topic** is a logical channel where publishers send messages. Multiple independent subscribers can consume the same topic through different subscriptions without each publisher knowing subscribers.

### Subscription

A **subscription** hangs off a topic and maintains its own cursor, dead-letter queue, and filter rules. Each subscription is consumed by at most one logical subscriber service in this design (one subscriber process per subscription).

## Platform topic split

This integration uses **at least two topics**:

| Topic role | Typical name | Traffic |
|------------|--------------|---------|
| Main data plane | MainTopic | Business event deliveries to webhooks |
| Deploy control plane | DeployTopic | Bus version rollout and coordination |

Separating deploy messages prevents operational commands from competing with business filters and retry policies on the main topic.

## Message flow

```text
Publisher.Send(MainTopic, message)
        │
        ▼
   Topic engine
        │
        ├── filter match ──► Subscription 1 ──► Subscriber 1
        └── filter match ──► Subscription 2 ──► Subscriber 2
```

Non-matching messages are not delivered to that subscription (they may match another).

## Filters and actions

Filters are boolean predicates on user properties or body fields. **Actions** can rewrite properties before delivery (less common in webhook path). Incorrect filter syntax causes silent non-delivery — validation at registration time is critical.

## Scaling dimensions

- **More event types** → more filter rules or subscriptions
- **More target URLs** → more subscriptions (grouped by URL policy)
- **More tenant systems** → more publisher/subscriber pairs per bus instance via NGSystem mapping

## Technology note

Connection strings, namespaces, and ports belong in secure configuration stores — see [[service-bus-technology]] for stack context without secrets.

## Relationships

- [[service-bus-topics-and-subscriptions]] --part_of--> [[architecture-overview]]
- [[service-bus-topics-and-subscriptions]] --uses--> [[service-bus-technology]]
- [[service-bus-topics-and-subscriptions]] --routes_through--> [[event-delivery-flow]]
- [[service-bus-topics-and-subscriptions]] --includes--> [[event-subscription-concept]]

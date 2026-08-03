---
type: technology
id: TEC-001
name: Service Bus Technology
status: active
---

# Service Bus Technology

## Stack position

The integration uses **Microsoft Service Bus** messaging (Windows Service Bus / Azure Service Bus lineage):

- **Topics** for publish-subscribe
- **Subscriptions** with filters
- **SDK** connection via namespace connection string (stored in secure configuration—not in vault notes)

## Connection model

App configuration provides:

- Broker endpoint (sb:// namespace)
- STS / runtime / management ports (on-premises hosting pattern)
- `ServiceBusId` identifying this host instance

## Fixed topic names

Operational convention keeps names stable across releases:

- `MainTopic` — webhook data plane
- `DeployTopic` — deployment control plane
- `ManagerSubscription` — manager consumer binding name

Changing these without coordinated migration breaks all environments.

## Broker capabilities used

| Feature | Usage |
|---------|-------|
| Topics | Fan-out from publishers |
| Subscription filters | EventType routing |
| Peek-lock receive | Subscriber processing |
| Dead-letter | Poison message isolation |
| Message properties | Filter on EventType |

## Comparison: Windows vs. Azure

Conceptually identical programming model; hosting differs:

- On-prem Windows Service Bus with local SQL persistence
- Azure Service Bus cloud namespace

Migration between them affects connection strings and port layout but not the EVR→Publisher→Topic mental model.

## Upgrade notes

Study material on Windows Service Bus covers broker limits (message size, filter count, TTL). Platform upgrades should validate:

- Filter expression compatibility
- Max message size vs. largest COF payload
- Lock duration vs. HTTP timeout

## Relationships

- [[service-bus-technology]] --uses--> [[service-bus-topics-and-subscriptions]]
- [[service-bus-technology]] --supports--> [[architecture-overview]]
- [[service-bus-technology]] --related_to--> [[retry-error-and-sequence-handling]]

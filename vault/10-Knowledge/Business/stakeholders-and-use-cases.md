---
type: business
name: Stakeholders and Use Cases
status: active
---

# Stakeholders and Use Cases

## Stakeholders

### Event publishers (RamBase platform)

Own event definitions, archives (EVR/VET), and API surfaces that expose event history. They ensure triggers are consistent with ERP semantics.

### Service Bus operators

Run the bus host, monitor NGSystem mappings, manage deployments, and tune retry policies. They bridge ERP credentials and broker topology.

### Integration developers (subscribers)

Register webhooks, choose event types, implement HTTPS endpoints, and handle idempotency on their side.

### RamBase tenant administrators

Approve which systems participate, manage API client credentials, and scope which event streams a tenant exposes.

## Primary use cases

### UC-1: Inventory change notification

When stock levels or hold records change, a warehouse system receives `HldCreated` / `HldUpdated` style events and updates local cache.

### UC-2: Order pipeline automation

Sales order lifecycle events drive external CRM or fulfillment workflows without polling order APIs.

### UC-3: Webhook self-management

Special manager-handled event types (`WebHookCreated`, `WebHookUpdated`, `WebHookDeleted`) allow dynamic subscription changes to propagate through the bus without restarting subscribers.

### UC-4: Controlled deployment

Deploy-topic messages coordinate rolling updates of bus components across environments while systems remain registered in NGSystem.

### UC-5: Audit and replay

Event archives support historical queries; operators compare live webhook stream against archive for reconciliation.

## Non-goals for subscribers

- Assuming exactly-once delivery without local deduplication
- Treating webhook payload as authoritative financial record without verification
- Blocking ERP transactions on subscriber HTTP latency

## Relationships

- [[stakeholders-and-use-cases]] --belongs_to--> [[push-events-business-goal]]
- [[stakeholders-and-use-cases]] --uses--> [[webhooks-concept]]
- [[stakeholders-and-use-cases]] --uses--> [[event-subscription-concept]]

---
type: map
name: Master Knowledge Map
status: active
---

# Master Knowledge Map

Navigation hub for the Service Bus knowledge graph.

## Business layer

- [[push-events-business-goal]] — why push events exist
- [[stakeholders-and-use-cases]] — who and what scenarios

## Domain layer

- [[events-concept]] — ERP event semantics
- [[webhooks-concept]] — HTTP callback subscriptions
- [[event-subscription-concept]] — broker bindings
- [[service-bus-topics-and-subscriptions]] — topic/subscription primitives
- [[event-payload-and-formats]] — COFs and serialization
- [[event-driven-integration-patterns]] — integration patterns catalog

## Architecture layer

- [[architecture-overview]] — canonical pipeline hub
- [[system-context-and-boundaries]] — trust boundaries
- [[event-delivery-flow]] — step-by-step delivery
- [[data-architecture]] — EVR/VET/WHA/NGSystem
- [[deployment-architecture]] — hosts and rollout topology

## Design layer

- [[publisher-design]]
- [[subscriber-and-manager-design]]
- [[routing-and-filter-design]]
- [[retry-error-and-sequence-handling]]
- [[deployment-event-design]]

## Technology layer

- [[service-bus-technology]]
- [[rambase-api-integration]]

## Process layer

- [[creating-an-event]]
- [[activating-a-system]]
- [[release-and-deployment]]
- [[webhook-lifecycle]]

## Governance

- [[subscription-filter-rules]]
- [[event-type-registration-rules]]
- [[topic-per-purpose-decision]]
- [[subscription-grouping-by-target-url]]

## Reading paths

### New integrator

```text
push-events-business-goal → events-concept → webhooks-concept →
event-delivery-flow → event-payload-and-formats → webhook-lifecycle
```

### New operator

```text
architecture-overview → activating-a-system →
retry-error-and-sequence-handling → release-and-deployment
```

### Architect

```text
architecture-overview → data-architecture → routing-and-filter-design →
topic-per-purpose-decision → deployment-architecture
```

## Graph

See [[03-traceability-map]] and `99-Graph/graph.json`.

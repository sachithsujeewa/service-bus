---
type: decision
id: DEC-001
name: Topic per Purpose Decision
status: active
approval_required: true
---

# Decision: Separate Topics for Data and Deploy Planes

## Context

Service Bus topics multiplex all message kinds. Mixing business webhook events with deployment commands complicates filters, retry policies, and incident isolation.

## Decision

Use **MainTopic** for business webhook data plane and **DeployTopic** for service version coordination.

## Rationale

- Independent retry and monitoring
- Deploy storms do not contend with partner HTTP delivery
- Clearer security review surface (deploy messages often ops-only)

## Consequences

- Publishers/subscribers must bind correct topic in configuration
- Misconfigured topic name causes total delivery failure—configuration is critical
- Two connection publish paths in service code

## Alternatives rejected

| Alternative | Why rejected |
|-------------|--------------|
| Single topic with message kind property | Filter complexity; higher blast radius |
| External deploy tool only | Loses coordinated quiesce with running subscribers |

## Relationships

- [[topic-per-purpose-decision]] --affects--> [[service-bus-topics-and-subscriptions]]
- [[topic-per-purpose-decision]] --supports--> [[deployment-event-design]]
- [[topic-per-purpose-decision]] --documented_in--> [[architecture-overview]]

---
type: decision
name: Broker Decision Pending
status: needs_review
chapter: 11-New-System
decision_status: pending
---

# Broker Decision (Pending)

## Status

**No go/no-go broker selection** as of vault scan (M4 in progress).

## Candidates under evaluation

Azure Service Bus (baseline), RabbitMQ, Kafka, NATS+JetStream, ActiveMQ Artemis, Redpanda.

## Tensions to resolve

| Tension | Detail |
|---------|--------|
| Charter vs scorecard | Charter emphasizes RabbitMQ; scorecard ranks NATS #1 |
| Workload fit | Webhook queues ≠ streaming log — Kafka/Redpanda may be overkill |
| .NET ecosystem | NATS weaker vs RabbitMQ/Kafka clients |
| K8s operations | NATS/RabbitMQ both viable; Azure SB Premium differs |

## Decision inputs required

- M3 production telemetry (volume, latency, backlog)
- M4 POC results on ordering, duplicates, restart recovery
- TCO model at projected growth
- Ops team skill map

## Decision record template (when ready)

```markdown
Decision: Selected broker = ?
Context: M4 POC results
Consequences: Routing model, HA, hosting
Alternatives rejected: ...
```

## Relationships

- [[broker-decision-pending]] --blocks--> [[target-platform-direction]]
- [[broker-decision-pending]] --requires--> [[broker-evaluation-summary]]
- [[broker-decision-pending]] --validates--> [[CONC-015]], [[CONC-016]]

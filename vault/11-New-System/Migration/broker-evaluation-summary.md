---
type: migration
name: Broker Evaluation Summary
status: active
chapter: 11-New-System
decision_status: pending
---

# Broker Evaluation Summary

Synthesized from POP broker scorecard and M4 milestone criteria. **No broker selected yet.**

## Evaluation context

- Microsoft-centric enterprise, on-prem / hybrid deployment
- Workload: **webhook task queues** + **topic pub/sub** + **ordering** + **retry** — not primary event streaming
- Current baseline: on-prem Azure Service Bus (score 78/100 in internal scorecard)

## Weighted rankings (internal scorecard)

| Rank | Broker | Score | Summary |
|------|--------|-------|---------|
| 1 | NATS + JetStream | 85 | Ops, performance, TCO, portability; smaller .NET ecosystem |
| 2 | Redpanda | 82 | Kafka API, simpler ops; streaming/replay strength |
| 3 | Apache Kafka | 79 | Industry log; heavier day-2 ops |
| 4 | Azure Service Bus (current) | 78 | Strong .NET; limited replay/portability |
| 5 | RabbitMQ | 76 | Classic messaging; self-managed ops at scale |
| 6 | ActiveMQ Artemis | 70 | JMS legacy |

**Note:** Rankings are **program artifacts** — re-weight when production telemetry quantifies volume and streaming needs.

## Criteria weights

| Criterion | Weight |
|-----------|--------|
| Messaging model | 15% |
| Streaming & replay | 10% |
| Reliability & HA | 15% |
| Performance & scale | 15% |
| Operational ease | 15% |
| TCO & licensing | 10% |
| .NET / Azure fit | 10% |
| Portability | 10% |

## Workload fit for RamBase webhook relay

| Pattern | Best fit (scorecard) | Relevance |
|---------|---------------------|-----------|
| Task queues & work distribution | RabbitMQ / Azure SB | **High** — webhook delivery |
| Enterprise pub/sub with filters | Azure SB / RabbitMQ | **High** — current SqlFilter model |
| Event sourcing / replay | Kafka / Redpanda | **Low today** — no replay requirement in charter |
| Low-latency internal signals | NATS | Medium — TCP wake path |
| Hybrid on-prem + K8s | RabbitMQ / NATS | **High** — charter direction |

## M4 POC must validate

- Event ordering per system stream
- Guaranteed delivery semantics under broker restart
- Duplicate handling vs. current MessageId dedup
- Subscriber failure and infinite retry policy replacement
- Service Bus process restart recovery
- Backlog recovery after outage

## Decision guidance (from scorecard)

**Stay on Azure SB** if Azure-centric, limited ops headcount, no multi-day replay need.

**Migrate** if cost at scale, hybrid without Premium lock-in, Kafka-style replay, or lighter self-hosted ops on K8s.

## Program scope tension

Charter lists RabbitMQ as **primary candidate** while scorecard ranks NATS highest — **requires reconciliation** in decision record ([[broker-decision-pending]]).

## Relationships

- [[broker-evaluation-summary]] --compared_with--> [[rabbitmq-routing-options]]
- [[broker-evaluation-summary]] --requires--> [[milestone-roadmap-m1-m5]]
- [[broker-evaluation-summary]] --affects--> [[routing-strategy-options]]

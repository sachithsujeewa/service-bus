---
type: concern-analysis
name: Observability Gaps
status: active
chapter: 11-New-System
---

# Observability Gaps

## Current signals

| Signal | Location | Limitation |
|--------|----------|------------|
| Windows Event Log | `RB_SB` source | No correlation across hosts |
| SQL `Log` table | RambaseServiceBus | Query-heavy; not real-time SLO |
| MS Teams | Alert webhook | No metrics, noisy |
| Grafana dashboards | Production exists | Functional overview; may lack E2E latency |

## Missing capabilities

- Prometheus-style metrics (publish rate, lag, POST latency, DLQ depth)
- Distributed tracing (EVR id → MessageId → HTTP trace)
- Health/readiness endpoints for K8s
- Unified correlation ID across publish and POST
- SLO dashboards with error budgets

## M3 deliverable alignment

Baseline metrics before migration:

- Resource utilization
- Functional bus metrics
- Baseline report + alerting improvements

## Target observability design

Program page **Service Bus Observability design** is empty — must be filled for M5.

Minimum metric set for ASUCs:

```text
publisher_lag_seconds (cursor vs MAX(EVR))
broker_queue_depth (per subscription)
webhook_post_duration_seconds
webhook_post_failures_total
leader_active_host_id
management_binding_reconciliation_errors
```

## Relationships

- [[observability-gaps]] --validates--> [[CONC-009]]
- [[observability-gaps]] --blocks--> [[proposed-interfaces-gap]]
- [[observability-gaps]] --requires--> [[milestone-roadmap-m1-m5]]

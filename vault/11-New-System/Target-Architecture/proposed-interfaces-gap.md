---
type: gap
name: Proposed Interfaces Gap
status: draft
chapter: 11-New-System
---

# Proposed Interfaces Gap

Program wiki **Service Bus Interfaces / APIs** is a placeholder (2026-06-03).

## Needed for ASUC and implementation

| Interface | Purpose |
|-----------|---------|
| Health/readiness | K8s probes, load balancer |
| Admin API | Replay cursor, drain system, inspect DLQ |
| Metrics scrape | Prometheus OpenMetrics |
| Webhook contract test | CI validation of HTTP shape |
| Management event schema | Versioned webhook CRUD messages |

## Current state

No public REST admin API in legacy codebase — ops use SQL + Event Log + Grafana.

## ASUC seeds

- Operator drains system before maintenance
- SRE validates deployment via health endpoint
- Integrator contract test in CI pipeline

## Relationships

- [[proposed-interfaces-gap]] --belongs_to--> [[observability-gaps]]
- [[proposed-interfaces-gap]] --blocks--> [[target-platform-direction]]

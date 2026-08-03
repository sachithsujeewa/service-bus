---
type: architecture-target
name: Target Platform Direction
status: draft
chapter: 11-New-System
---

# Target Platform Direction

**Status: draft** — placeholders in program wiki; synthesized direction from charter and M5.

## Target stack (directional)

| Layer | Current | Target |
|-------|---------|--------|
| Runtime | .NET Framework 4.6–4.7, Windows Service | **.NET 8** |
| Host | Windows Server VMs | **Linux containers on Kubernetes** |
| Broker | On-prem MS Service Bus 1.1 | **TBD** (see [[broker-decision-pending]]) |
| Config | App.config plaintext | **GitOps + secret store** |
| Deploy | Manual service stop/start | **CI/CD pipeline** |
| Observability | Event Log + SQL + Teams + Grafana | **Metrics, traces, SLO dashboards** |

## Architectural goals (target state)

1. **Portability** — run on K8s without Windows-only broker lock-in
2. **Operability** — DLQ, health checks, defined RTO/RPO, runbooks
3. **Security** — no plaintext secrets; log redaction; filter validation
4. **Testability** — component isolation; broker in CI
5. **Zero disruption** — parallel run or phased cutover with contract tests

## K8s deployment sketch (conceptual)

```text
Namespace: service-bus
 ├── deployment: publisher-worker (per-system or pooled — TBD)
 ├── deployment: webhook-dispatcher (active replica count tied to HA model)
 ├── deployment: management-consumer
 ├── statefulset or external: broker cluster
 ├── secrets: broker creds, SQL, HMAC keys via CSI
 └── configmaps: non-secret tuning (poll intervals, backoff)
```

**HA redesign decision:** Keep SQL heartbeat leader election or move to K8s lease / broker consumer groups — **open** ([[ha-dr-concerns]]).

## Out of scope for platform move

Changing RamBase EVR emission model or WHA schema — migration is **bus host + broker**, not ERP redesign.

## Relationships

- [[target-platform-direction]] --belongs_to--> [[modernization-charter]]
- [[target-platform-direction]] --requires--> [[broker-decision-pending]]
- [[target-platform-direction]] --targets_replacement_of--> [[10-Knowledge/Architecture/deployment-architecture]]

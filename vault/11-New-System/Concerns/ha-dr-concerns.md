---
type: concern-analysis
name: HA and DR Concerns
status: active
chapter: 11-New-System
---

# HA and DR Concerns

## Current HA model

- **Active/passive** per `ServiceBusCloudId`
- Election: `CloudStateMonitor` + `CloudHostSync` heartbeats (~500 ms)
- Priority field on `ServicebusHosts` — lower number = higher priority
- **RTO (webhook gap):** ~60 seconds on host death (CSM delay + subscriber startup)
- **Not** active/active; not horizontal consumer scale-out

## DR gaps

| Gap | Impact |
|-----|--------|
| No automated second-site failover | Manual redeploy + DB/broker restore |
| No cursor replication | RPO for "published to broker" = last acked cursor |
| No event replay tooling | Manual `PublishedEvent` edit is dangerous |
| No chaos/failover test harness in codebase | Unknown real RTO/RPO |
| Broker namespace DR | Dual endpoint in connection string — farm HA, not geo |

## Control DB SPOF (CONC-003)

When **RambaseServiceBus SQL** unavailable:

- `CheckShouldRun` fails → hosts assume passive posture
- **Entire cloud stops webhook delivery** even if ERP SQL and broker healthy

Target must either:

- HA the control database explicitly, or
- Decouple leadership from delivery (K8s leases, broker consumer groups)

## Failover boundary risks

- Split-brain mitigated by priority + blocking timestamps — **not formal consensus**
- Passive host abandons messages → broker churn at boundary
- Duplicate POSTs possible during handoff

## Target direction questions

1. Keep SQL heartbeat or replace with K8s `Lease` / Strimzi / operator patterns?
2. Accept 60s RTO or invest in faster handoff?
3. Define DR runbook with tested RPO for cursor and WHA bindings?

## ASUC seeds

- [[uc-failover-active-passive]]
- Active host dies during high backlog — recovery behavior
- DR restore in secondary datacenter — manual procedure

## Relationships

- [[ha-dr-concerns]] --validates--> [[CONC-003]], [[CONC-006]], [[CONC-011]]
- [[ha-dr-concerns]] --contrasts_with--> [[10-Knowledge/Architecture/deployment-architecture]]

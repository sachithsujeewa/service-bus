---
type: discovery
name: As-Implemented Contract Summary
status: active
chapter: 11-New-System
source_type: codebase-contract
---

# As-Implemented Architecture Contract (Discovery Summary)

Synthesized from codebase analysis (`ARCHITECTURE_CONTRACT`, `Service Bus 2.0` technical analysis). This describes **what the running system actually does** — richer than legacy wiki docs in `10-Knowledge/`.

**Chapter note:** This is discovery input for migration, not the legacy chapter narrative. Compare via [[05-architecture-chapter-comparison]].

## One-liner

Windows service: **poll RamBase SQL event tables** → publish to **on-prem Azure Service Bus topics** → **HTTP webhook delivery**. Multi-host HA via **DB-backed leader election**; only active host runs subscribers.

## Solution layout

| Artifact | Role |
|----------|------|
| `ServiceBus` library (.NET 4.7.2) | Business logic |
| `ServiceBusService` (.NET 4.6.1) | Windows Service host → `ServiceBusManager.Startup()` |
| SDK | `WindowsAzure.ServiceBus` 2.2.4 — legacy `BrokeredMessage`, `SqlFilter` |

## Runtime topology

```text
ServiceBusManager
 ├── CloudStateMonitor (leader election)
 ├── ServiceBusManagementSubscriber (webhook CRUD)
 ├── RBSTcpListener (wake publisher)
 └── RambaseSystemClient × N (per NG_SYSTEM row)
      ├── RambaseSystemPublisher (poll EVR)
      └── WebHookSubscriber × M (per URL subscription)
```

## Broker topology (as-implemented)

| Entity | Rule |
|--------|------|
| System topic | Path = `NGSystem.Name` (not single MainTopic for all) |
| Management topic | Config `ServiceBusManagementTopic` |
| Webhook subscription | MD5(`Sub {systemName} {url}`) — max 50 chars |
| Message body | Literal `"Rambase Event"` — **payload in Properties** |
| MessageId | `{SystemID}-{RamBaseEventId}` |
| Duplicate detection | 20 min window, ordering enabled per topic |
| MaxDeliveryCount | `int.MaxValue` — effectively no DLQ by count |

## Message property contract (preserve in migration)

| Property | Role |
|----------|------|
| `EventType` | Filter + handler dispatch |
| `SystemID` | System name |
| `Database` | Company scope |
| `RamBaseEventId` | Monotonic id |
| `RegisterTime`, `RegisterPid` | Audit; pid used in ignore-users filter |
| `ObjectDetailsUri` | API deep link |
| `P_{key}` | Event parameters (`[col]` → `P_col` in filters) |

## SQL filter semantics (webhook rules)

Built by `SubscriptionFilters.GenerateWebHookEventTypeFilter`:

- `EventType = '...'`
- Optional `Database = '...'` (skip if ALL)
- Optional `RegisterPid NOT IN (...)` from ignore-users
- Optional `PARAMETERFILTER` with `[col]` → `P_col` rewrite

**Concern:** Business rules in broker SQL strings — largest portability lock-in ([[CONC-002]]).

## HA model (as-implemented)

- Hosts in `ServicebusHosts` + heartbeats in `CloudHostSync` (~500 ms)
- **Only active host** starts webhook subscribers
- Failover delay ~**60 s** (lock token expiry tuning)
- **Gotcha:** Publishers may start on passive hosts — duplicate publish risk ([[CONC-004]])

## Delivery semantics (honest)

| Claim | Truth |
|-------|-------|
| Per-system ordering | Mostly — sequential publish + ordered topic |
| At-least-once to webhook | Yes, with duplicates |
| Exactly-once | No |
| Every EVR → webhook | No — ReadyFlag skip, wrong-filter complete |
| Bounded latency | No SLA in code |

## Latency budget (order-of-magnitude)

| Stage | Typical |
|-------|---------|
| Poll wake | 0–1000 ms (`PublisherSleepInterval`) |
| ReadyFlag | 0–60000 ms tolerance |
| HTTP POST | up to 30 s |
| Failover gap | ~60 s |

## Security (as-implemented)

- Plaintext secrets in `App.config` ([[CONC-007]])
- Optional HMAC on webhook POST (`X-Rambase-Signature`)
- No management plane auth beyond broker connection string
- Verbose logging may capture payload PII

## Observability (as-implemented)

- Windows Event Log (`RB_SB`)
- SQL `Log` table in RambaseServiceBus DB
- MS Teams webhook for alerts
- Grafana dashboards exist for current production monitoring
- **No** metrics API, distributed tracing, health endpoint

## Critical SPOF

**RambaseServiceBus SQL down** → CSM treats cluster as passive → **global webhook delivery stop** even if broker and ERP SQL healthy ([[CONC-003]]).

## Relationships

- [[as-implemented-contract-summary]] --contrasts_with--> [[10-Knowledge/Architecture/architecture-overview]]
- [[as-implemented-contract-summary]] --preserves_contract_from--> [[preserve-vs-replace-contract]]
- [[as-implemented-contract-summary]] --explains--> [[delivery-guarantees-analysis]]
- [[as-implemented-contract-summary]] --documents--> [[01-new-system-source-catalog]]

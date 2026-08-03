# Proposed MVP Architecture

**Status:** design intent (pre-implementation)  
**Implemented by:** `prototype/` + `diagrams/mvp/01-platform-overview.md` (deployment view)

## MVP scope

**Goal:** One RamBase-style business event → broker → matching registered webhook → HTTP POST to external partner.

| Category | Items |
|----------|--------|
| **Mocks allowed** | RamBase event writer (simulated EVR or API); external partner (local HTTP receiver) |
| **Real (core pillars)** | Event ingestion + publish; message broker; webhook registration + filters; routing/filter evaluation; dispatcher (retry, ack, optional HMAC); control data (cursor, registry); basic observability |
| **Out of MVP** | Multi-host HA / leader election; full NG_SYSTEM fleet sync; TCP wake listener; production DR, GitOps fleet; broker scorecard POC matrix; every event type / tenant |

## Proposed MVP architecture (ASCII)

```text
┌────────────────────────────────────────────────────────────────────────────────────────────┐
│                              MVP — END-TO-END EVENT → WEBHOOK                             │
│                         Service Bus 2.0 · architectural spine only                        │
└────────────────────────────────────────────────────────────────────────────────────────────┘

  LEGEND:  [MOCK]  simulated in prototype   [REAL]  implemented component   [TBD]  stub/minimal

┌────────────────────────────────────────────────────────────────────────────────────────────┐
│ EXTERNAL (mockable)                                                                        │
│                                                                                            │
│  ┌─────────────────────┐         ┌─────────────────────┐         ┌─────────────────────┐ │
│  │ [MOCK] RamBase App  │         │ [MOCK] Partner app    │         │ Developer / Ops     │ │
│  │                     │         │                     │         │ (curl, browser)       │ │
│  │ POST /mock/events   │         │ POST /webhook       │         │                       │ │
│  │ or SQL seed script  │         │ receives JSON +     │         │ GET /health           │ │
│  │                     │         │ X-Rambase-EventID   │         │ GET /metrics          │ │
│  └──────────┬──────────┘         └──────────▲──────────┘         └──────────┬──────────┘ │
│             │ emits event                    │ HTTPS POST                      │ observes  │
└─────────────┼────────────────────────────────┼─────────────────────────────────┼──────────┘
              │                                │                                 │
              ▼                                │                                 ▼
┌────────────────────────────────────────────────────────────────────────────────────────────┐
│ PLATFORM BOUNDARY — MVP SERVICE BUS (.NET 8, single deployable or 2–3 small services)      │
│                                                                                            │
│  ┌──────────────────────────────────────────────────────────────────────────────────────┐ │
│  │ A. INGESTION PLANE (replaces EVR poll for MVP)                                       │ │
│  │                                                                                      │ │
│  │   ┌──────────────────┐      ┌──────────────────┐      ┌──────────────────────────┐ │ │
│  │   │ [MOCK] Event      │      │ [REAL] Event      │      │ [REAL] Publish cursor    │ │ │
│  │   │ Simulator API     │─────►│ Normalizer        │─────►│ store (SQLite/Postgres)  │ │ │
│  │   │ (OrderCreated,    │      │ (RamBaseEvent DTO │      │ LastPublishedEventId     │ │ │
│  │   │  HldCreated, …)   │      │  + P_* params)    │      │ per system               │ │ │
│  │   └──────────────────┘      └────────┬─────────┘      └──────────────────────────┘ │ │
│  │                                        │ publish                                      │ │
│  └────────────────────────────────────────┼──────────────────────────────────────────────┘ │
│                                           ▼                                                │
│  ┌──────────────────────────────────────────────────────────────────────────────────────┐ │
│  │ B. MESSAGING PLANE (broker spine — RabbitMQ recommended for MVP charter fit)         │ │
│  │                                                                                      │ │
│  │   ┌────────────────────────────────────────────────────────────────────────────┐    │ │
│  │   │ [REAL] Message Broker                                                        │    │ │
│  │   │                                                                              │    │ │
│  │   │   exchange: rambase.events (topic or headers)                                │    │ │
│  │   │        │                                                                     │    │ │
│  │   │        ├── route key / headers: system + EventType + Database              │    │ │
│  │   │        │                                                                     │    │ │
│  │   │        ├── queue: system.SQLRIC.delivery  ──► webhook dispatcher           │    │ │
│  │   │        └── queue: bus.management          ──► management handler            │    │ │
│  │   │                                                                              │    │ │
│  │   │   message body: JSON envelope (MVP normalizes away property-bag legacy)    │    │ │
│  │   │   message id: {SystemID}-{RamBaseEventId}  (preserve contract id)          │    │ │
│  │   └────────────────────────────────────────────────────────────────────────────┘    │ │
│  └──────────────────────────────────────────────────────────────────────────────────────┘ │
│                                           │                                                │
│              ┌────────────────────────────┴────────────────────────────┐                  │
│              ▼                                                         ▼                  │
│  ┌───────────────────────────────┐              ┌───────────────────────────────────────┐ │
│  │ C. REGISTRATION / CONTROL     │              │ D. DELIVERY PLANE                      │ │
│  │                               │              │                                        │ │
│  │ [REAL] Webhook Registry API   │              │ [REAL] Webhook Dispatcher worker     │ │
│  │ POST /webhooks                │              │                                        │ │
│  │  • targetUrl                  │   bindings   │ consume queue                          │ │
│  │  • eventType                  │─────────────►│   │                                    │ │
│  │  • database scope             │              │   ├─► [REAL] Filter engine           │ │
│  │  • optional HMAC secret       │              │   │     EventType, Database,         │ │
│  │                               │              │   │     ParameterFilter (in-proc)    │ │
│  │ [REAL] Webhook store (DB)     │              │   │                                    │ │
│  │ WHA-equivalent table          │              │   ├─► [REAL] HTTP client (Poster)    │ │
│  │                               │              │   │     POST JSON, 30s timeout       │ │
│  │ [REAL] Management handler     │              │   │     X-Rambase-EventID header     │ │
│  │ on bus.management queue       │              │   │     optional X-Rambase-Signature │ │
│  │ WebHookCreated/Updated/       │              │   │                                    │ │
│  │ Deleted → update bindings     │              │   ├─► ack/nack + backoff + DLQ     │ │
│  │                               │              │   │     (MVP: DLQ after N retries)   │ │
│  └───────────────────────────────┘              │   └──────────────────────────────► Partner │
│                                                 └───────────────────────────────────────┘ │
│                                                                                            │
│  ┌──────────────────────────────────────────────────────────────────────────────────────┐ │
│  │ E. CROSS-CUTTING (minimal but real — architectural filler, not optional in MVP)    │ │
│  │                                                                                      │ │
│  │  [REAL] Config        env + secrets (no plaintext in repo)                           │ │
│  │  [REAL] Health        /health, /ready (broker + DB connectivity)                     │ │
│  │  [REAL] Observability structured logs + 3–5 Prometheus metrics                       │ │
│  │                       publish_total, dispatch_total, post_latency, dlq_depth, lag  │ │
│  │  [STUB] Admin API     replay/drain — interface only, not full implementation         │ │
│  └──────────────────────────────────────────────────────────────────────────────────────┘ │
│                                                                                            │
│  MVP HA: [STUB] single replica — Leader coordinator interface present but always ACTIVE  │
└────────────────────────────────────────────────────────────────────────────────────────────┘

              ▼                    ▼                    ▼
┌──────────────────┐  ┌──────────────────┐  ┌──────────────────────────┐
│ [REAL] MVP DB      │  │ [REAL] Broker    │  │ [MOCK] Partner listens   │
│ webhooks           │  │ RabbitMQ         │  │ localhost:9090/webhook   │
│ publish_cursor     │  │ (docker)         │  │ (or MockServer)          │
│ optional event_log │  │                  │  │                          │
└──────────────────┘  └──────────────────┘  └──────────────────────────┘
```

## Happy-path sequence

```text
Partner registers          Developer fires mock           Dispatcher delivers
        │                         event                         │
        ▼                         │                             ▼

  POST /webhooks                  │                    POST https://partner/webhook
  { url, eventType, db }          │                    { event envelope JSON }
        │                         │                             │
        ▼                         ▼                             ▼
 ┌─────────────┐    publish    ┌─────────┐    consume    ┌─────────────┐
 │ Webhook     │───mgmt msg───►│ Broker  │──────────────►│ Dispatcher  │
 │ Registry    │               │         │               │ + Filter    │
 └─────────────┘               └────┬────┘               └─────────────┘
        │                           │                            │
        │ update binding            │ business msg               │ 200 OK → ack
        └──────────────────────────►│◄── Event Simulator ────────┘
                                    publish
```

## Architectural pillars (MVP vs full system)

| Pillar | MVP implementation | Full system (later) |
|--------|-------------------|---------------------|
| **Decoupled publish/consume** | Simulator → broker → dispatcher | Real EVR poll + TCP wake |
| **Webhook registration** | REST + DB + management queue | WHA + RamBase mgmt events |
| **Routing / filters** | In-process filter engine (+ optional headers exchange) | Full ParameterFilter + ignore-users |
| **Delivery contract** | JSON body, EventID header, optional HMAC | Same + XML format |
| **Reliability** | Manual ack, backoff, **DLQ with cap** | Infinite retry policy decision |
| **Idempotency key** | MessageId = SystemID-EventId | + dedup store if broker lacks it |
| **Observability** | Health + metrics + structured logs | Full tracing, SLO dashboards |
| **HA** | Interface stub, single instance | CloudStateMonitor or K8s lease |
| **Portability** | .NET 8 + container + pluggable broker client | Multi-replica K8s fleet |

## What this MVP proves

```text
RamBase event semantics  ──►  normalized envelope  ──►  broker  ──►  filter  ──►  partner webhook
        ▲                         ▲                    ▲           ▲
     [mock]                   [real]               [real]      [real]
```

Exercises every **plane** from the target design (ingest, messaging, registration, delivery, observability) without Windows Service, legacy SDK, or SQL heartbeat HA.

## Related diagrams

| File | Focus |
|------|--------|
| [01-platform-overview.md](01-platform-overview.md) | As-built Docker Compose topology |
| [02-component-relationships.md](02-component-relationships.md) | .NET project wiring |
| [03-happy-path-sequence.md](03-happy-path-sequence.md) | Runnable demo steps |
| `vault/11-New-System/MVP/mvp-deployment-architecture.md` | Vault deployment note |

## Target alignment

- Implements slice of [[proposed-component-model]] (`vault/11-New-System/Target-Architecture/`)
- Broker choice for MVP: RabbitMQ (program broker still TBD)

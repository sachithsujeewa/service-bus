---
type: use-case-catalog
name: MVP Use Case Catalog
chapter: 11-New-System
scope: mvp-prototype
status: active
---

# MVP Use Case Catalog

Architecturally and functionally significant use cases for the **local MVP prototype** (`prototype/`). Each maps to automated or manual tests.

## Actor model

| Actor | App | Credential |
|-------|-----|------------|
| **Source system** | `source-app` | API key role `source` |
| **Integration partner** | `partner-app` | API key role `partner` |
| **Bus operator** | curl / future admin UI | API key role `admin` |
| **Dispatcher** | `service-bus-dispatcher` | RabbitMQ user + DB |
| **Registry API** | `service-bus-api` | validates API keys |

## Architectural significant use cases (ASUC)

### ASUC-MVP-01 — End-to-end event relay

| Field | Value |
|-------|-------|
| **Trigger** | Source emits `OrderCreated` for system `DEMO` |
| **Flow** | source-app → API publish → RabbitMQ → dispatcher → partner POST |
| **Quality** | Delivery within 5s local; `X-Rambase-EventID` present |
| **Test** | `scripts/demo.sh` step 3–4 |
| **Note** | [[mvp-asuc-01-event-relay]] |

### ASUC-MVP-02 — Partner webhook registration (authZ)

| Field | Value |
|-------|-------|
| **Trigger** | Partner registers URL + event type via API |
| **Auth** | Partner API key required; cannot register another partner's scope |
| **Flow** | partner-app startup → POST /api/webhooks → DB + mgmt message |
| **Test** | demo step 1; reject without key |
| **Note** | [[mvp-asuc-02-webhook-registration]] |

### ASUC-MVP-03 — Filter mismatch (no delivery)

| Field | Value |
|-------|-------|
| **Trigger** | Event type `InvoicePaid` but webhook only `OrderCreated` |
| **Expected** | No HTTP POST; message acked after filter skip |
| **Quality** | No false delivery |
| **Test** | manual POST wrong event type |

### ASUC-MVP-04 — Unauthorized publish

| Field | Value |
|-------|-------|
| **Trigger** | POST /api/events without source key |
| **Expected** | 401 Unauthorized |
| **Concern** | CONC-007 security baseline |

### ASUC-MVP-05 — Dispatcher retry and DLQ

| Field | Value |
|-------|-------|
| **Trigger** | Partner returns 503 |
| **Expected** | Backoff retry; after max → DLQ queue |
| **Note** | [[mvp-asuc-05-retry-dlq]] |

### ASUC-MVP-06 — HMAC verification (partner side)

| Field | Value |
|-------|-------|
| **Trigger** | Webhook registered with HMAC secret |
| **Expected** | Partner validates `X-Rambase-Signature` |
| **Test** | partner-app logs verification result |

### ASUC-MVP-07 — Health and readiness

| Field | Value |
|-------|-------|
| **Trigger** | K8s-style probes |
| **Expected** | `/health` all services; API `/ready` checks DB + broker |

## Functional use cases (user-facing)

| ID | Title | Actor | Summary |
|----|-------|-------|---------|
| F-01 | Emit business event | Source | Fire event after mock ERP action |
| F-02 | Subscribe to events | Partner | Register webhook for event type + database |
| F-03 | Receive push notification | Partner | Handle JSON payload at HTTPS endpoint |
| F-04 | Unsubscribe | Partner | DELETE webhook registration |
| F-05 | Inspect failed deliveries | Operator | GET /api/dlq |
| F-06 | List my webhooks | Partner | GET /api/webhooks |

## Traceability to target pillars

| Pillar | MVP ASUCs |
|--------|-----------|
| Ingestion | ASUC-MVP-01, F-01 |
| Messaging | ASUC-MVP-01 |
| Registration | ASUC-MVP-02, F-02, F-04 |
| Routing/filter | ASUC-MVP-03 |
| Delivery | ASUC-MVP-01, F-03 |
| Security | ASUC-MVP-02, ASUC-MVP-04, ASUC-MVP-06 |
| Reliability | ASUC-MVP-05 |
| Observability | ASUC-MVP-07 |

## Relationships

- [[mvp-use-case-catalog]] --feeds--> [[mvp-deployment-architecture]]
- [[mvp-use-case-catalog]] --implements--> [[preserve-vs-replace-contract]]

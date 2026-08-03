---
type: technology
name: MVP Technology Stack
chapter: 11-New-System
scope: mvp
status: active
---

# MVP Technology Stack

## Runtime

| Layer | Choice | Rationale |
|-------|--------|-----------|
| Language | C# / **.NET 8** | Charter target; single stack for all apps |
| API host | ASP.NET Core Minimal APIs | Fast MVP, OpenAPI |
| Worker | `BackgroundService` + RabbitMQ.Client | Dispatcher pattern |
| Partner/Source | ASP.NET Core Minimal APIs | Same toolchain |

## Messaging

| Component | Technology |
|-----------|------------|
| Broker | **RabbitMQ 3.13** (management plugin) |
| Client | `RabbitMQ.Client` 6.x |
| Exchange | `rambase.events` (topic) + `bus.management` (fanout) |
| Serialization | `System.Text.Json` |
| MessageId | `{SystemId}-{RamBaseEventId}` preserved |

## Data

| Store | Technology | Contents |
|-------|------------|----------|
| Registry DB | **PostgreSQL 16** | webhooks, publish_cursor, api_keys (optional) |
| ORM | EF Core 8 or Dapper | MVP uses Npgsql + raw SQL for simplicity |

## Authentication & authorization

| Surface | Mechanism |
|---------|-----------|
| HTTP API | **API Key** header `X-Api-Key` mapped to role + `partnerId` (MVP) |
| Future | JWT Bearer (same role claims) |
| RabbitMQ | Username/password per vhost `rambase` |
| Partner webhook inbound | Optional **HMAC-SHA256** `X-Rambase-Signature` |
| Secrets | Docker env / `.env` — not committed |

### API key roles (env-configured)

```text
SOURCE_API_KEY   → role source
PARTNER_API_KEY  → role partner, partnerId demo-partner
ADMIN_API_KEY    → role admin
```

## Observability

| Tool | Usage |
|------|-------|
| `Microsoft.Extensions.Logging` | Structured console |
| `prometheus-net.AspNetCore` | `/metrics` on API + dispatcher |
| Health checks | `AspNetCore.HealthChecks.NpgSql`, RabbitMQ |

## Containerization

| Tool | Usage |
|------|-------|
| Docker | Multi-stage build per service |
| Docker Compose | Local orchestration |
| Optional later | K8s manifests from same images |

## Libraries (shared)

- `ServiceBus.Contracts` — DTOs, constants
- `ServiceBus.Messaging` — RabbitMQ publish/consume helpers

## Relationships

- [[mvp-technology-stack]] --implements--> [[target-platform-direction]]
- [[mvp-technology-stack]] --validates--> [[rabbitmq-routing-options]]

---
type: deployment
name: MVP Deployment Architecture
chapter: 11-New-System
scope: mvp
status: active
---

# MVP Deployment Architecture (Containerized Local)

> **Proposed design (pre-implementation):** `diagrams/mvp/00-proposed-mvp-architecture.md`  
> This note documents the **as-built** Docker Compose topology.

## Topology

```text
┌──────────────────────────────── docker-compose network: rambase-mvp ────────────────────────────────┐
│                                                                                                      │
│  ┌─────────────┐   emit events    ┌──────────────────┐    publish    ┌─────────────┐                │
│  │ source-app  │ ───────────────► │ service-bus-api  │ ────────────► │  rabbitmq   │                │
│  │ :5101       │   X-Api-Key      │ :8080            │             │ :5672       │                │
│  └─────────────┘                  │  • auth          │             │ :15672 UI   │                │
│                                   │  • /api/events   │             └──────┬──────┘                │
│  ┌─────────────┐   register       │  • /api/webhooks │                    │ consume               │
│  │ partner-app │ ───────────────► │  • /health       │                    ▼                       │
│  │ :5102       │                  └────────┬─────────┘             ┌──────────────────┐           │
│       ▲                               │                           │ service-bus-     │           │
│       │ webhook POST                  │ SQL                       │ dispatcher       │           │
│       │                               ▼                           │ (worker)         │           │
│       └───────────────────────────────┼───────────────────────────┴──────────────────┘           │
│                                       ▼                                                            │
│                              ┌─────────────────┐                                                   │
│                              │ postgres :5432  │                                                   │
│                              │ webhooks,cursor │                                                   │
│                              └─────────────────┘                                                   │
└────────────────────────────────────────────────────────────────────────────────────────────────────┘
```

## Service matrix

| Service | Image build | Ports | Depends on |
|---------|-------------|-------|------------|
| `postgres` | `postgres:16-alpine` | 5432 | — |
| `rabbitmq` | `rabbitmq:3.13-management-alpine` | 5672, 15672 | — |
| `service-bus-api` | `prototype/src/ServiceBus.Api` | 8080 | postgres, rabbitmq |
| `service-bus-dispatcher` | `prototype/src/ServiceBus.Dispatcher` | — (internal) | postgres, rabbitmq |
| `source-app` | `prototype/src/SourceApp` | 5101 | service-bus-api |
| `partner-app` | `prototype/src/PartnerApp` | 5102 | service-bus-api |

## RabbitMQ topology

```text
vhost: rambase

exchange rambase.events (topic)
  ├─ bind queue rambase.delivery  routing key: event.{systemId}
  └─ (future per-system shards)

exchange bus.management (fanout)
  └─ queue bus.management → dispatcher cache refresh

queue rambase.dlq  (dead letter from delivery after max retries)
```

## Auth flow diagram

```text
Source App                    Service Bus API                 Partner App
    │                              │                              │
    │ POST /api/events             │                              │
    │ X-Api-Key: $SOURCE_KEY       │                              │
    │─────────────────────────────►│ validate role=source         │
    │                              │                              │
    │                              │ POST /api/webhooks           │
    │                              │◄─────────────────────────────│
    │                              │ X-Api-Key: $PARTNER_KEY      │
    │                              │ validate role=partner        │
    │                              │                              │
    │                              │         POST /webhook        │
    │                              │─────────────────────────────►│
    │                              │ X-Rambase-EventID            │
    │                              │ X-Rambase-Signature (opt)    │
```

## Local test procedure

```bash
cd prototype
cp .env.example .env
docker compose up --build -d
docker compose run --rm demo-runner ./scripts/demo.sh
# or: ./scripts/demo.sh from host if curl available
```

## Verification checklist

- [ ] `curl http://localhost:8080/health` → healthy
- [ ] `curl http://localhost:5102/health` → healthy
- [ ] RabbitMQ UI http://localhost:15672 (guest only if enabled for dev)
- [ ] demo script prints partner received event JSON

## Production path (not MVP)

Same images → K8s Deployments + Secrets + Ingress; HA replicas for dispatcher with single-active lease (future).

## Relationships

- [[mvp-deployment-architecture]] --documents--> [[mvp/README]]
- [[mvp-deployment-architecture]] --validates--> [[mvp-use-case-catalog]]

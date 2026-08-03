# Service Bus MVP Prototype

Local containerized prototype: **source app → service bus (RabbitMQ) → partner webhook**.

## Architecture docs (vault)

- `vault/11-New-System/MVP/mvp-use-case-catalog.md`
- `vault/11-New-System/MVP/mvp-deployment-architecture.md`
- `vault/11-New-System/MVP/mvp-technology-stack.md`

## Prerequisites

- **Docker Desktop** with WSL2 backend — see [docs/DOCKER-TROUBLESHOOTING.md](docs/DOCKER-TROUBLESHOOTING.md) if Docker won't start
- Alternative without Docker: [docs/RUN-LOCAL-NO-DOCKER.md](docs/RUN-LOCAL-NO-DOCKER.md)
- Optional: .NET 8 SDK for local `dotnet run`

## Quick start

```bash
cd prototype
docker compose up -d
```

Wait ~30s, then open **demo UIs** in your browser:

| UI | URL |
|----|-----|
| Service Bus control panel | http://localhost:8080/ |
| Mock RamBase (source) | http://localhost:5101/ |
| Partner inbox | http://localhost:5102/ |

Full walkthrough: [docs/TESTING-GUIDE.md](docs/TESTING-GUIDE.md)

Session history (why things changed): [../diagrams/chat-history/INDEX.md](../diagrams/chat-history/INDEX.md)

Your questions log: [../diagrams/chat-history/USER-REQUEST-LOG.md](../diagrams/chat-history/USER-REQUEST-LOG.md)

CLI alternative:

```bash
./scripts/demo.sh
```

## Services

| Service | Port | Role |
|---------|------|------|
| service-bus-api | 8080 | Registry, publish API, auth |
| service-bus-dispatcher | — | Consume RabbitMQ, POST webhooks |
| source-app | 5101 | Mock RamBase event emitter |
| partner-app | 5102 | Mock integrator + webhook receiver |
| rabbitmq | 5672, 15672 | Message broker (UI: guest or rambase/rambase) |
| postgres | 5432 | Webhooks, cursor, DLQ |

## Authentication

| Key | Header | Role |
|-----|--------|------|
| `source-dev-key` | `X-Api-Key` | Publish events |
| `partner-dev-key` | `X-Api-Key` | Register webhooks |
| `admin-dev-key` | `X-Api-Key` | DLQ, all webhooks |

## Manual test

```bash
# Emit event via source-app
curl -X POST http://localhost:5101/emit/OrderCreated

# Check partner received
curl http://localhost:5102/received
```

## Stop

```bash
docker compose down -v
```

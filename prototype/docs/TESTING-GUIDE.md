# MVP Testing Guide (with Demo UIs)

Step-by-step guide to demo and test the full event → webhook flow using **browser UIs** or the CLI script.

## Architecture (what you’re proving)

```text
Source (5101)  →  API (8080)  →  RabbitMQ  →  Dispatcher  →  Partner (5102)
                      ↕ Postgres (webhooks, cursor, DLQ)
```

## Prerequisites

- Docker Desktop + WSL2 running
- Stack up: `docker compose up -d` from `prototype/`

Verify:

```bash
docker compose ps   # all 6 services Up
```

---

## Demo UIs (browser)

Open these in order for a live demo:

| Step | URL | Role |
|------|-----|------|
| Hub + API | http://localhost:8080/ | Publish events, register webhooks, view DLQ |
| 1 — Emit | http://localhost:5101/ | Mock RamBase — fire `OrderCreated` |
| 5 — Receive | http://localhost:5102/ | Partner inbox (auto-refreshes) |
| Broker (optional) | http://localhost:15672/ | RabbitMQ management (`rambase` / `rambase`) |

### Recommended demo script (2 minutes)

1. **Partner inbox** — open http://localhost:5102/ (note count = 0, auto-refresh on)
2. **Service Bus panel** — open http://localhost:8080/ → click **Refresh list** under Webhooks (partner registers on startup)
3. **Source app** — open http://localhost:5101/ → click **Emit event → Service Bus API**
4. **Partner inbox** — within ~2s count increases; table shows `DEMO-N` and JSON payload
5. **Optional** — RabbitMQ UI → Queues → `rambase.delivery` (message consumed)

### What each UI does

**http://localhost:8080/** (Service Bus control panel)

- Publish event (source API key)
- Register webhook (partner API key)
- List active webhooks
- Inspect dead-letter queue (admin API key)
- Links to source, partner, RabbitMQ

**http://localhost:5101/** (Mock RamBase)

- Pick event type, system, database
- POST to `/emit/{eventType}` → forwards to `POST /api/events`

**http://localhost:5102/** (External partner)

- Shows webhook POSTs received at `/webhook`
- Verifies HMAC (`demo-hmac-secret` in docker-compose)
- JSON API still available: `GET /received`

---

## CLI quick test

```bash
./scripts/demo.sh
```

Same flow without browsers — fails with hints if services are down.

---

## Manual curl (API reference)

```bash
# Health
curl http://localhost:8080/health
curl http://localhost:8080/ready

# Publish (source)
curl -X POST http://localhost:8080/api/events \
  -H "Content-Type: application/json" \
  -H "X-Api-Key: source-dev-key" \
  -d '{"systemId":"DEMO","eventType":"OrderCreated","database":"ALL","parameters":{"orderId":"abc"}}'

# List webhooks (partner)
curl -H "X-Api-Key: partner-dev-key" http://localhost:8080/api/webhooks

# Partner received
curl http://localhost:5102/received

# DLQ (admin)
curl -H "X-Api-Key: admin-dev-key" http://localhost:8080/api/dlq
```

---

## API keys (demo defaults)

| Key | Role | Header |
|-----|------|--------|
| `source-dev-key` | Publish events | `X-Api-Key` |
| `partner-dev-key` | Register webhooks | `X-Api-Key` |
| `admin-dev-key` | DLQ, all webhooks | `X-Api-Key` |

Pre-filled in the control panel UI. **Not for production.**

---

## Troubleshooting

| Symptom | Fix |
|---------|-----|
| Blank / failed UI API calls | `docker compose ps` — API, postgres, rabbitmq must be Up |
| Partner count stays 0 | Check dispatcher logs: `docker compose logs service-bus-dispatcher` |
| API crash loop | Postgres down → `docker compose up -d` |
| Partner inbox shows 4× same event | Duplicate webhook registrations — see below |
| Webhook not registered | Partner app logs on startup; or register via http://localhost:8080/ |

### Partner inbox: 4 records for 1 event

The dispatcher delivers **once per matching webhook**. Partner-app registers on every container restart, so rebuilds can create 4 identical webhooks → 4 POSTs → 4 inbox rows.

Check: http://localhost:8080/ → **Refresh list** under Webhooks.

Clean slate:

```bash
docker compose down -v
docker compose up -d --build
```

After rebuild, idempotent registration + dedupe by target URL prevents this going forward.

**Full reset:**

```bash
docker compose down -v
docker compose up -d --build
```

Wait ~30s, then open UIs again.

---

## Related docs

- [README.md](../README.md) — stack overview
- [DOCKER-TROUBLESHOOTING.md](DOCKER-TROUBLESHOOTING.md) — WSL / Docker issues
- [RUN-LOCAL-NO-DOCKER.md](RUN-LOCAL-NO-DOCKER.md) — without Docker
- `diagrams/mvp/00-proposed-mvp-architecture.md` — design diagram

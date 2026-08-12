# Session: 2026-08-11 — webhook routing & retry fix

Full user log: [USER-REQUEST-LOG.md](USER-REQUEST-LOG.md) (U-025).

## User requests (this session)

| ID | Category | You asked | Outcome |
|----|----------|-----------|---------|
| U-025 | troubleshooting | Register new webhook `http://localhost:53174/PrintEventReceiver.ashx` — publish sends unlimited requests to partner app, not the new webhook | Root cause + fixes in dispatcher; `host.docker.internal` guidance |

## Goal

Explain and fix why registering a host-machine webhook caused endless duplicate deliveries to the built-in partner inbox instead of the new endpoint.

## Decisions

| Decision | Rationale |
|----------|-----------|
| Skip already-delivered URLs on retry | Prevents partner spam when another webhook fails |
| Republish with incremented `x-retry-count` | RabbitMQ nack/requeue never updated retry header → infinite ~2s loop |
| Document `host.docker.internal` | `localhost` inside dispatcher container ≠ user's Windows host |

## Artifacts created / updated

| Path | Change |
|------|--------|
| `prototype/src/ServiceBus.Dispatcher/DispatcherWorker.cs` | Per-URL delivery tracking; proper retry cap; republish on failure |
| `prototype/src/ServiceBus.Api/InMemoryWebhookDispatcher.cs` | Same partial-delivery behavior for no-Docker path |
| `prototype/src/ServiceBus.Messaging/IMessageBus.cs` | `BusMessage.DeliveredUrls` |
| `prototype/src/ServiceBus.Messaging/InMemoryMessageBus.cs` | Requeue carries delivered URLs |
| `prototype/docs/TESTING-GUIDE.md` | Troubleshooting: host webhooks, endless partner inbox |
| `prototype/src/ServiceBus.Api/DemoUi.cs` | Default URL hint for `host.docker.internal` |

## Diagrams added/updated

- None

## Open items

- [ ] User rebuilds Docker stack: `docker compose up -d --build`
- [ ] Re-register webhook with `http://host.docker.internal:53174/PrintEventReceiver.ashx`
- [ ] Optionally delete partner-app webhook if testing only the custom endpoint

## Related

- [TESTING-GUIDE.md](../../prototype/docs/TESTING-GUIDE.md)
- Prior duplicate-webhook fix: U-020

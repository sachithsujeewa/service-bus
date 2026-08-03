# MVP — Platform Overview

**Runnable code:** `prototype/`  
**Vault specs:** `vault/11-New-System/MVP/`

## Local Docker stack

```text
┌─────────────┐     ┌─────────────┐     ┌─────────────┐
│  SourceApp  │     │  PartnerApp │     │   Browser   │
│  :5100      │     │  :5200      │     │  (optional) │
└──────┬──────┘     └──────┬──────┘     └─────────────┘
       │ POST /emit        │ POST /webhook (inbound)
       │ X-Api-Key         │ HMAC verify
       ▼                   ▼
┌──────────────────────────────────────────────────────┐
│              ServiceBus.Api  :5000                     │
│  /api/events  /api/webhooks  /api/dlq  /health        │
│  API key auth (source | partner | admin)              │
└───────────────┬──────────────────────┬───────────────┘
                │ publish              │ metadata (SQL)
                ▼                      ▼
       ┌─────────────┐         ┌─────────────┐
       │  RabbitMQ   │         │ PostgreSQL  │
       │  :5672      │         │  :5432      │
       └──────┬──────┘         └─────────────┘
              │ consume
              ▼
       ┌─────────────┐
       │ Dispatcher  │
       │  Worker     │──── HTTP POST ────► Partner webhook URL
       └─────────────┘
```

## Ports (docker-compose)

| Service      | Port  |
|-------------|-------|
| API         | 5000  |
| SourceApp   | 5100  |
| PartnerApp  | 5200  |
| RabbitMQ    | 5672, 15672 (mgmt) |
| PostgreSQL  | 5432  |

## Related

- `prototype/README.md`
- `diagrams/mvp/03-happy-path-sequence.md`

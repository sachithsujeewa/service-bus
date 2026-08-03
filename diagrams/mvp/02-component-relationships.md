# MVP — Component Relationships

**Projects:** `prototype/src/`

```text
┌─────────────────────────────────────────────────────────────────┐
│                     ServiceBus.Contracts                         │
│  EventEnvelope, WebhookRegistration, MessageTopology, DTOs        │
└────────────────────────────┬────────────────────────────────────┘
                             │ referenced by all
     ┌───────────────────────┼───────────────────────┐
     ▼                       ▼                       ▼
┌─────────────┐      ┌─────────────┐         ┌─────────────┐
│ Messaging   │      │ Api         │         │ Dispatcher  │
│ IMessageBus │◄────►│ REST + auth │         │ Worker      │
│ RabbitMQ    │      │ IDataStore  │         │ HTTP client │
│ InMemory    │      │ Postgres/   │         │ retry/DLQ   │
│             │      │ SQLite      │         │             │
└─────────────┘      └──────┬──────┘         └──────┬──────┘
                            │                       │
              ┌─────────────┴─────────────┐         │
              ▼                           ▼         │
       ┌─────────────┐             ┌─────────────┐  │
       │ SourceApp   │             │ PartnerApp  │  │
       │ emit events │             │ register +  │  │
       │ via API     │             │ receive     │  │
       └─────────────┘             └─────────────┘  │
                                                     │
              RabbitMQ exchange / queues ◄───────────┘
```

## Auth roles

```text
X-Api-Key: source-key   → POST /api/events, SourceApp /emit
X-Api-Key: partner-key  → POST /api/webhooks, PartnerApp lifecycle
X-Api-Key: admin-key    → /api/dlq, admin operations
```

## Related

- `vault/11-New-System/MVP/mvp-tech-stack.md`

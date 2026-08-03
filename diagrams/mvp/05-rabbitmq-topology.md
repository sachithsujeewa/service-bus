# MVP — RabbitMQ Topology

**Config:** `ServiceBus.Contracts/MessageTopology.cs`

## Exchanges and queues

```text
events.exchange (topic)
    │
    ├── routing: events.{systemId}.{eventType}
    │
    ▼
events.queue  ──► DispatcherWorker (consumer)

management.exchange (topic)
    │
    ├── routing: webhook.registered | webhook.removed
    │
    ▼
management.queue ──► (Api cache refresh / future consumer)

dlq.exchange
    │
    ▼
dlq.queue ──► persisted mirror in Postgres (admin API)
```

## Publish path

```text
Api.EventsController
     │
     ▼
RabbitMqMessageBus.PublishAsync
     │
     ▼
channel.BasicPublish(
  exchange: events.exchange,
  routingKey: events.{systemId}.{eventType},
  body: serialized EventEnvelope
)
```

## Dev mode (no Docker)

```text
Messaging:Provider = InMemory
     │
     ▼
InMemoryMessageBus (in-process channel)
     │
     ▼
InMemoryWebhookDispatcher (embedded in Api — WIP)
```

## Related

- `vault/11-New-System/Migration/rabbitmq-routing-options.md`
- `prototype/docs/DOCKER-TROUBLESHOOTING.md`

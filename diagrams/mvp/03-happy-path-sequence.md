# MVP — Happy Path Sequence

## End-to-end demo flow

```text
1. PartnerApp startup
   PartnerApp ──POST /api/webhooks──► Api
   Api stores registration in Postgres
   Api publishes WebhookRegistered (management plane)

2. Source emits mock RamBase event
   SourceApp ──POST /api/events──► Api
   Api validates API key + envelope
   Api publishes to RabbitMQ (events exchange)

3. Dispatcher consumes
   Dispatcher ◄──consume── RabbitMQ
   Dispatcher loads webhook registrations (filter match)
   Dispatcher ──POST HTTPS──► PartnerApp /webhook
   PartnerApp verifies HMAC signature

4. Success path
   PartnerApp ──200 OK──► Dispatcher
   Dispatcher marks delivery success (audit log)

5. Failure path (optional demo)
   PartnerApp returns 500
   Dispatcher retries with backoff
   After max attempts → DLQ row in Postgres
   Admin ──GET /api/dlq──► inspect failed deliveries
```

## Message envelope (conceptual)

```text
EventEnvelope
 ├── eventType      (e.g. CustomerCreated)
 ├── systemId       (e.g. SQLRIC)
 ├── ramBaseEventId (correlation)
 ├── properties     (P_* style payload map)
 └── timestamp
```

## Related

- `prototype/scripts/demo.sh`
- `diagrams/mvp/04-auth-flow.md`

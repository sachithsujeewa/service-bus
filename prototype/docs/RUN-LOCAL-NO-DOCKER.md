# Run MVP Without Docker

Use this when Docker Desktop cannot start (e.g. WSL not installed).

## Prerequisites

1. **.NET 8 SDK** — `dotnet --version`
2. **PostgreSQL** — local port 5432, create DB `rambase_bus`, user/password `rambase`
3. **RabbitMQ** — local port 5672, user `rambase`, password `rambase`, vhost `rambase`

Create vhost in RabbitMQ (management UI or CLI):

```text
User: rambase / rambase
Vhost: rambase
Permissions: configure/write/read .*
```

## Connection strings (all services)

```text
Host=localhost;Port=5432;Database=rambase_bus;Username=rambase;Password=rambase
RabbitMq:Host=localhost
RabbitMq:User=rambase
RabbitMq:Password=rambase
RabbitMq:VHost=rambase
```

## Start order (4 terminals)

### Terminal 1 — API

```bash
cd prototype/src/ServiceBus.Api
dotnet run --urls http://localhost:8080
```

### Terminal 2 — Dispatcher

```bash
cd prototype/src/ServiceBus.Dispatcher
dotnet run
```

### Terminal 3 — Partner (registers webhook on startup)

```bash
cd prototype/src/PartnerApp
dotnet run --urls http://localhost:5102
```

Set env:

```bash
export ServiceBus__BaseUrl=http://localhost:8080
export Partner__PublicWebhookUrl=http://localhost:5102/webhook
```

Windows PowerShell:

```powershell
$env:ServiceBus__BaseUrl="http://localhost:8080"
$env:Partner__PublicWebhookUrl="http://localhost:5102/webhook"
```

### Terminal 4 — Emit event

```bash
curl -X POST http://localhost:5101/emit/OrderCreated
# or start source-app:
cd prototype/src/SourceApp && dotnet run --urls http://localhost:5101
```

## Verify

```bash
curl http://localhost:5102/received
```

## When Docker is fixed

Prefer `docker compose up --build` — same stack, no manual DB/Rabbit setup.

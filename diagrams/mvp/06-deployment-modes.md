# MVP — Deployment Modes

## Mode A — Docker Compose (primary)

```text
docker compose up
     │
     ├── postgres (healthcheck)
     ├── rabbitmq (healthcheck)
     ├── api      (depends on both)
     ├── dispatcher (depends on rabbitmq + postgres)
     ├── source-app (depends on api)
     └── partner-app (depends on api)

demo: prototype/scripts/demo.sh
```

## Mode B — Local dev without Docker (planned / partial)

```text
dotnet run --project ServiceBus.Api
  Messaging:Provider=InMemory
  DataStore:Provider=SQLite

dotnet run --project SourceApp
dotnet run --project PartnerApp

Status: Program.cs dual wiring incomplete — see chat-history
```

## Mode C — Kubernetes (program target, not MVP code)

```text
Helm chart (future)
  ├── api deployment + ingress
  ├── dispatcher deployment (HPA)
  ├── broker operator or managed service
  └── external Postgres
```

## Docker blocker on Windows

```text
Docker Desktop
     │
     ▼
requires WSL2
     │
     ▼
wsl --install + reboot
     │
     └── OR use Mode B until WSL available
```

## Related

- `prototype/docs/DOCKER-TROUBLESHOOTING.md`
- `diagrams/mvp/01-platform-overview.md`

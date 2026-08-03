---
type: architecture
id: ARC-002
name: System Context and Boundaries
status: active
---

# System Context and Boundaries

## Context diagram

```text
                    ┌──────────────────┐
                    │ Integration      │
                    │ Partners (HTTPS) │
                    └────────▲─────────┘
                             │ webhook POST
                    ┌────────┴─────────┐
                    │ Service Bus Host │
                    │ Publishers       │
                    │ Subscribers      │
                    │ Manager          │
                    └────────┬─────────┘
           API calls         │ broker SDK
              ┌──────────────┼──────────────┐
              ▼              ▼              ▼
     ┌────────────┐  ┌────────────┐  ┌────────────┐
     │ RamBase    │  │ Service Bus│  │ Repository │
     │ ERP/API    │  │ Broker       │  │ NGSystem   │
     └────────────┘  └────────────┘  └────────────┘
```

## Inside the boundary

**Service Bus solution** includes:

- Windows service process(es) on bus host machines
- Publisher and subscriber assemblies
- EF / data access to PublishedEvent and repository databases
- Configuration appSettings (connection strings stored securely in deployment)

**RamBase ERP** includes:

- Business transactions that emit events
- Event archives and registration metadata
- Webhook registration APIs and WHA/WHT stores

## Outside the boundary

- Partner application logic and databases
- Corporate firewalls and allow-lists for outbound HTTP
- Azure/Windows Service Bus infrastructure administration (namespace, certificates)

## Trust boundaries

1. **ERP ↔ Bus** — API client credentials per system; read events, manage webhooks
2. **Bus ↔ Broker** — namespace connection string; topic publish/consume rights
3. **Bus ↔ Partner** — URL trust, TLS, optional shared secrets on HTTP

Credentials must not cross boundaries in logs or documentation vaults.

## NGSystem as integration hub

NGSystem table fields (conceptual):

| Field role | Purpose |
|------------|---------|
| System name | Unique system id (RIC, etc.) |
| SB_ID | Which bus instance serves the system |
| Client id/secret | API credentials for publisher/subscriber |

Polling NGSystem (~minute interval) drives **dynamic activation** without redeploying the service for each new system.

## Relationships

- [[system-context-and-boundaries]] --part_of--> [[architecture-overview]]
- [[system-context-and-boundaries]] --uses--> [[rambase-api-integration]]
- [[system-context-and-boundaries]] --stores_in--> [[data-architecture]]
- [[system-context-and-boundaries]] --activated_on--> [[activating-a-system]]

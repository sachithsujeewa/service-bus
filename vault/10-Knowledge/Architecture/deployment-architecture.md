---
type: architecture
id: ARC-005
name: Deployment Architecture
status: active
---

# Deployment Architecture

## Runtime topology

```text
┌──────────────────────────────────────────────┐
│ Bus host machine(s)                          │
│  Windows Service: Rambase Service Bus        │
│   - ServiceBusService instance(s)            │
│   - Publishers (per active system)           │
│   - Subscribers (per subscription)         │
│   - Manager consumer                         │
└──────────────────────────────────────────────┘
         │                    │
         ▼                    ▼
   Service Bus           SQL Server fleet
   namespace             (ERP, Repository, Bus DB)
```

## Instance identity

Each bus host carries a configurable **`ServiceBusId`** — must be unique when multiple bus instances exist. Topics and subscription names are largely **fixed constants** in configuration (`MainTopic`, `DeployTopic`, `ManagerSubscription`) to avoid accidental drift across environments.

## Environment dimensions

| Dimension | Typical variation |
|-----------|-------------------|
| Broker namespace | per environment (dev/test/prod) |
| SQL endpoints | per environment |
| ServiceBusId | per physical bus cluster |
| NGSystem rows | which systems active in env |

## Deploy topic coordination

Rolling out a new bus **version** uses DeployTopic messages so components:

- Stop safely
- Replace binaries or config
- Rebind subscriptions
- Resume publishers without manual per-subscription edits

Procedure detail: [[release-and-deployment]] and [[deployment-event-design]].

## SQL system activation

Activating or deactivating an **SQL-backed system** on the bus involves NGSystem `SB_ID` assignment and credential provisioning—see [[activating-a-system]].

## Network

- Outbound HTTPS to partner URLs from subscriber role
- Outbound API to RamBase from publisher/subscriber
- Broker ports (runtime vs. management) per Service Bus hosting model

## Failure isolation

- One misbehaving subscriber subscription should not block others (separate subscriptions).
- Publisher failure for one system should not stop other systems on same instance (per-system publisher isolation).

## Relationships

- [[deployment-architecture]] --part_of--> [[architecture-overview]]
- [[deployment-architecture]] --deployed_via--> [[release-and-deployment]]
- [[deployment-architecture]] --uses--> [[deployment-event-design]]
- [[deployment-architecture]] --activated_on--> [[activating-a-system]]

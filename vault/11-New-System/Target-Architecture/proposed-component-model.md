---
type: architecture-target
name: Proposed Component Model
status: draft
chapter: 11-New-System
---

# Proposed Component Model (Target Decomposition)

Draft decomposition for Service Bus 2.0 — separates concerns the monolith currently merges in `ServiceBusManager`.

## Principle

Split **publish path**, **dispatch path**, **management path**, and **coordination** so each can scale and deploy independently on K8s.

## Proposed services (logical)

| Component | Responsibility | Legacy analogue |
|-----------|----------------|-----------------|
| **System registry sync** | Poll NG_SYSTEM; emit system lifecycle events | `UpdateSystemList` in manager |
| **Event publisher** | EVR poll, ReadyFlag, cursor, broker publish | `RambaseSystemPublisher` |
| **Wake listener** | TCP 4-byte system id notification | `RBSTcpListener` |
| **Webhook dispatcher** | Consume routed messages, HTTP POST, retry | `WebHookSubscriber` + `Poster` |
| **Filter engine** | Evaluate webhook rules if not in broker | `SubscriptionFilters` + SqlFilter |
| **Management consumer** | Webhook CRUD → binding updates | `ServiceBusManagementSubscriber` |
| **Leader coordinator** | Active/passive or K8s-native HA | `CloudStateMonitor` |
| **Audit logger** | Structured logs, metrics, traces | `SBEventLogger` |

## Interface boundaries (to be specified)

Program placeholders exist for:

- REST/gRPC admin APIs ([[proposed-interfaces-gap]])
- Event contracts on wire ([[proposed-data-model-gap]])

## Data stores (unchanged conceptually)

- Per-system RamBase SQL (EVR, WHA, EVR_T1)
- Repository NG_SYSTEM
- RambaseServiceBus control DB (cursor, hosts, logs) — **may merge or split** in target

## Deployment units (options)

**Option 1 — Modular monolith (.NET 8)** single deployment with internal queues — faster MVP.

**Option 2 — Split publishers/dispatchers** — better scale for hot systems.

Decision ties to [[scalability-ceiling-analysis]] and M5 MVP scope.

## Relationships

- [[proposed-component-model]] --part_of--> [[target-platform-direction]]
- [[proposed-component-model]] --contrasts_with--> [[as-implemented-contract-summary]]

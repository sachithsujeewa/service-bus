---
type: migration
name: Preserve vs Replace Contract
status: active
chapter: 11-New-System
approval_required: true
---

# Preserve vs Replace Contract

Migration boundary: what integrators and operators **must keep** vs what engineers **should replace** when reimplementing the messaging spine.

## Preserve (business contract)

| Item | Rationale |
|------|-----------|
| Event property schema | `EventType`, `SystemID`, `Database`, `RamBaseEventId`, `P_*`, etc. |
| Sequential cursor in `PublishedEvent` | Recovery and ordering semantics |
| ReadyFlag wait + skip-after-timeout | Documented behavior — or explicitly change with comms |
| Webhook HTTP shape | JSON/XML body, `X-Rambase-EventID`, optional HMAC |
| Webhook filter semantics | Event type, database, ignore users, parameter filter |
| Management events for live webhook CRUD | Hot reconfiguration without restart |
| Multi-host active/passive model | Or **deliberate redesign** with signed ADR |
| Per-system topic isolation | Or explicit replacement design |
| TCP notification as fast-wake | Optional — can drop to poll-only with latency trade |

## Replace (broker-specific)

| Item | Replacement work |
|------|------------------|
| `NamespaceManager`, topic/sub provisioning | New broker admin API or GitOps |
| `TopicClient.Send` / `SubscriptionClient.OnMessage` | New client SDK |
| `BrokeredMessage` + `SqlFilter` | Headers exchange, in-process filter engine, or framework |
| STS connection string / ports | Cloud or K8s-native broker auth |
| Lock/abandon/complete | New ack/nack model |
| Built-in duplicate detection + ordering | Explicit idempotency store + queue design |

## Nice-to-fix during migration

- Secrets out of config → vault/K8s secrets
- Dead-letter + max delivery policy with alerting
- Clarify passive host publish behavior
- Structured logging, metrics, tracing
- Modern .NET 8 + container host
- Normalize payload to JSON message body (breaking change — needs version strategy)

## Integrator communication impact

Any change to:

- Property-only vs body payload
- Delivery guarantees (especially skip policy)
- HMAC or header names
- Ordering across failover

Requires **explicit migration notice** — charter mandates zero silent disruption.

## Relationships

- [[preserve-vs-replace-contract]] --preserves_contract_from--> [[as-implemented-contract-summary]]
- [[preserve-vs-replace-contract]] --requires--> [[modernization-charter]]
- [[preserve-vs-replace-contract]] --validates--> [[use-case-catalog-for-architects]]

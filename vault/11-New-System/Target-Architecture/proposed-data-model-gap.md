---
type: gap
name: Proposed Data Model Gap
status: draft
chapter: 11-New-System
---

# Proposed Data Model Gap

Program wiki **Service Bus Data & storage** is a placeholder.

## Documented in discovery (as-is)

See [[as-implemented-contract-summary]] and [[10-Knowledge/Architecture/data-architecture]] (legacy chapter — do not merge).

## Target documentation needed

| Artifact | Content |
|----------|---------|
| Wire message schema | JSON body vs properties decision |
| Cursor model | `PublishedEvent` evolution |
| Idempotency store | If broker dedup replaced |
| DLQ message schema | Operator inspection |
| GitOps manifest | Exchanges, queues, bindings per system |

## Message body normalization

Strong recommendation in migration analysis: move from property bag to **JSON body** for broker portability — **breaking change** requiring integrator versioning strategy.

## Relationships

- [[proposed-data-model-gap]] --requires--> [[preserve-vs-replace-contract]]
- [[proposed-data-model-gap]] --affects--> [[proposed-component-model]]

---
type: migration
name: Migration Pitfalls Matrix
status: active
chapter: 11-New-System
---

# Migration Pitfalls Matrix

Cross-reference for ASUC authoring — what breaks during broker migration if not explicitly designed.

| # | Pitfall | Detection | Mitigation direction |
|---|---------|-----------|---------------------|
| 1 | Filter semantics drift | Webhook never fires or wrong events | Golden filter test suite; WHA ↔ binding reconciliation job |
| 2 | Ordering break on failover | Duplicate or reversed entity updates | Per-stream single consumer; version COF; idempotent handlers |
| 3 | Dedup window change | Surge of duplicate POSTs after cutover | Idempotency store keyed on MessageId/EventId |
| 4 | Property size limits | Publish failures for large COF sets | Move payload to body; compress or trim |
| 5 | Passive host double-publish | Duplicate broker messages | Gate publishers on leader or distributed cursor lock |
| 6 | ReadyFlag policy change | Silent loss or delayed storms | Explicit policy ADR + integrator notice |
| 7 | Management event loss | WHA vs broker mismatch | Reconciliation cron; outbox for mgmt events |
| 8 | Lock/ack timeout mismatch | Message loss or infinite retry | Retune visibility timeout vs HTTP 30s |
| 9 | MD5 subscription names | Ops cannot map URL to queue | Human-readable naming in new system |
| 10 | Environment parity | Staging ≠ prod rules | GitOps for exchanges/bindings |
| 11 | Cursor manual edit | Skip or duplicate integrator events | Tooling for safe replay (new capability) |
| 12 | Secrets in migration config | Security incident | Secret store from day one |

## ASUC mapping

Each row should spawn at least one **failure-mode** or **migration-cutover** use case in test strategy M4/M5.

## Relationships

- [[migration-pitfalls-matrix]] --validates--> [[preserve-vs-replace-contract]]
- [[migration-pitfalls-matrix]] --explains--> [[migration-drivers-and-concerns]]

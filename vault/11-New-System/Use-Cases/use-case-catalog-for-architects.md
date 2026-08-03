---
type: use-case-catalog
name: Use Case Catalog for Architects
status: active
chapter: 11-New-System
---

# Use Case Catalog for Architects (ASUC Seeds)

Base catalog for writing **architecturally significant use cases**. Each entry is a seed — expand into full ASUC documents with actors, triggers, flows, quality scenarios, and acceptance criteria.

## How to write an ASUC from this vault

1. Pick a concern from [[architectural-concerns-register]]
2. Pick a seed below (or compose cross-cutting)
3. State **measurable** outcomes (latency p99, RTO, error rate, zero duplicate business effect)
4. Link **preserve** items from [[preserve-vs-replace-contract]]
5. Note **legacy vs target** chapter without merging narratives

## Catalog

| ID | Seed note | Primary concerns | Quality focus |
|----|-----------|------------------|---------------|
| ASUC-01 | [[uc-event-relay-happy-path]] | — | Performance, reliability |
| ASUC-02 | [[uc-failover-active-passive]] | CONC-003, CONC-006 | Availability |
| ASUC-03 | [[uc-webhook-hot-reconfiguration]] | CONC-013 | Reliability, operability |
| ASUC-04 | [[uc-broker-migration-poc]] | CONC-015 | Portability, reliability |
| ASUC-05 | [[uc-stuck-pipeline-evr-gap]] | CONC-018 | Reliability |
| ASUC-06 | [[uc-readyflag-skip-policy]] | CONC-005 | Reliability |
| ASUC-07 | [[uc-infinite-retry-dlq-policy]] | CONC-008 | Operability |
| ASUC-08 | [[uc-control-db-outage]] | CONC-003 | Availability |
| ASUC-09 | [[uc-peak-load-backlog]] | CONC-010 | Performance |
| ASUC-10 | [[uc-zero-disruption-cutover]] | Charter | Reliability, operability |

## Cross-cutting scenarios (not yet expanded)

- Partner verifies HMAC on high-volume stream
- Credential rotation without downtime
- Grafana alert on publisher lag SLO breach
- GitOps rollback of binding change
- Multi-system onboarding within 60s NGSystem sync

## Relationships

- [[use-case-catalog-for-architects]] --feeds--> [[architectural-concerns-register]]
- [[use-case-catalog-for-architects]] --requires--> [[preserve-vs-replace-contract]]

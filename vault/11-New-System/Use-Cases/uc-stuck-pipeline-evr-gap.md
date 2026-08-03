---
type: use-case-seed
id: ASUC-05
name: Stuck Pipeline EVR Gap
status: seed
chapter: 11-New-System
---

# ASUC-05: Stuck Pipeline on EVR Sequence Gap

## Intent

Specify operator and system behavior when EVR sequence has a gap (`eventId != cursor+1`).

## Trigger

Missing EVR row (purge, failed insert, data repair) while publisher expects strict sequential ids.

## As-is behavior

Publisher blocks retrying until gap resolved or sync-forward to `MIN(EVR.NO)` — **all webhooks for system stall**.

## Quality scenarios

| Scenario | Desired outcome |
|----------|-----------------|
| Detect stall | Alert within N minutes |
| Operator action | Runbook: repair EVR vs cursor advance |
| Target system | Optional skip policy with audit |

## Concerns

[[CONC-018]]

## Relationships

- [[uc-stuck-pipeline-evr-gap]] --explained_by--> [[as-implemented-contract-summary]]
- [[uc-stuck-pipeline-evr-gap]] --validates--> [[observability-gaps]]

---
type: use-case-seed
id: ASUC-06
name: ReadyFlag Skip Policy
status: seed
chapter: 11-New-System
---

# ASUC-06: ReadyFlag Timeout Skip

## Intent

Clarify integrator expectation when event never becomes ready within `ReadyFlagToleranceTimeInMilliSeconds`.

## As-is

Event **skipped**, cursor advanced, **no webhook** — intentional loss path.

## ASUC must document

- Whether integrators are notified (today: no)
- Whether target system changes to block vs skip
- Monitoring alert on skip count

## Concerns

[[CONC-005]]

## Relationships

- [[uc-readyflag-skip-policy]] --explained_by--> [[delivery-guarantees-analysis]]

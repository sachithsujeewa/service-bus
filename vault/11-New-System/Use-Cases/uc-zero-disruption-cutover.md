---
type: use-case-seed
id: ASUC-10
name: Zero Disruption Cutover
status: seed
chapter: 11-New-System
---

# ASUC-10: Zero-Disruption Migration Cutover

## Intent

Charter constraint: migrate broker and host **without breaking** existing integrator contracts.

## Strategies (evaluate in ASUC)

- Dual-write dual-read shadow period
- Per-system phased cutover
- Contract test gate on webhook HTTP shape
- Rollback within RTO

## Preserve checklist

From [[preserve-vs-replace-contract]] — all items must have verification tests.

## Concerns

All migration pitfalls [[migration-pitfalls-matrix]]

## Relationships

- [[uc-zero-disruption-cutover]] --requires--> [[modernization-charter]]
- [[uc-zero-disruption-cutover]] --validates--> [[preserve-vs-replace-contract]]

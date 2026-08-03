---
type: concern-register
name: Architectural Concerns Register
status: active
chapter: 11-New-System
---

# Architectural Concerns Register

Master register for **architecturally significant use case** authoring. Each concern links to quality attributes and ASUC seeds.

## Concern index

| ID | Concern | Quality attributes | Severity | Note |
|----|---------|-------------------|----------|------|
| CONC-001 | Platform EOL (MS SB 1.1, .NET Framework) | Maintainability, portability | Critical | [[migration-drivers-and-concerns]] |
| CONC-002 | Broker-side SQL filter lock-in | Portability, testability | Critical | [[rabbitmq-routing-options]] |
| CONC-003 | RambaseServiceBus SQL as HA SPOF | Availability | Critical | [[ha-dr-concerns]] |
| CONC-004 | Passive host may still publish | Reliability | High | [[as-implemented-contract-summary]] |
| CONC-005 | Silent event loss paths | Reliability | High | [[delivery-guarantees-analysis]] |
| CONC-006 | ~60s failover RTO gap | Availability | High | [[ha-dr-concerns]] |
| CONC-007 | Plaintext secrets in config | Security | Critical | [[security-debt-register]] |
| CONC-008 | No DLQ / infinite retry | Operability, reliability | High | [[delivery-guarantees-analysis]] |
| CONC-009 | Observability gaps (no metrics/trace) | Operability | High | [[observability-gaps]] |
| CONC-010 | Scale ceiling (1 POST/URL, 1 active host) | Performance, scalability | High | [[scalability-ceiling-analysis]] |
| CONC-011 | No replay / dangerous cursor edit | Operability, DR | High | [[ha-dr-concerns]] |
| CONC-012 | Property-bag message format | Portability | Medium | [[proposed-data-model-gap]] |
| CONC-013 | WHA ↔ broker rule drift | Reliability | High | [[migration-pitfalls-matrix]] |
| CONC-014 | ParameterFilter injection risk | Security | Medium | [[security-debt-register]] |
| CONC-015 | Broker decision unresolved | Program risk | High | [[broker-decision-pending]] |
| CONC-016 | Charter vs scorecard broker ranking | Governance | Medium | [[broker-evaluation-summary]] |
| CONC-017 | Placeholder NFR/security/observability pages | Completeness | Medium | Target chapter gaps |
| CONC-018 | EVR sequence gap blocks pipeline | Reliability | High | [[uc-stuck-pipeline-evr-gap]] |

## Quality attribute coverage

```text
Availability    → CONC-003, CONC-006, CONC-011
Reliability     → CONC-004, CONC-005, CONC-008, CONC-013, CONC-018
Security        → CONC-007, CONC-014
Performance     → CONC-010
Operability     → CONC-008, CONC-009, CONC-011
Portability     → CONC-001, CONC-002, CONC-012
Maintainability → CONC-001, CONC-017
```

## ASUC authoring rule

Each ASUC should cite ≥1 `CONC-###` and state **measurable** response (metric, RTO, policy).

## Discovery agent

Refresh this register on each [[04-Agent-Skills/13-new-system-discovery-skill]] run.

## Relationships

- [[architectural-concerns-register]] --derived_from--> [[migration-drivers-and-concerns]]
- [[architectural-concerns-register]] --feeds--> [[use-case-catalog-for-architects]]

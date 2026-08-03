---
type: map
name: New System Map
chapter: 11-New-System
status: active
---

# New System Chapter Map

Navigation for modernization / target architecture — **isolated from** [[01-master-knowledge-map]].

## Program

- [[modernization-charter]]
- [[milestone-roadmap-m1-m5]]

## Discovery (as-implemented truth for migration)

- [[as-implemented-contract-summary]]
- [[migration-drivers-and-concerns]]

## Migration

- [[preserve-vs-replace-contract]] ← **contract boundary**
- [[broker-evaluation-summary]]
- [[rabbitmq-routing-options]] (candidate only)
- [[migration-pitfalls-matrix]]

## Target architecture (draft)

- [[target-platform-direction]]
- [[proposed-component-model]]
- [[proposed-interfaces-gap]]
- [[proposed-data-model-gap]]

## Concerns (ASUC inputs)

- [[architectural-concerns-register]] ← **hub**
- [[delivery-guarantees-analysis]]
- [[ha-dr-concerns]]
- [[security-debt-register]]
- [[observability-gaps]]
- [[scalability-ceiling-analysis]]

## Decisions (pending)

- [[broker-decision-pending]]
- [[routing-strategy-options]]

## Use cases (ASUC seeds)

- [[use-case-catalog-for-architects]] ← **start for ASUC writing**
- ASUC-01 through ASUC-10 notes in `Use-Cases/`

## Agent

- [[04-Agent-Skills/13-new-system-discovery-skill]]
- [[05-Agent-Prompts/02-scan-new-system-folder]]

## Reading path for architects

```text
modernization-charter
  → as-implemented-contract-summary
  → architectural-concerns-register
  → preserve-vs-replace-contract
  → use-case-catalog-for-architects
  → expand ASUC seeds
```

## Relationships

- [[04-new-system-map]] --contrasts_with--> [[01-master-knowledge-map]]
- [[04-new-system-map]] --documents--> [[05-architecture-chapter-comparison]]

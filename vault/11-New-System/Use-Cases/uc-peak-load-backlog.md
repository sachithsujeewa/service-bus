---
type: use-case-seed
id: ASUC-09
name: Peak Load Backlog
status: seed
chapter: 11-New-System
---

# ASUC-09: Peak Load and Backlog

## Intent

Capacity ASUC for high event rate — sequential POST per URL and single active host.

## Stress factors

- Events/sec on hot system
- Many webhooks on same URL (filter union)
- Slow partner response near 30s timeout

## Measures

- Max sustainable publish rate per system
- Backlog depth vs publisher lag metric
- p99 delivery latency under load

## Concerns

[[CONC-010]]

## Relationships

- [[uc-peak-load-backlog]] --explained_by--> [[scalability-ceiling-analysis]]

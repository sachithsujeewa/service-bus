---
type: concern-analysis
name: Scalability Ceiling Analysis
status: active
chapter: 11-New-System
---

# Scalability Ceiling Analysis

Where growth hits walls in as-implemented system — drives target decomposition.

## Hard ceilings

| Dimension | Limit | Bottleneck |
|-----------|-------|------------|
| Concurrent POST per URL | **1** | `MaxConcurrentCalls=1` per subscription |
| Active webhook consumers | **1 host** entire cloud | CSM active/passive |
| Publisher throughput per system | Sequential EVR ids | Single publisher thread per system |
| Poll batch | ~26 events hardcoded | Code constant |
| Internal task pool | **12 threads** | Hardcoded `QueuedTaskScheduler` |
| Broker property size | Broker-specific | Large `EVR_T1` param sets |

## Scaling dimensions that work

- **More RamBase systems** — one topic + publisher per system (count scales, not hot system)
- **More unique webhook URLs** — more subscriptions (ops complexity grows)

## What does not scale

- Hot integrator URL needing parallel delivery
- Event rate spike on single system without backlog latency growth
- Cross-region fan-out

## Target implications

| Need | Architectural response |
|------|------------------------|
| Higher POST throughput to one URL | Competing consumers + idempotency store |
| Faster failover | Reduce 60s delay; active/active with caution |
| Hot system | Partition streams or shard publisher |
| K8s horizontal pod scale | Split dispatcher from publisher |

Ties to [[proposed-component-model]] Option 2.

## ASUC seeds

- Peak order day — backlog and latency
- Integrator demands 10× webhook volume — capacity planning

## Relationships

- [[scalability-ceiling-analysis]] --validates--> [[CONC-010]]
- [[scalability-ceiling-analysis]] --affects--> [[target-platform-direction]]

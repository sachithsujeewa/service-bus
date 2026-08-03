---
type: program
name: Modernization Charter
status: active
chapter: 11-New-System
---

# Modernization Charter

## Project

**RamBase Infrastructure Modernization: Service Bus Redesigning**

| Role | Name |
|------|------|
| Sponsor | Jonas Pettersson |
| Technical owner | Ragnar Wiik Johansen |
| Product / PM | Sajith Hettiarachchi |
| Architect | Kamil Polikiewicz |

**Status:** Started (as of program export 2026-07)

## Objective

Redesign and modernize RamBase Service Bus by:

1. Migrating from **legacy Microsoft Service Bus 1.1** (on-prem) to a **new broker layer** (candidate under evaluation)
2. Porting `RambaseServiceBus.exe` to **.NET 8**
3. Enabling **containerization** and **Kubernetes** alignment
4. Maintaining **zero disruption** to existing message flows and webhook integrations

## Strategic constraint

This is a **Service Bus domain** project — not a general VM-to-K8s migration. The Windows-only broker dependency is the technical blocker for containerization.

## Intended outcomes (technical)

| Outcome | Mechanism |
|---------|-----------|
| Decoupled integration | Async messaging instead of point-to-point |
| Standardized integration layer | Common protocols (REST, AMQP, etc.) and formats |
| Scalability | Horizontal broker and service scaling |
| Reliability | Queues, retry, **dead-letter discipline** (target state) |
| Observability | Central message flow, failure, latency tracking |
| Security | Secret management, hardened trust model |

## Scope

**In scope:** Modernize messaging platform; improve failure handling, monitoring, scalability, deployment; RabbitMQ as **primary candidate** (not final decision).

**Out of scope:** New business features or integrations that do not exist today.

## Relationship to legacy chapter

- [[modernization-charter]] --targets_replacement_of--> [[10-Knowledge/Technology/service-bus-technology]]
- [[modernization-charter]] --contrasts_with--> [[10-Knowledge/Architecture/deployment-architecture]]
- [[modernization-charter]] --requires--> [[preserve-vs-replace-contract]]

## ASUC relevance

Charter constraints define **non-negotiables** for every architecturally significant use case: zero integration disruption, preserve webhook HTTP contract, explicit delivery semantics.

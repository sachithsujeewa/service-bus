---
type: chapter-index
name: New System Chapter
chapter: 11-New-System
status: active
boundary_policy: isolated-from-10-Knowledge
---

# Chapter 11 — New System (Modernization & Target Architecture)

## Chapter boundary

This chapter documents **proposed modernization**, **migration discovery**, and **target architecture direction** for RamBase Service Bus 2.0. It is intentionally **separate** from `10-Knowledge/` (legacy / as-documented architecture).

```text
10-Knowledge/     → Baseline knowledge graph (historical + synthesized ERP/bus model)
11-New-System/    → Modernization program, as-implemented discovery, target design, ASUC seeds
```

Do not merge notes across chapters without explicit `contrasts_with` or `targets_replacement_of` links.

## Purpose for architects

Use this chapter as the **base knowledge for architecturally significant use cases (ASUCs)**:

- What must be preserved in migration (business contract)
- What must be redesigned (broker coupling, HA, ops)
- Open decisions (broker, routing, HA model)
- Concern register for quality-attribute scenarios

## Start here

1. [[modernization-charter]]
2. [[architectural-concerns-register]]
3. [[preserve-vs-replace-contract]]
4. [[use-case-catalog-for-architects]]
5. [[04-new-system-map]]

## Subfolders

| Folder | Content |
|--------|---------|
| `Program/` | Charter, milestones M1–M5 |
| `Discovery/` | As-implemented analysis from codebase contract |
| `Target-Architecture/` | K8s, .NET 8, component direction |
| `Migration/` | Broker evaluation, pitfalls, preserve/replace |
| `Concerns/` | Quality-attribute and risk registers |
| `Decisions/` | Pending architecture decisions |
| `Use-Cases/` | ASUC catalog and scenario seeds |

## Agent maintenance

Run [[04-Agent-Skills/13-new-system-discovery-skill]] against `_intake/new system/` when sources update.

## Relationships

- [[README]] --contrasts_with--> [[10-Knowledge/Architecture/architecture-overview]]
- [[README]] --documents--> [[01-new-system-source-catalog]]

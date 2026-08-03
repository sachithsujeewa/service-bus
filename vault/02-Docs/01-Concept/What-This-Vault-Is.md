---
type: documentation
category: concept
name: What This Vault Is
status: active
---

# What This Vault Is

## Purpose

This vault is the **working knowledge graph** for RamBase push events, webhooks, and Service Bus operations. It answers:

- **Why** push events exist and who consumes them
- **What** events, subscriptions, and webhooks mean in RamBase
- **How** messages flow from ERP activity to subscriber endpoints
- **How to** register events, activate systems, deploy, and operate

## What it is not

- Not a copy of `_intake/` Confluence exports
- Not a credential or configuration store (secrets stay out of notes)
- Not a substitute for live system documentation when behavior changes

## Two architecture chapters

- `10-Knowledge/` — baseline knowledge from historical documentation synthesis
- `11-New-System/` — modernization program, **as-implemented codebase discovery**, target architecture, architecturally significant use case (ASUC) seeds

These chapters are **intentionally isolated**. See [[30-Maps/05-architecture-chapter-comparison]].

## Bootstrap method

The vault was created by applying the agentic skill pipeline from `base/`:

1. Catalog raw sources in `_intake/` (structure only)
2. Classify topics into the target documentation hierarchy
3. Synthesize deep structured notes without transcribing source prose
4. Wire typed relationships and maps
5. Record execution trace in `01-Processing/`

## Traceability model

Each knowledge note can link upstream to business goals and downstream to implementation patterns. Identifier prefixes:

| Prefix | Layer |
|--------|-------|
| `BUS-` | Business context |
| `CON-` | Domain concept |
| `ARC-` | Architecture |
| `DES-` | Design |
| `TEC-` | Technology |
| `IMP-` | Implementation pattern |
| `OPS-` | Operator procedure |
| `RULE-` | Rule or constraint |
| `DEC-` | Recorded decision |

## Relationships

- [[What This Vault Is]] --explains--> [[30-Maps/01-master-knowledge-map]]
- [[What This Vault Is]] --derived_from--> [[base/02-Docs/01-Concept/01-What-This-Vault-Is]]

---
type: vault-home
name: RamBase Service Bus Knowledge Vault
domain: service-bus-webhooks-events
status: active
created: 2026-07-30
synthesis_policy: deep-synthesized-from-intake-structure
---

# RamBase Service Bus Knowledge Vault

Structured Obsidian knowledge graph for RamBase events, webhooks, and the Service Bus integration platform.

This vault was bootstrapped using the **Generic Agentic Obsidian Vault** flow (`base/`). Raw historical exports remain in `_intake/` and are **not** duplicated here. Notes in `10-Knowledge/` are **deep synthesized** technical knowledge — architecture, design, and operations written for engineers rediscovering the system.

## Knowledge chain

```text
Business need
   ↓
Domain concepts
   ↓
Architecture
   ↓
Design
   ↓
Technology
   ↓
Implementation patterns
   ↓
Operator guidance
```

## Start here

- [[02-Docs/01-Concept/What-This-Vault-Is]]
- [[30-Maps/01-master-knowledge-map]]
- [[30-Maps/02-architecture-map]]
- [[10-Knowledge/Architecture/architecture-overview]]

## Folder map

```text
00-Inbox          Source catalog stubs (pointers to _intake archive)
01-Processing     Skill execution traces from vault bootstrap
02-Docs           Vault concept and usage documentation
03-Templates      Note templates for ongoing agent maintenance
04-Agent-Skills   Skill specs (mirrors base framework)
05-Agent-Prompts  Domain operating prompts
10-Knowledge      Legacy / baseline structured knowledge (DO NOT merge with 11)
11-New-System     Modernization chapter — isolated from 10-Knowledge
  Program/ Discovery/ Migration/ Target-Architecture/
  Concerns/ Decisions/ Use-Cases/
20-Relationships  Typed link vocabulary
30-Maps           Overview and navigation maps
40-Reviews        Approval and review notes
90-Archive        Deprecated notes
99-Graph          Graph metadata (graph.json + new-system-graph.json)
```

## Two architecture chapters

| Chapter | Start here | Purpose |
|---------|------------|---------|
| Legacy baseline | [[30-Maps/01-master-knowledge-map]] | Domain concepts, synthesized legacy architecture |
| New system / modernization | [[11-New-System/README]] | Migration discovery, target direction, **ASUC seeds** |

See [[30-Maps/05-architecture-chapter-comparison]].

## Relationship to other folders

| Folder | Role |
|--------|------|
| `base/` | Generic agentic vault framework and skill pipeline |
| `_intake/` | Historical Confluence export archive (read-only evidence) |
| `vault/` | This structured knowledge graph (working vault) |

## Open in Obsidian

Open the `vault` folder as an Obsidian vault. Use the graph view with `99-Graph/graph.json` for typed node coloring.

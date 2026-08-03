# Documentation Maturity and Traceability Model

## Purpose

This model scores how completely and clearly the **historical documentation** expresses each layer. It does not score the live system and does not introduce new requirements.

## Levels

| Level | Meaning | Required evidence |
|---|---|---|
| 0 — Missing | No relevant source content found | Gap record |
| 1 — Fragmented | Isolated, repeated, ambiguous, or mixed-layer content exists | Source references |
| 2 — Classified | All content is assigned to the correct layer | Source-to-target map |
| 3 — Structured | Content is consolidated, readable, deduplicated, and faithful | Reviewed professional document |
| 4 — Traceable | Provenance and upstream/downstream relationships are explicit | Complete traceability records |
| 5 — Maintained | Ownership and review keep structure and traceability current | Change control and review history |

## Documentation layers to assess

| Layer | Questions answered | Initial estimate | Target |
|---|---|---:|---:|
| Business context and goals | Why does this exist, for whom, and for what outcome? | 1 | 4 |
| Requirements | What behaviour, rules, and constraints are explicitly stated? | 1 | 4 |
| Domain and concepts | What do events, webhooks, subscriptions, and related terms mean? | 2 | 4 |
| Architecture | What are the boundaries, components, responsibilities, flows, and topology? | 1 | 4 |
| Design | How do publishing, routing, delivery, errors, ordering, and deployment flows work? | 1 | 4 |
| Technology | Which products, APIs, protocols, stores, and hosting mechanisms are described? | 1 | 4 |
| Implementation | Which tables, classes, COFs, resources, fields, settings, and procedures realize the design? | 1 | 4 |
| User/operator guidance | How do users and operators configure, use, release, and troubleshoot it? | 1 | 4 |
| Source provenance | Can every rewritten statement be traced to original content? | 1 | 4 |
| Cross-layer traceability | Can a reader follow business need through implementation? | 0 | 4 |
| Questions and gaps | Are source silence, contradictions, and unresolved questions visible? | 1 | 4 |
| Editorial governance | Are meaning changes prevented and reviewed? | 0 | 4 |

The initial scores are hypotheses based on the exported pages. They describe documentation condition, not implementation quality.

## Layer-specific maturity chain

For each topic—such as event creation, webhook delivery, subscription handling, error handling, or deployment events—assess this chain:

`Business need → Requirement → Architecture → Design → Technology → Implementation → Guidance`

A topic reaches level 4 only when:

- all relevant source statements are classified;
- every rewritten statement retains source provenance;
- explicit source requirements are distinguishable from proposals and questions;
- connections between adjacent layers are recorded;
- missing layers are shown as gaps rather than filled by assumption.

## Scoring rules

- Score each layer at the lowest fully satisfied level.
- Overall maturity is the lowest critical-layer score, not an average.
- A layer with polished prose but missing provenance cannot exceed level 3.
- A layer containing invented requirements cannot pass review and must be corrected.
- Level 5 is reached only after ownership and repeated review demonstrate maintainability.

## Assessment process

1. Inventory all source units.
2. Score each target layer before restructuring.
3. Attach evidence and list blockers to the next level.
4. Convert blockers into documentation backlog items.
5. Re-score after content mapping, rewriting, traceability, and review.
6. Record score changes and evidence in the maturity assessment.

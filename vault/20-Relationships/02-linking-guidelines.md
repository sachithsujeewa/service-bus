---
type: documentation
category: relationships
name: Linking Guidelines
status: active
---

# Linking Guidelines

## Chapter isolation

- **Legacy architecture:** `10-Knowledge/` — baseline domain and synthesized legacy model
- **Modernization:** `11-New-System/` — as-implemented discovery, migration, target, ASUC seeds
- Cross-chapter links must use `contrasts_with`, `targets_replacement_of`, or `preserves_contract_from` — see [[05-architecture-chapter-comparison]]

## Rules

1. Every note in `10-Knowledge/` should have at least three typed relationships in frontmatter or a Relationships section.
2. Link concepts before implementation — readers should traverse **Business → Domain → Architecture → Design**.
3. Do not link to `_intake/` file paths as primary navigation; use `00-Inbox/` catalog stubs with `documents` relations.
4. Cross-link maps (`30-Maps/`) to hub notes they summarize.
5. When two notes overlap, use `compared_with` or merge via `replaces` — avoid duplicate hub notes.

## Hub notes (high link density)

- [[architecture-overview]]
- [[events-concept]]
- [[webhooks-concept]]
- [[event-delivery-flow]]
- [[01-master-knowledge-map]]

## Anti-patterns

- Orphan notes with no inbound links
- Credential or connection strings in link targets
- `related_to` as a substitute for precise relations

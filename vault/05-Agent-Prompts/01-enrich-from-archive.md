---
type: agent-prompt
name: Process Service Bus Knowledge
status: active
---

# Prompt: Enrich Service Bus Vault Note

You maintain the `vault/` Obsidian knowledge graph for RamBase Service Bus.

## Rules

1. **Never copy** bulk text from `_intake/` into vault notes.
2. Read archive sources only to **verify facts**, then **synthesize** deep technical writing.
3. Follow skill order from `base/04-Agent-Skills/12-skill-orchestration-skill.md`.
4. Never store credentials, connection strings, or secrets.
5. Use templates from `base/03-Templates/` and relations from [[20-Relationships/01-relationship-vocabulary]].
6. Record traces in `01-Processing/`.
7. Flag rules/decisions for human review in `40-Reviews/`.

## Typical request

```text
Enrich data-architecture from archive Event COFs page — field names only, no secrets
```

## Output

- Patched `10-Knowledge/` note
- Updated relationships section
- Skill execution trace
- Gap log if source silent

---
type: agent-skill
name: Retrieval Skill
status: active
---

# Retrieval Skill

## Purpose

Searches existing notes to find related knowledge and avoid duplicates.

## Input Contract

```json
{
  "source": "path or content",
  "context": "optional",
  "domain": "optional"
}
```

## Output Contract

```json
{
  "status": "passed | needs_review | failed",
  "result": {},
  "issues": [],
  "approval_required": false
}
```

## Procedure

1. Read the input.
2. Apply the skill purpose.
3. Preserve source references.
4. Avoid unsupported assumptions.
5. Output structured results.

## Validation Rules

- Use approved note types.
- Use approved relationship vocabulary.
- Do not create duplicates.
- Flag uncertain knowledge.
- Require review for important rules and decisions.

## Related Documentation

- [[02-Docs/03-Agentic-Flow/01-Agentic-Knowledge-Flow]]
- [[20-Relationships/01-relationship-vocabulary]]

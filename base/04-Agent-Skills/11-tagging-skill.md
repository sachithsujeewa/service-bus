---
type: agent-skill
name: Tagging Skill
status: active
---

# Tagging Skill

## Purpose

Adds useful tags to notes so humans and agents can quickly filter, search, and group related knowledge.

Tags should support discovery. They should not replace YAML metadata or typed relationships.

## Input Contract

```json
{
  "note_path": "path to note",
  "note_type": "concept | process | rule | decision | example | raw-note | other",
  "domain": "optional domain",
  "content": "note content",
  "mode": "suggest | apply"
}
```

## Output Contract

```json
{
  "status": "passed | needs_review | failed",
  "result": {
    "suggested_tags": [],
    "applied_tags": [],
    "rejected_tags": []
  },
  "issues": [],
  "approval_required": false
}
```

## Procedure

1. Read the note metadata and content.
2. Identify the note type.
3. Identify the domain or subject area.
4. Identify important technologies, methods, entities, or themes.
5. Generate a small set of tags.
6. Prefer stable, reusable tags over one-off tags.
7. Add tags to YAML frontmatter when applying tags.
8. Avoid duplicating information already expressed better as a relationship.
9. Validate tag style.
10. Summarize added or suggested tags.

## Tag Style

Use lowercase kebab-case:

```text
obsidian
knowledge-management
architecture
markdown
agentic-workflow
```

Avoid:

```text
Obsidian
Knowledge Management
knowledge_management
very-long-specific-tag-that-will-never-be-reused
```

## Where Tags Go

Put tags in YAML frontmatter:

```yaml
tags:
  - obsidian
  - knowledge-management
  - architecture
```

## Recommended Tag Types

Use a small number of tags from these categories:

- Domain tags: `knowledge-management`, `education`, `software-engineering`
- Tool tags: `obsidian`, `git`, `markdown`
- Structure tags: `concept`, `process`, `rule`, `decision`, `example`
- Workflow tags: `agentic-workflow`, `inbox-processing`, `review`
- Topic tags: `architecture`, `metadata`, `knowledge-graph`

## Validation Rules

- Do not add more than 3 to 6 tags unless the note is broad.
- Do not add tags that duplicate every relationship target.
- Do not use spaces in tags.
- Do not use uppercase tags.
- Do not use punctuation except hyphens.
- Prefer existing tags if the vault already has a related tag.
- If the note is high-stakes or domain-sensitive, suggest tags instead of applying them automatically.

## Example

Input note:

```text
Obsidian Vault Architecture
```

Suggested tags:

```yaml
tags:
  - obsidian
  - knowledge-management
  - architecture
  - markdown
```

## Related Documentation

- [[02-Docs/01-Concept/02-Obsidian-Compatible-Structure]]
- [[04-Agent-Skills/05-markdown-generation-skill]]
- [[04-Agent-Skills/07-validation-skill]]


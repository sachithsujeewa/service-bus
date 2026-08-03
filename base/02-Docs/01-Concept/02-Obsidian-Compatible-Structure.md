---
type: documentation
category: concept
name: Obsidian-Compatible Structure
status: active
---

# Obsidian-Compatible Structure

An Obsidian-compatible structure means the vault follows conventions that Obsidian understands.

## It uses

```text
Markdown files
Folders
[[wiki links]]
YAML frontmatter
#tags
Attachments
```

## It does not require

```text
Obsidian UI
Obsidian SDK
Obsidian CLI
Obsidian plugins
```

## Example note

```markdown
---
type: concept
name: Learning Outcome
domain: Education
status: active
---

# Learning Outcome

A learning outcome describes what a learner should be able to do.

## Relationships

- [[Learning Outcome]] --belongs_to--> [[Course]]
- [[Assessment]] --validates--> [[Learning Outcome]]
```

## Why this matters

Because the vault is plain files, it can be managed by:

- Obsidian
- VS Code
- Cursor
- Git
- Scripts
- AI agents
- Any Markdown editor

Obsidian becomes one possible viewer, not the whole system.

---
type: documentation
category: how-to
name: How To Use This Vault
status: active
---

# How To Use This Vault

## Step 1: Put raw notes into `00-Inbox`

Use `00-Inbox` for unprocessed knowledge.

Examples:

- Meeting notes
- Lecture notes
- Research notes
- Customer discussions
- Ideas
- Code observations
- Project updates
- Reading notes

## Step 2: Convert raw notes into structured notes

Move knowledge from the inbox into the correct folders under `10-Knowledge`.

Use templates from `03-Templates`.

## Step 3: Add YAML metadata

Each note should have frontmatter.

Example:

```markdown
---
type: concept
name: Example Concept
domain: General
status: active
source: 00-Inbox/raw-note.md
---
```

## Step 4: Add relationships

Use this relationship format:

```markdown
- [[Source]] --relation--> [[Target]]
```

Example:

```markdown
- [[Concept A]] --requires--> [[Concept B]]
```

## Step 5: Build maps

Use `30-Maps` for overview notes.

Maps help humans and agents see how knowledge connects.

## Step 6: Review important knowledge

Use `40-Reviews` when knowledge needs approval or validation.

Important knowledge should not be blindly accepted.

## Step 7: Archive old knowledge

Use `90-Archive` for deprecated or old notes.

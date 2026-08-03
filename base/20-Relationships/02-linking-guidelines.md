---
type: guideline
name: Linking Guidelines
status: active
---

# Linking Guidelines

## Use wiki links

Use:

```markdown
[[Concept Name]]
```

## Use typed relationships

Use:

```markdown
- [[Source]] --relation--> [[Target]]
```

## Keep relationships meaningful

Weak:

```markdown
- [[Concept A]] --related_to--> [[Concept B]]
```

Better:

```markdown
- [[Concept A]] --requires--> [[Concept B]]
```

## Avoid too many relationship names

Use the approved vocabulary where possible.

## Direction matters

Write the relationship in a direction that makes sense.

Example:

```markdown
- [[Foreign Key]] --requires--> [[Primary Key]]
```

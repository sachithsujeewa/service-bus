---
type: example
name: Decision Note Example
status: active
---

# Decision Note Example

## Situation

A team chooses one approach over another.

## Decision note structure

```markdown
---
type: decision
name: Decision Name
domain: General
status: proposed
approval_required: true
---

# Decision Name

## Context

Why this decision is needed.

## Options

1. Option A
2. Option B

## Decision

Selected option and reason.

## Relationships

- [[Decision Name]] --affects--> [[Related Process]]
- [[Decision Name]] --requires--> [[Related Rule]]

## Consequences

- Positive consequence
- Trade-off
```

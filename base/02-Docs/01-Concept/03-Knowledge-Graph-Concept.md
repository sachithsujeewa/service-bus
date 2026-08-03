---
type: documentation
category: concept
name: Knowledge Graph Concept
status: active
---

# Knowledge Graph Concept

A knowledge graph is a network of connected concepts.

In this vault, the graph is built using links and relationship annotations.

## Basic link

```markdown
[[Concept A]] is related to [[Concept B]]
```

This creates a simple connection.

## Typed relationship

```markdown
- [[Concept A]] --requires--> [[Concept B]]
```

This creates a meaningful relationship.

## Example

```text
Course --includes--> Topic
Topic --requires--> Prerequisite
Assessment --validates--> Learning Outcome
```

## Why typed relationships are useful

A simple link says:

```text
A is connected to B
```

A typed relationship says:

```text
A requires B
A uses B
A validates B
A explains B
```

This is easier for humans and AI agents to reason with.

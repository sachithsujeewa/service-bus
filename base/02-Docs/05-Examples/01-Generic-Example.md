---
type: example
name: Generic Knowledge Example
status: active
---

# Generic Knowledge Example

## Raw note

```text
Concept B should be understood before Concept A.
Concept A is used inside Process X.
Process X is validated by Review Y.
```

## Structured notes

```markdown
- [[Concept A]] --requires--> [[Concept B]]
- [[Process X]] --uses--> [[Concept A]]
- [[Review Y]] --validates--> [[Process X]]
```

## Graph

```text
Concept B → Concept A → Process X → Review Y
```

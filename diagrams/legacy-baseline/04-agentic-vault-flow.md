# Agentic Vault Processing Flow

**Source:** `base/` generic framework

```text
00-Inbox (raw catalog)
        │
        ▼
Skill Orchestration Skill
        │
        ├── Intake Skill
        ├── Classification Skill
        ├── Retrieval Skill
        ├── Update Planning Skill
        ├── Markdown Generation Skill
        ├── Relationship Skill
        ├── Tagging Skill
        ├── Validation Skill
        └── Summary Skill
        │
        ▼
01-Processing/ (execution trace)
        │
        ▼
10-Knowledge/ (structured notes)
        │
        ▼
30-Maps/ + 99-Graph/
```

## Human approval gates

```text
Level 0: Auto-apply
Level 1: Review recommended
Level 2: Human approval required
Level 3: Expert approval required
```

## Related

- `base/02-Docs/03-Agentic-Flow/01-Agentic-Knowledge-Flow.md`

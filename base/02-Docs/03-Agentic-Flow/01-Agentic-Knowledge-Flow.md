---
type: documentation
category: agentic-flow
name: Agentic Knowledge Flow
status: active
---

# Agentic Knowledge Flow

This vault can be maintained using an AI agent.

The agent should not randomly write notes.  
It should follow a controlled skill-based flow.

## Flow

```text
1. Human adds raw knowledge to 00-Inbox
2. Skill Orchestration Skill controls the processing flow
3. Intake Skill extracts clean facts
4. Classification Skill identifies knowledge type
5. Retrieval Skill checks existing notes
6. Planning Skill decides create/update actions
7. Markdown Skill writes or patches notes
8. Relationship Skill adds typed links
9. Tagging Skill adds useful filtering tags
10. Validation Skill checks quality
11. Summary Skill explains changes
12. Human approves important changes
```

## Why skill-based?

Skill-based flow is safer than one large free-form agent.

It gives:

- Clear responsibility
- Repeatability
- Traceability
- Validation
- Human approval
- Better governance

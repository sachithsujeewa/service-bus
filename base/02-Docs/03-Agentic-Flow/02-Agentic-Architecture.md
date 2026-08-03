---
type: documentation
category: agentic-flow
name: Agentic Architecture
status: active
---

# Agentic Architecture

```text
+------------------+
| Human / Source   |
+--------+---------+
         |
         v
+------------------+
| 00-Inbox         |
+--------+---------+
         |
         v
+---------------------------+
| Knowledge Orchestrator    |
+--------+------------------+
         |
         v
+--------------------------------------------------+
| Skill Pipeline                                    |
| Intake → Classify → Retrieve → Plan → Generate    |
| Link → Validate → Summarize → Review              |
+--------+-----------------------------------------+
         |
         v
+---------------------------+
| Structured Vault          |
+--------+------------------+
         |
         v
+---------------------------+
| Knowledge Graph           |
+--------+------------------+
         |
         v
+---------------------------+
| Human + Agent Understanding|
+---------------------------+
```

## Main components

- `00-Inbox`: raw input
- `04-Agent-Skills`: skill specifications
- `05-Agent-Prompts`: operating prompts
- `10-Knowledge`: structured knowledge
- `20-Relationships`: relationship vocabulary
- `30-Maps`: overview maps
- `99-Graph`: graph outputs

---
type: raw-note
status: processed
source: manual
created: 2026-05-30
processed_to: 10-Knowledge/Processes/inbox-processing-workflow.md
---

# How Inbox Processing Works

Copying a note into `00-Inbox` is the first step, but it does not automatically convert the note into structured knowledge.

The vault is agent-ready, not fully automatic by itself.

## Process

1. Add a rough note to `00-Inbox`.
2. Ask an AI agent to process the inbox note.
3. The agent reads the raw note.
4. The agent identifies concepts, processes, rules, decisions, examples, and questions.
5. The agent creates or updates structured notes in `10-Knowledge`.
6. The agent adds typed relationships between notes.
7. The agent summarizes what changed.
8. A human reviews important changes when needed.

## Example Request

```text
Process the inbox note `how-inbox-processing-works.md`
```

## Key Idea

The inbox is a capture area.

Structured knowledge should live in `10-Knowledge`, with links, metadata, and relationships.

---
type: process
name: Inbox Processing Workflow
domain: General
status: active
source: 00-Inbox/how-inbox-processing-works.md
approval_required: false
created: 2026-05-30
---

# Inbox Processing Workflow

## Purpose

The inbox processing workflow explains how raw notes move from `00-Inbox` into structured knowledge inside the vault.

Copying a note into `00-Inbox` captures the idea, but it does not automatically create structured knowledge. A human or AI agent must process the inbox note.

## Steps

1. Add a rough note to `00-Inbox`.
2. Ask an AI agent to process the inbox note.
3. Run the Skill Orchestration Skill.
4. Read the raw note.
5. Identify concepts, processes, rules, decisions, examples, and questions.
6. Create or update structured notes in `10-Knowledge`.
7. Add typed relationships between notes.
8. Add useful filtering tags to YAML frontmatter.
9. Validate metadata, relationships, tags, and review requirements.
10. Summarize what changed.
11. Review important changes when needed.

## Relationships

- [[Inbox Processing Workflow]] --uses--> [[00-Inbox]]
- [[Inbox Processing Workflow]] --produces--> [[10-Knowledge]]
- [[Inbox Processing Workflow]] --uses--> [[04-Agent-Skills/12-skill-orchestration-skill]]
- [[Inbox Processing Workflow]] --uses--> [[03-Templates]]
- [[Inbox Processing Workflow]] --uses--> [[04-Agent-Skills]]
- [[Inbox Processing Workflow]] --uses--> [[04-Agent-Skills/11-tagging-skill]]
- [[Inbox Processing Workflow]] --uses--> [[20-Relationships/01-relationship-vocabulary]]
- [[02-Docs/03-Agentic-Flow/01-Agentic-Knowledge-Flow]] --explains--> [[Inbox Processing Workflow]]

## Inputs

- Raw notes
- Meeting notes
- Lecture notes
- Research notes
- Ideas
- Questions

## Outputs

- Structured concept notes
- Structured process notes
- Structured rule notes
- Structured decision notes
- Example notes
- Typed relationships
- YAML tags
- Change summaries

## Risks

- Raw notes may stay unprocessed if no human or agent reviews them.
- The agent may create weak relationships if the source note is unclear.
- Important rules or decisions may need human review before becoming trusted knowledge.

## Example Request

```text
Process the inbox note `how-inbox-processing-works.md`
```

---
type: agent-skill
name: Skill Orchestration Skill
status: active
---

# Skill Orchestration Skill

## Purpose

Controls the full skill-processing flow for turning raw inbox notes into structured vault knowledge.

This skill decides which skills should run, in what order, what each skill should receive, and what output artifacts should be produced.

Use this skill when the user asks to:

- Process an inbox note
- Run the skill workflow
- Show skill execution output
- Convert raw knowledge into structured notes
- Explain what artifacts were generated

## Input Contract

```json
{
  "request": "user request",
  "source_note": "path to inbox note",
  "mode": "process | reprocess | inspect | dry-run",
  "output_trace": true
}
```

## Output Contract

```json
{
  "status": "passed | needs_review | failed",
  "result": {
    "source_note": "",
    "generated_notes": [],
    "updated_notes": [],
    "relationships": [],
    "tags": [],
    "execution_trace": "",
    "summary": ""
  },
  "issues": [],
  "approval_required": false
}
```

## Skill Order

Run skills in this order:

```text
1. Intake Skill
2. Classification Skill
3. Retrieval Skill
4. Update Planning Skill
5. Markdown Generation Skill
6. Relationship Skill
7. Tagging Skill
8. Validation Skill
9. Summary Skill
```

Optional skills:

```text
10. Knowledge Graph Exploration Skill
```

Use the optional graph exploration skill when the user asks to view relationships, inspect the graph, find missing nodes, or visualize a graph neighborhood.

## Procedure

1. Read the user request.
2. Identify the source note.
3. Check whether the source note exists.
4. Check whether the source note is already processed.
5. If already processed, avoid duplicate output unless the user explicitly asks to reprocess.
6. Run Intake Skill to extract facts.
7. Run Classification Skill to choose the note type.
8. Run Retrieval Skill to find related or duplicate notes.
9. Run Update Planning Skill to decide create/update actions.
10. Run Markdown Generation Skill to create or update structured notes.
11. Run Relationship Skill to add typed relationships.
12. Run Tagging Skill to suggest or apply YAML tags.
13. Run Validation Skill to check metadata, relationships, tags, and review requirements.
14. Run Summary Skill to explain what changed.
15. If `output_trace` is true, create a skill execution output note in `01-Processing`.

## Output Artifacts

Possible artifacts:

- Updated inbox note
- Structured concept note
- Structured process note
- Structured rule note
- Structured decision note
- Example note
- Skill execution output note
- Relationship list
- Tag list
- Review warning
- Summary

## Reprocessing Rules

If the source note already has:

```yaml
status: processed
processed_to: path/to/structured-note.md
```

Then:

1. Do not create a duplicate structured note.
2. Read the existing target note.
3. Update the existing target only if new information is present.
4. Produce a new execution trace if requested.
5. Explain that the note was already processed.

## Validation Rules

- Do not create duplicate notes for the same source.
- Preserve source references.
- Use templates from `03-Templates`.
- Use approved relationships from [[20-Relationships/01-relationship-vocabulary]] where possible.
- Run Tagging Skill before Validation Skill.
- Mark inbox notes as processed only after structured output exists.
- Create review warnings for rules, decisions, policies, or uncertain knowledge.
- Do not update `99-Graph` unless explicitly requested.

## Example

Input:

```text
Process the inbox note `obsidian-vault-architecture.md`
```

Skill flow:

```text
Skill Orchestration
  -> Intake
  -> Classification
  -> Retrieval
  -> Update Planning
  -> Markdown Generation
  -> Relationship
  -> Tagging
  -> Validation
  -> Summary
```

Expected artifacts:

```text
00-Inbox/obsidian-vault-architecture.md
10-Knowledge/Concepts/obsidian-vault-architecture.md
01-Processing/obsidian-vault-architecture-skill-execution.md
```

## Related Skills

- [[04-Agent-Skills/01-intake-skill]]
- [[04-Agent-Skills/02-classification-skill]]
- [[04-Agent-Skills/03-retrieval-skill]]
- [[04-Agent-Skills/04-update-planning-skill]]
- [[04-Agent-Skills/05-markdown-generation-skill]]
- [[04-Agent-Skills/06-relationship-skill]]
- [[04-Agent-Skills/11-tagging-skill]]
- [[04-Agent-Skills/07-validation-skill]]
- [[04-Agent-Skills/08-summary-skill]]
- [[04-Agent-Skills/10-knowledge-graph-exploration-skill]]


---
type: agent-skill
name: Knowledge Graph Exploration Skill
status: active
---

# Knowledge Graph Exploration Skill

## Purpose

Explores the vault's knowledge graph by reading notes, extracting typed relationships, finding connected concepts, and summarizing graph structure for a human or agent.

Use this skill when the user asks questions such as:

- What is connected to this concept?
- Show the graph around this note.
- Find missing or weak relationships.
- List all existing relationships.
- Explain how this knowledge area is structured.
- Find isolated notes.

## Input Contract

```json
{
  "focus_note": "optional note path or note name",
  "scope": "folder, whole vault, or selected notes",
  "depth": "optional relationship depth",
  "output_format": "summary | relationship-list | ascii-graph | review-report"
}
```

## Output Contract

```json
{
  "status": "passed | needs_review | failed",
  "result": {
    "nodes": [],
    "relationships": [],
    "isolated_notes": [],
    "missing_targets": [],
    "observations": [],
    "recommended_updates": []
  },
  "issues": [],
  "approval_required": false
}
```

## Procedure

1. Identify the requested scope.
2. Search notes for typed relationship lines using this pattern:

   ```markdown
   [[Source]] --relation--> [[Target]]
   ```

3. Extract nodes, relationships, source files, and line references.
4. If a focus note is provided, filter relationships to the requested graph neighborhood.
5. Group relationships by source node and relationship type.
6. Detect missing or unresolved target notes.
7. Detect isolated notes that have no inbound or outbound relationships.
8. Detect weak relationships such as `related_to` when a more specific relationship may fit.
9. Produce the requested output format.
10. Recommend updates, but do not modify graph files unless explicitly asked.

## Relationship Sources

Prefer active knowledge notes first:

- `10-Knowledge`
- `20-Relationships`
- `30-Maps`
- `99-Graph`

Use these as supporting context only:

- `02-Docs`
- `03-Templates`
- `04-Agent-Skills`
- `01-Processing`

## Output Formats

### Relationship List

Use this format:

```text
Source --relation--> Target
```

### ASCII Graph

Use this format for simple graph visualization:

```text
+----------+     relation     +----------+
| Source   | ---------------->| Target   |
+----------+                  +----------+
```

### Review Report

Include:

- Strong relationships
- Weak relationships
- Missing target notes
- Isolated notes
- Suggested new notes
- Suggested relationship improvements

## Validation Rules

- Use approved relationship vocabulary from [[20-Relationships/01-relationship-vocabulary]] where possible.
- Do not treat template placeholder relationships as active graph facts.
- Do not treat documentation examples as active graph facts unless the user asks for all examples too.
- Keep source file references for extracted relationships.
- Flag unresolved wiki links.
- Flag unsupported relationship names.
- Do not update `99-Graph` without explicit user approval.

## Example

Input:

```json
{
  "focus_note": "Obsidian Vault Architecture",
  "scope": "10-Knowledge",
  "depth": 1,
  "output_format": "ascii-graph"
}
```

Output:

```text
Obsidian Vault Architecture --includes--> Vault Folder
Obsidian Vault Architecture --includes--> Markdown Notes
Obsidian Vault Architecture --includes--> YAML Frontmatter
Wiki Links --supports--> Knowledge Graph
Graph View --uses--> Wiki Links
```

## Related Documentation

- [[02-Docs/01-Concept/03-Knowledge-Graph-Concept]]
- [[20-Relationships/01-relationship-vocabulary]]
- [[20-Relationships/02-linking-guidelines]]
- [[99-Graph/graph]]


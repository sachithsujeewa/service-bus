---
type: raw-note
status: processed
source: manual
created: 2026-05-30
processed_to: 10-Knowledge/Concepts/obsidian-vault-architecture.md
---

# Obsidian Vault Architecture

An Obsidian vault is a folder-based knowledge system made from plain Markdown files.

The basic architecture includes folders, notes, links, metadata, and optional graph views.

## Main Parts

1. Vault folder
2. Markdown notes
3. Folder structure
4. YAML frontmatter
5. Wiki links
6. Tags
7. Attachments
8. Graph view

## How It Works

The vault folder stores all notes and supporting files.

Each note is a Markdown file.

YAML frontmatter stores metadata such as type, status, domain, source, and created date.

Wiki links connect notes using the `[[Note Name]]` format.

Typed relationships can make links more meaningful, for example:

```markdown
- [[Concept Note]] --belongs_to--> [[Vault Folder]]
- [[Wiki Link]] --connects--> [[Markdown Notes]]
- [[Graph View]] --visualizes--> [[Relationships]]
```

The graph view is generated from links between notes.

## Key Idea

Obsidian is useful because the knowledge is stored as plain files, not inside a closed database.

This makes the vault readable by Obsidian, code editors, scripts, Git, and AI agents.

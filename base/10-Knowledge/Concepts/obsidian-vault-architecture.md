---
type: concept
name: Obsidian Vault Architecture
domain: Knowledge Management
status: active
source: 00-Inbox/obsidian-vault-architecture.md
approval_required: false
created: 2026-05-30
tags:
  - obsidian
  - knowledge-management
  - architecture
  - markdown
---

# Obsidian Vault Architecture

## Definition

Obsidian vault architecture is the file-based structure used to store and connect knowledge in an Obsidian-compatible vault.

An Obsidian vault is a folder-based knowledge system made from plain Markdown files, folders, links, metadata, and optional graph views.

## Why it matters

This architecture matters because the knowledge is stored as plain files instead of being locked inside a closed database.

That makes the vault readable by Obsidian, code editors, scripts, Git, and AI agents.

## Main Parts

- Vault folder
- Markdown notes
- Folder structure
- YAML frontmatter
- Wiki links
- Tags
- Attachments
- Graph view

## Relationships

- [[Obsidian Vault Architecture]] --includes--> [[Vault Folder]]
- [[Obsidian Vault Architecture]] --includes--> [[Markdown Notes]]
- [[Obsidian Vault Architecture]] --includes--> [[Folder Structure]]
- [[Obsidian Vault Architecture]] --includes--> [[YAML Frontmatter]]
- [[Obsidian Vault Architecture]] --includes--> [[Wiki Links]]
- [[Obsidian Vault Architecture]] --includes--> [[Tags]]
- [[Obsidian Vault Architecture]] --includes--> [[Attachments]]
- [[Obsidian Vault Architecture]] --includes--> [[Graph View]]
- [[Wiki Links]] --supports--> [[Knowledge Graph]]
- [[Graph View]] --uses--> [[Wiki Links]]
- [[YAML Frontmatter]] --supports--> [[Metadata]]

## Examples

```markdown
- [[Obsidian Vault Architecture]] --includes--> [[Markdown Notes]]
- [[Wiki Links]] --supports--> [[Knowledge Graph]]
- [[Graph View]] --uses--> [[Wiki Links]]
```

## Common Confusions

- Copying Markdown files into a vault does not automatically create structured knowledge.
- Obsidian is one possible viewer for the vault, but the vault itself is made of plain files.
- A graph view depends on links and relationships between notes.

## Agent Instructions

Before updating this concept, check:

- [[02-Docs/01-Concept/02-Obsidian-Compatible-Structure]]
- [[20-Relationships/01-relationship-vocabulary]]

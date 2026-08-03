# Agent guide — Service Bus workspace

Instructions for Cursor agents (and new chat sessions) working in this repo.

## Start here

| Path | Purpose |
|------|---------|
| [diagrams/chat-history/INDEX.md](diagrams/chat-history/INDEX.md) | Timeline of sessions |
| [diagrams/chat-history/USER-REQUEST-LOG.md](diagrams/chat-history/USER-REQUEST-LOG.md) | **All user questions** (U-001…) |
| [diagrams/chat-history/SESSION-CHECKLIST.md](diagrams/chat-history/SESSION-CHECKLIST.md) | End-of-session logging checklist |
| [diagrams/README.md](diagrams/README.md) | Architecture / MVP diagrams |

## Repo layout

| Path | Role |
|------|------|
| `base/` | Generic agentic Obsidian framework (skills, prompts) |
| `_intake/` | Read-only historical exports — **do not bulk-copy** |
| `vault/` | Obsidian knowledge graph (`10-Knowledge` legacy, `11-New-System` modernization) |
| `diagrams/` | Canonical ASCII diagrams + **chat history** |
| `prototype/` | Runnable MVP (Docker, .NET 8, RabbitMQ) |

## Mandatory: chat history logging

After meaningful work, **update logs before finishing**:

1. Add each user request to `diagrams/chat-history/USER-REQUEST-LOG.md` (increment `U-0XX`).
2. Create or update `diagrams/chat-history/YYYY-MM-DD-topic.md`.
3. Update `diagrams/chat-history/INDEX.md`.

Cursor rule: `.cursor/rules/chat-history-logging.mdc` (`alwaysApply: true`).

## MVP demo (quick)

```bash
cd prototype && docker compose up -d --build
```

- Control panel: http://localhost:8080/
- Source: http://localhost:5101/
- Partner: http://localhost:5102/
- Guide: [prototype/docs/TESTING-GUIDE.md](prototype/docs/TESTING-GUIDE.md)

## Constraints

- Keep `vault/10-Knowledge/` and `vault/11-New-System/` isolated; cross-link with typed relations only.
- Diagrams live in `diagrams/` — vault notes link, do not duplicate large ASCII blocks.

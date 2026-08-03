# Chat History

Curated summaries of Cursor agent sessions for the Service Bus workspace.

| Start here | Purpose |
|------------|---------|
| [INDEX.md](INDEX.md) | Timeline — what happened when |
| [USER-REQUEST-LOG.md](USER-REQUEST-LOG.md) | **What you asked** — all questions by ID (U-001…) |
| Session `YYYY-MM-DD-*.md` files | Agent work, decisions, artifacts, fixes |

## Why this folder exists

- Preserve **what you asked** and **what we built**
- Onboard others without re-reading full chat threads
- Track Docker/WSL, MVP demo, vault structure

## Session index

| Date | File | Summary |
|------|------|---------|
| — | [INDEX.md](INDEX.md) | Master timeline |
| — | [USER-REQUEST-LOG.md](USER-REQUEST-LOG.md) | **Your questions** (U-001 … U-024) |
| — | [SESSION-CHECKLIST.md](SESSION-CHECKLIST.md) | **Agent checklist** (end of session) |
| — | [_TEMPLATE-session.md](_TEMPLATE-session.md) | Copy for new session files |
| 2026-07-30 | [2026-07-30-service-bus-vault-and-mvp.md](2026-07-30-service-bus-vault-and-mvp.md) | Vault, MVP scaffold, diagrams |
| 2026-08-03 | [2026-08-03-mvp-demo-docker-and-uis.md](2026-08-03-mvp-demo-docker-and-uis.md) | Docker demo, UIs, fixes |

## How to maintain

After each meaningful session — **see [SESSION-CHECKLIST.md](SESSION-CHECKLIST.md)**:

1. **User requests** — add rows to [USER-REQUEST-LOG.md](USER-REQUEST-LOG.md) (new `U-0XX` IDs).
2. **Session file** — create from [_TEMPLATE-session.md](_TEMPLATE-session.md) or append to today's file.
3. **INDEX.md** — add timeline row.
4. Optional: export Cursor `.jsonl` to this folder.

**Automated for Cursor:** rule `.cursor/rules/chat-history-logging.mdc` (`alwaysApply: true`).  
**Onboarding:** repo root [AGENTS.md](../../AGENTS.md).

## Session file template

```markdown
# Session: YYYY-MM-DD — Topic

Full user log: [USER-REQUEST-LOG.md](USER-REQUEST-LOG.md) (U-0XX …).

## User requests (this session)

| ID | Category | You asked | Outcome |
|----|----------|-----------|---------|
| U-0XX | demo | … | … |

## Goal
…

## Decisions
…

## Artifacts created / updated
…

## Open items
- [ ] …
```

### Request categories

`discovery` · `architecture` · `implementation` · `demo` · `troubleshooting` · `documentation`

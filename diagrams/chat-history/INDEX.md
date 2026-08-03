# Chat history — master index

Curated session logs for the Service Bus workspace.

| Document | Use when you want… |
|----------|-------------------|
| **[USER-REQUEST-LOG.md](USER-REQUEST-LOG.md)** | **What you asked** (U-001 …) — questions, intent, outcomes |
| Session files below | What the agent built, fixed, decided |
| [README.md](README.md) | How to maintain logs |

## Timeline

| Date | Session | Focus |
|------|---------|--------|
| 2026-07-30 | [service-bus-vault-and-mvp](2026-07-30-service-bus-vault-and-mvp.md) | Vault bootstrap, `11-New-System/`, MVP scaffold, `diagrams/` library, WSL blocked |
| 2026-08-03 | [mvp-demo-docker-and-uis](2026-08-03-mvp-demo-docker-and-uis.md) | WSL fixed, end-to-end Docker demo, browser UIs, webhook dedupe, build fixes |

## Quick “what happened when”

### 2026-07-30 — Foundation

- Built Obsidian vault (`10-Knowledge` legacy + `11-New-System` isolated chapter)
- Scanned `_intake/new system/` for modernization discovery
- Scaffolded `prototype/` (.NET 8, RabbitMQ, Postgres, API keys)
- Created `diagrams/` with legacy, new-system, and MVP ASCII diagrams
- Docker blocked on user machine (WSL not installed)

### 2026-08-03 — Runnable demo

- User installed WSL; Docker stack verified
- Fixed builds (`ServiceBusMvp.sln`, compile errors, Partner Dockerfile)
- Added demo UIs: http://localhost:8080/, :5101/, :5102/
- Added `prototype/docs/TESTING-GUIDE.md` and improved `demo.sh`
- Fixed duplicate webhook registrations → 4× partner inbox rows
- Restored `diagrams/mvp/00-proposed-mvp-architecture.md`

## Where to start (newcomer)

1. [USER-REQUEST-LOG.md](USER-REQUEST-LOG.md) — your questions and what was delivered
2. This INDEX — timeline by date
3. `diagrams/README.md` — diagram catalog
4. `prototype/docs/TESTING-GUIDE.md` — run the demo

## Maintaining history

See [README.md](README.md). After each session:

1. Add rows to **USER-REQUEST-LOG.md** (new `U-0XX` IDs)
2. Add **User requests** table to the session file
3. Update this INDEX timeline

## Raw transcripts

Cursor JSONL exports (optional) can be stored here as `YYYY-MM-DD-topic.jsonl`. Curated `.md` files are the primary onboarding path.

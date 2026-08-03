# Session checklist (agents)

Copy this checklist at the **end of each meaningful Cursor session**. Makes work observable for the next chat.

## Pre-flight (new chat / unclear context)

- [ ] Read [INDEX.md](INDEX.md) — what happened before
- [ ] Read [USER-REQUEST-LOG.md](USER-REQUEST-LOG.md) — last `U-0XX` ID
- [ ] Skim latest `YYYY-MM-DD-*.md` session file
- [ ] Repo guide: [../../AGENTS.md](../../AGENTS.md)

## During session

- [ ] Note each **user request** (one line + category) for the log
- [ ] Note **files changed** and **why**

## End of session (mandatory if non-trivial work)

- [ ] **USER-REQUEST-LOG.md** — new rows `U-0XX` (category, question, outcome)
- [ ] **Session file** — create from [_TEMPLATE-session.md](_TEMPLATE-session.md) or append to today's file
- [ ] **INDEX.md** — new timeline row if new session topic/day
- [ ] **diagrams/README.md** — chat-history table row if new session file
- [ ] Mark completed items in prior session **Open items** when applicable

## Categories for user requests

`discovery` · `architecture` · `implementation` · `demo` · `troubleshooting` · `documentation`

## Optional

- [ ] Export Cursor transcript `.jsonl` to this folder (`YYYY-MM-DD-topic.jsonl`)
- [ ] New diagram → `diagrams/<folder>/` + row in `diagrams/README.md`

## Quick links

| Demo | URL |
|------|-----|
| Service Bus API | http://localhost:8080/ |
| Mock RamBase | http://localhost:5101/ |
| Partner inbox | http://localhost:5102/ |

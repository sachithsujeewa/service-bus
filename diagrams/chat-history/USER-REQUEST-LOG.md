# User request log

Structured log of **what you asked** in Cursor sessions — intent, outcome, and where to find results.

For agent decisions and technical fixes, see session files in [INDEX.md](INDEX.md).

---

## How to read this log

| Column | Meaning |
|--------|---------|
| **#** | Request number (global across project) |
| **When** | Date (session) |
| **Category** | Type of request |
| **You asked** | Your question / instruction (summarized) |
| **Outcome** | What was delivered |

**Categories:** `discovery` · `architecture` · `implementation` · `demo` · `troubleshooting` · `documentation`

---

## 2026-07-30 — Vault, new system, MVP scaffold

| # | Category | You asked | Outcome |
|---|----------|-----------|---------|
| U-001 | discovery | Explain the `base/` agentic workflow using an ASCII diagram | Agentic flow diagram; vault bootstrap approach |
| U-002 | implementation | Bootstrap structured Obsidian from `_intake/` — deep content, **no valuable/raw copy** from intake | `vault/` knowledge graph; synthesized notes in `10-Knowledge/` |
| U-003 | architecture | Scan `_intake/new system/` with a new agent; discover new architecture concerns; **separate chapter** from legacy; goal = ASUC knowledge base | `vault/11-New-System/` isolated chapter; discovery agent specs |
| U-004 | architecture | Key components of the new system — ASCII diagram of component relationships | Component model notes + `diagrams/new-system/03-component-relationships.md` |
| U-005 | architecture | MVP sub-scope: RamBase event → partner webhook; mock allowed; **draw proposed MVP ASCII before implement** | Proposed MVP diagram (later `diagrams/mvp/00-proposed-mvp-architecture.md`) |
| U-006 | implementation | Discover ASUCs, tech stack, containerized deployment; source app, partner app, RabbitMQ, auth/authz | `vault/11-New-System/MVP/*`, `prototype/` scaffold |
| U-007 | troubleshooting | Docker does not start properly | Diagnosed WSL not installed; `DOCKER-TROUBLESHOOTING.md` |
| U-008 | troubleshooting | Fix the Docker issue | WSL install guidance; no-Docker path documented (partial) |
| U-009 | documentation | Dump all MVP + architecture diagrams to a separate folder; **maintain chat history** going forward | `diagrams/` library; `diagrams/chat-history/` started |
| U-010 | documentation | Proposed MVP architecture diagram seems missing | Added `diagrams/mvp/00-proposed-mvp-architecture.md` |

---

## 2026-08-03 — Docker demo, UIs, fixes

| # | Category | You asked | Outcome |
|---|----------|-----------|---------|
| U-011 | troubleshooting | Fixed WSL — please re-check | Docker/WSL verified; stack built; end-to-end demo run |
| U-012 | troubleshooting | `./scripts/demo.sh` stops after `== Health checks ==` | Explained silent `curl` fail; improved script; stack restart |
| U-013 | demo | Guide how to test the complete flow | Testing steps (later `prototype/docs/TESTING-GUIDE.md`) |
| U-014 | demo | Minimal UIs for demo interfaces + write into a guide | UIs at `:8080/`, `:5101/`, `:5102/`; `TESTING-GUIDE.md` |
| U-015 | demo | Give demo steps | Short live-demo script (3 tabs, 5 steps) |
| U-016 | troubleshooting | http://localhost:5102/ looks empty | Old Docker image (404 on `/`); rebuild with `--build` |
| U-017 | demo | Complete simple demo flow | Step-by-step demo flow document |
| U-018 | troubleshooting | Emit on `:5101/` doesn’t work; publish from `:8080/` works | Fixed source-app emit UI + server logic; SUCCESS/FAILED feedback |
| U-019 | demo | Do I need to redeploy to Docker after changes? | Yes — `docker compose up -d --build` after code edits |
| U-020 | troubleshooting | One event → 4 records on partner inbox `:5102` | Duplicate webhook registrations; idempotent register + delivery dedupe; `down -v` for clean DB |
| U-021 | troubleshooting | `docker compose up -d --build` fails on `partner-app` publish | `PartnerApp/Dockerfile` missing `ServiceBus.Contracts` |
| U-022 | documentation | Chat history not properly documented | `2026-08-03` session log, `INDEX.md`, updated README |
| U-023 | documentation | Log what I asked as well in a proper manner | This file + **User requests** sections in session logs |

---

## Quick lookup by topic

| Topic | Request IDs |
|-------|-------------|
| Vault / knowledge graph | U-002, U-003 |
| New system vs legacy | U-003, U-004 |
| MVP design & prototype | U-005, U-006 |
| Diagrams folder | U-009, U-010 |
| Docker / WSL | U-007, U-008, U-011, U-019, U-021 |
| Demo / testing | U-013, U-014, U-015, U-017 |
| UI issues | U-016, U-018 |
| Partner inbox duplicates | U-020 |
| Chat / documentation | U-009, U-022, U-023 |

---

## Adding your next request

1. Continue the table in a new **session section** (or append to this file after each session).
2. Add a row to the session file under **User requests** (see [README.md](README.md)).
3. Update [INDEX.md](INDEX.md) if it’s a new session day.

**Template row:**

```markdown
| U-0XX | category | Your question (one line) | What was delivered |
```

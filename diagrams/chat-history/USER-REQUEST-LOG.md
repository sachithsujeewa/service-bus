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
| U-023 | documentation | Log what I asked in a proper manner | [USER-REQUEST-LOG.md](USER-REQUEST-LOG.md) + session **User requests** tables |
| U-024 | documentation | Logging mechanism for all future chats (observable for new sessions) | `.cursor/rules/chat-history-logging.mdc`, `AGENTS.md`, `SESSION-CHECKLIST.md`, templates |

## 2026-08-11 — Webhook routing & retry fix

| # | Category | You asked | Outcome |
|---|----------|-----------|---------|
| U-025 | troubleshooting | Register new webhook `localhost:53174/PrintEventReceiver.ashx` — events flood partner app, not new URL | Diagnosed Docker `localhost` + infinite retry + all-or-nothing delivery; fixed `DispatcherWorker` + `InMemoryWebhookDispatcher`; `TESTING-GUIDE.md` host.docker.internal guidance |

## 2026-08-12 — Vault architecture overview

| # | Category | You asked | Outcome |
|---|----------|-----------|---------|
| U-026 | architecture | Explain architecture of this vault | Explained folder map, two chapters (10 vs 11), knowledge chain, agentic pipeline, workspace relationships |
| U-027 | troubleshooting | After registering `host.docker.internal:53174/PrintEventReceiver.ashx`, should VS breakpoint on ProcessRequest hit on publish? | Curl hits BP; bus gets HTTP 400 Invalid Hostname — IIS Express rejects `Host: host.docker.internal` |
| U-028 | implementation | Allow any port if request comes from localhost or host.docker.internal | Dispatcher rewrites `Host` for `host.docker.internal:any-port` → `localhost:port` (IIS Express) |
| U-029 | troubleshooting | Register `https://host.docker.internal:44394/` — no login BP; DLQ SSL errors | Dev HttpClient trusts local HTTPS certs for host.docker.internal; rebuild dispatcher |
| U-030 | troubleshooting | HTTP :53174 hits BP; HTTPS :44394 still does not | Root cause: 302 → `localhost` followed from Docker (`Connection refused`); disabled auto-redirect; need anonymous handler URL |

---

## 2026-08-14 — Visual architecture & RabbitMQ

| # | Category | You asked | Outcome |
|---|----------|-----------|---------|
| U-031 | architecture | Visualize new architecture; explain RabbitMQ middleman — less text, more AI visuals | `diagrams/mvp/visuals/*.png`, [[mvp-visual-architecture-guide]], `.cursor/rules/visual-first-explanations.mdc` |
| U-032 | architecture | Visual diagram of how things connect and RabbitMQ usage | `diagrams/mvp/visuals/rabbitmq-connections-mvp.png`; vault guide updated |
| U-033 | architecture | Entity model — archive relationships + link to logical architecture | `diagrams/new-system/visuals/entity-model-archives-to-architecture.png`, [[entity-model-archives-architecture]] |
| U-034 | architecture | Give me a visual diagram | `diagrams/mvp/visuals/service-bus-complete-visual.png` — one-page complete picture |
| U-035 | architecture | Archive structures diagram with field names and types | `diagrams/legacy-baseline/visuals/archive-structures-field-reference.png`, [[archive-structures-visual]] |
| U-036 | architecture | Generate a visual diagram | `diagrams/mvp/visuals/one-event-journey-flow.png` — ItmShipped end-to-end example |

## 2026-08-21 — Elevate solution writeup

| # | Category | You asked | Outcome |
|---|----------|-----------|---------|
| U-037 | documentation | Create `SOLUTION.md` from the Elevate 2026 template, elaborating each section | [SOLUTION.md](../../SOLUTION.md); diagrams in `diagrams/elevate/visuals/` |
| U-038 | documentation | Do not use a fixed skill count; use the presentation decks as source | [SOLUTION.md](../../SOLUTION.md) rewritten from `presentations/` language; slides embedded |
| U-039 | documentation | No relative links — submit as a sole document | Relative image/path links removed; Mermaid diagrams only |

---

---

## Quick lookup by topic

| Topic | Request IDs |
|-------|-------------|
| Vault / knowledge graph | U-002, U-003, U-026 |
| Elevate solution writeup | U-037, U-038, U-039 |
| New system vs legacy | U-003, U-004, U-026 |
| MVP design & prototype | U-005, U-006 |
| Diagrams folder | U-009, U-010 |
| Docker / WSL | U-007, U-008, U-011, U-019, U-021 |
| Demo / testing | U-013, U-014, U-015, U-017 |
| UI issues | U-016, U-018 |
| Partner inbox duplicates | U-020, U-025 |
| Chat / documentation | U-009, U-022, U-023, U-024 |

---

## Adding your next request

1. Continue the table in a new **session section** (or append to this file after each session).
2. Add a row to the session file under **User requests** (see [README.md](README.md)).
3. Update [INDEX.md](INDEX.md) if it’s a new session day.

**Template row:**

```markdown
| U-0XX | category | Your question (one line) | What was delivered |
```

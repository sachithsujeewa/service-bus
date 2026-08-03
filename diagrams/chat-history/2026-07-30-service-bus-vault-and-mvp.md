# Session: 2026-07-30 — Service Bus vault, new-system chapter, MVP prototype

Full user question log: [USER-REQUEST-LOG.md](USER-REQUEST-LOG.md) (U-001 through U-010).

## User requests (this session)

| ID | Category | You asked | Outcome |
|----|----------|-----------|---------|
| U-001 | discovery | Explain `base/` agentic workflow (ASCII) | Agentic flow diagram + vault approach |
| U-002 | implementation | Bootstrap Obsidian from `_intake/` — deep content, no raw copy | `vault/10-Knowledge/` synthesized graph |
| U-003 | architecture | Scan `_intake/new system/`; separate chapter; ASUC knowledge base | `vault/11-New-System/` + discovery agent |
| U-004 | architecture | New system key components — relationship diagram | Component notes + `diagrams/new-system/` |
| U-005 | architecture | MVP: event → partner webhook; ASCII **before** code | Proposed MVP architecture diagram |
| U-006 | implementation | ASUCs, tech, containerized local deploy; source/partner/RabbitMQ/auth | `prototype/` + MVP vault docs |
| U-007 | troubleshooting | Docker won’t start | WSL not installed — documented |
| U-008 | troubleshooting | Fix Docker issue | WSL install path + no-Docker doc (partial) |
| U-009 | documentation | Dump diagrams to folder; maintain chat history | `diagrams/` + `chat-history/` |
| U-010 | documentation | Proposed MVP diagram missing | `diagrams/mvp/00-proposed-mvp-architecture.md` |

## Goal

- Understand base framework agentic flow
- Bootstrap structured Obsidian vault from `_intake` (without copying raw export content)
- Isolate **legacy** vs **new system** architecture chapters
- Define and scaffold **MVP**: source app → RabbitMQ bus → partner webhook
- Centralize diagrams and maintain chat history

## Decisions

| Decision | Rationale |
|----------|-----------|
| `vault/10-Knowledge/` = legacy baseline chapter | Avoid contaminating historical synthesized model |
| `vault/11-New-System/` = modernization + discovery | As-implemented contract, migration, target, ASUC seeds |
| `prototype/` = runnable MVP | .NET 8, RabbitMQ, Postgres, API key auth |
| Diagrams in `diagrams/` at repo root | Single visual library, not scattered ASCII |
| Broker for MVP: RabbitMQ (charter candidate) | Containerized local test; broker TBD for production |
| Docker blocked: WSL not installed | Documented in `prototype/docs/DOCKER-TROUBLESHOOTING.md` — **resolved 2026-08-03** (see next session) |

## Artifacts created

| Path | Purpose |
|------|---------|
| `vault/` | Full Obsidian knowledge graph |
| `vault/11-New-System/` | New architecture chapter + MVP discovery docs |
| `prototype/` | Docker Compose + .NET 8 services |
| `diagrams/` | Consolidated architecture/MVP diagrams |
| `diagrams/chat-history/` | This session log |

## Key technical findings (new system discovery)

- As-implemented: **per-system topic** = `NGSystem.Name`, payload in **message properties**
- HA: SQL `CloudHostSync` heartbeat; ~60s failover gap
- Control DB (RambaseServiceBus SQL) SPOF for webhook delivery
- Broker selection **not decided** (NATS #1 in internal scorecard vs RabbitMQ in charter)

## Docker issue (user machine)

- Error: `Docker Desktop is unable to start` — **WSL not installed**
- Fix: Admin PowerShell `wsl --install` + reboot, then restart Docker Desktop
- Alternative: `prototype/docs/RUN-LOCAL-NO-DOCKER.md` (local Postgres + RabbitMQ + dotnet run)
- In-progress: dev mode without Docker (SQLite + in-memory bus) — partial scaffold

## Diagrams consolidated

All ASCII diagrams live under `diagrams/` (16 diagram files + this README index):

| Folder | Files |
|--------|-------|
| `legacy-baseline/` | 01–04 (pipeline, component map, knowledge chain, agentic flow) |
| `new-system/` | 01–06 (as-implemented, K8s target, components, planes, isolation, brokers) |
| `mvp/` | 01–06 (platform, components, happy path, auth, RabbitMQ, deployment modes) |

Vault notes should link here instead of duplicating large ASCII blocks.

## Open items

- [x] Install WSL2 and verify `docker compose up` in `prototype/` — done 2026-08-03
- [ ] Complete no-Docker dev mode (if Docker remains blocked)
- [ ] Expand ASUC seeds into full use case documents
- [ ] Broker go/no-go after M4 POC
- [ ] Field-level COF/WHA enrichment from `_intake` via agent (no bulk copy)

## Follow-up session

See [2026-08-03-mvp-demo-docker-and-uis.md](2026-08-03-mvp-demo-docker-and-uis.md) and [INDEX.md](INDEX.md).

## Reading order for newcomers

1. `diagrams/README.md`
2. `vault/README.md` → `11-New-System/README.md`
3. `prototype/README.md`
4. `diagrams/mvp/01-platform-overview.md`

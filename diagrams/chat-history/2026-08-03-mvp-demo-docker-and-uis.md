# Session: 2026-08-03 — MVP demo, Docker, UIs, duplicate webhook fix

Continuation of [2026-07-30-service-bus-vault-and-mvp.md](2026-07-30-service-bus-vault-and-mvp.md).

Full user question log: [USER-REQUEST-LOG.md](USER-REQUEST-LOG.md) (U-011 through U-023).

## User requests (this session)

| ID | Category | You asked | Outcome |
|----|----------|-----------|---------|
| U-011 | troubleshooting | Fixed WSL — re-check | Docker stack verified and running |
| U-012 | troubleshooting | `demo.sh` hangs at health checks | Script error hints; stack health explained |
| U-013 | demo | How to test complete flow | `prototype/docs/TESTING-GUIDE.md` |
| U-014 | demo | Minimal UIs + guide | Demo UIs on 8080/5101/5102 + guide |
| U-015 | demo | Demo steps | Live demo walkthrough (3 browser tabs) |
| U-016 | troubleshooting | `:5102/` looks empty | Rebuild images; UI route was missing in old container |
| U-017 | demo | Complete simple demo flow | Step-by-step demo script |
| U-018 | troubleshooting | Emit on `:5101/` fails; `:8080/` works | Source-app emit fix + clear SUCCESS/FAILED UI |
| U-019 | demo | Need to redeploy to Docker? | Yes — `docker compose up -d --build` after code changes |
| U-020 | troubleshooting | 1 event → 4 inbox rows | Duplicate webhooks; idempotent register + dedupe |
| U-021 | troubleshooting | `partner-app` Docker build error | Fixed Dockerfile (Contracts project) |
| U-022 | documentation | Chat history not documented | Session logs + INDEX |
| U-023 | documentation | Log what I asked properly | [USER-REQUEST-LOG.md](USER-REQUEST-LOG.md) |

## Goal

- Complete missing **proposed MVP architecture** diagram in `diagrams/`
- Verify **WSL / Docker** after user fixed Windows environment
- Make MVP **demoable** (guide, scripts, browser UIs)
- Fix runtime issues discovered during live demo (emit, empty pages, duplicate deliveries)

## User issues encountered (and resolution)

| Issue | Root cause | Fix |
|-------|------------|-----|
| `docker compose` build failed | Only `ServiceBusMvp.slnx` in repo; Dockerfiles expect `.sln` | Added `ServiceBusMvp.sln` |
| API/dispatcher build errors | Missing `using` in `IMessageBus.cs`, `IDataStore.cs` | Added contract usings + Sqlite package |
| Containers crash on start | RabbitMQ not ready when API connects | `ConnectWithRetryAsync`, compose `restart` |
| `demo.sh` stops at health checks | Postgres/RabbitMQ exited; `curl -sf` silent fail | Better errors in script; restart stack |
| http://localhost:5102/ blank | Old image without `GET /` UI route (404) | Rebuild with `--build` |
| Emit on 5101 “doesn’t work” | Weak UI feedback; empty `parameters` | Fixed JS + server `orderId`; SUCCESS/FAILED styling |
| 4 inbox rows for 1 event | 4 duplicate webhook registrations in Postgres | Idempotent register; dedupe delivery by URL |
| `partner-app` Docker build fail | Dockerfile missing `ServiceBus.Contracts` copy | Updated `PartnerApp/Dockerfile` |

## Decisions

| Decision | Rationale |
|----------|-----------|
| Demo UIs at `/` on 8080, 5101, 5102 | Clear live demo without curl |
| Idempotent webhook registration | Partner re-registers on every container restart |
| One HTTP POST per target URL per event | Duplicate DB rows should not multiply delivery |
| `docker compose up -d --build` after code changes | Containers run images, not workspace source |
| `docker compose down -v` for clean demo DB | Removes duplicate webhook rows |

## Artifacts created / updated

| Path | Change |
|------|--------|
| `diagrams/mvp/00-proposed-mvp-architecture.md` | Restored pre-implementation ASCII diagram |
| `diagrams/chat-history/2026-08-03-mvp-demo-docker-and-uis.md` | This session log |
| `diagrams/chat-history/INDEX.md` | Master timeline + reading order |
| `prototype/docs/TESTING-GUIDE.md` | Full demo walkthrough with UI URLs |
| `prototype/src/ServiceBus.Api/DemoUi.cs` | Control panel at `:8080/` |
| `prototype/src/SourceApp/DemoUi.cs` | Mock RamBase emitter at `:5101/` |
| `prototype/src/PartnerApp/DemoUi.cs` | Partner inbox at `:5102/` |
| `prototype/scripts/demo.sh` | Clear FAILED hints |
| `prototype/ServiceBusMvp.sln` | Classic solution for Docker builds |
| `prototype/src/PartnerApp/Dockerfile` | Include Contracts project |
| `prototype/src/ServiceBus.Messaging/RabbitMqTopology.cs` | Connection retry |
| `prototype/docker-compose.yml` | RabbitMQ healthcheck, restart policies |
| Idempotent webhook + dispatcher dedupe | `DatabaseService`, `PartnerApp`, `DispatcherWorker` |

## Demo flow (current)

```text
1. docker compose up -d --build
2. http://localhost:5102/  — partner inbox (auto-refresh)
3. http://localhost:8080/  — refresh webhooks list
4. http://localhost:5101/  — emit OrderCreated
5. Partner inbox shows 1 row per event (after clean DB)
```

CLI: `prototype/scripts/demo.sh`

## Docker / WSL status

- **Resolved:** WSL2 + Docker Desktop working (`docker version` shows Client + Server)
- Documented in `prototype/docs/DOCKER-TROUBLESHOOTING.md` (updated intro)

## Diagrams updated

| File | Note |
|------|------|
| `diagrams/mvp/00-proposed-mvp-architecture.md` | Added (was missing from initial dump) |

## Open items

- [ ] Optional: delete-duplicate-webhooks API or admin UI action (manual `down -v` works for demo)
- [ ] Complete no-Docker dev mode (`RUN-LOCAL-NO-DOCKER.md` — partial scaffold)
- [ ] Expand ASUC seeds; broker decision after M4 POC
- [ ] Field-level COF/WHA enrichment from `_intake` via agent

## Related

- [TESTING-GUIDE.md](../../prototype/docs/TESTING-GUIDE.md)
- [prototype/README.md](../../prototype/README.md)
- [00-proposed-mvp-architecture.md](../mvp/00-proposed-mvp-architecture.md)

# Session: 2026-08-12 — webhook VS breakpoint

Full user log: [USER-REQUEST-LOG.md](USER-REQUEST-LOG.md) (U-027).

## User requests (this session)

| ID | Category | You asked | Outcome |
|----|----------|-----------|---------|
| U-027 | troubleshooting | Register `host.docker.internal:53174/PrintEventReceiver.ashx`, publish — should VS breakpoint on ProcessRequest hit? | Curl hits BP; dispatcher gets **400 Invalid Hostname** (IIS Express Host header) |
| U-028 | implementation | Allow any port for localhost / host.docker.internal | `WebhookHttp.ApplyDevHostHeader` — any port on `host.docker.internal` gets `Host: localhost:port` |
| U-029 | troubleshooting | `https://host.docker.internal:44394/` → DLQ SSL, no login BP | `WebhookHttp.CreateClient` trusts IIS Express/VS certs for local hosts |
| U-030 | troubleshooting | HTTP ashx hits BP; HTTPS root does not | Redirect to `localhost` + login; `AllowAutoRedirect=false`; register anonymous path |

## Goal

Confirm expected debug behavior when bus delivers to a Visual Studio–hosted ashx; then make host webhooks work for any IIS Express port without binding edits; then HTTPS.

## Decisions

| Decision | Rationale |
|----------|-----------|
| Treat 400 as IIS Express binding issue | Docker curl reproduced `Bad Request - Invalid Hostname` for `Host: host.docker.internal` |
| Rewrite Host header in dispatcher (not IIS config) | Works for any port; no per-project `applicationhost.config` edits |
| Trust local HTTPS certs in dispatcher HttpClient | Container has no IIS Express / VS dev cert; DLQ showed SSL establish failure |
| Disable HttpClient auto-redirect | HTTPS site 302 → `https://localhost:…` is unreachable from Docker; login BP is on redirect target |

## Artifacts created / updated

| Path | Change |
|------|--------|
| `prototype/src/ServiceBus.Messaging/WebhookHttp.cs` | Host rewrite + local HTTPS trust + no auto-redirect |
| `prototype/src/ServiceBus.Dispatcher/DispatcherWorker.cs` | Clearer redirect errors |
| `prototype/docs/TESTING-GUIDE.md` | HTTPS / localhost redirect troubleshooting |

## Open items

- [ ] User registers anonymous HTTPS handler path (not site root / login)
- [ ] Prefer HTTP PrintEventReceiver URL that already works

## Related

- [2026-08-11-webhook-routing-retry.md](2026-08-11-webhook-routing-retry.md)

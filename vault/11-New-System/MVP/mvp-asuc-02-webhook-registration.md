---
type: use-case
id: ASUC-MVP-02
name: MVP Webhook Registration Auth
chapter: 11-New-System
scope: mvp
status: active
---

# ASUC-MVP-02: Webhook Registration with Authentication

## Statement

Only authenticated partners can register webhooks scoped to their `partnerId`; registrations drive live delivery without bus restart.

## Main flow

1. Partner App calls `POST /api/webhooks` with `X-Api-Key: partner` (or Bearer)
2. API validates key maps to `partnerId`
3. Row inserted in `webhooks` table
4. Management message published to `bus.management` queue
5. Dispatcher refreshes cache / binding on management event

## Authorization rules (MVP)

| Role | Can |
|------|-----|
| `partner` | CRUD own webhooks (`partnerId` from key) |
| `source` | POST events only |
| `admin` | All webhooks, DLQ inspect |

## Acceptance criteria

- [ ] 401 without API key
- [ ] 403 if partner tries another `partnerId`
- [ ] Delivery starts without dispatcher restart

## Relationships

- [[mvp-asuc-02-webhook-registration]] --requires--> [[mvp-technology-stack]]

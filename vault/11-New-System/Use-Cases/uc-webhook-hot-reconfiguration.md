---
type: use-case-seed
id: ASUC-03
name: Webhook Hot Reconfiguration
status: seed
chapter: 11-New-System
---

# ASUC-03: Webhook Hot Reconfiguration

## Intent

Partner adds or changes webhook (URL, event types, filters) **without** restarting bus service; delivery reflects new rules within bounded time.

## Trigger

`WebHookCreated` / `WebHookUpdated` / `WebHookDeleted` management event.

## Main flow

1. RamBase updates WHA
2. Management event on management topic
3. Management subscriber routes to `RambaseSystemClient`
4. Subscription rules / bindings updated live
5. New events match new rules; old rules stop matching

## Failure modes to specify

- Management event lost → WHA vs broker drift ([[CONC-013]])
- Overlap window during update → duplicate or missed types
- ParameterFilter change → injection or mis-filter

## Acceptance criteria (draft)

- Rule change visible within X seconds
- Reconciliation job detects drift within Y minutes
- No service restart required

## Relationships

- [[uc-webhook-hot-reconfiguration]] --preserves_contract_from--> [[preserve-vs-replace-contract]]
- [[uc-webhook-hot-reconfiguration]] --explained_by--> [[as-implemented-contract-summary]]

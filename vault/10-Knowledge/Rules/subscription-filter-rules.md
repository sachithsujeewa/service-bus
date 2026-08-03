---
type: rule
id: RULE-001
name: Subscription Filter Rules
status: active
approval_required: true
---

# Subscription Filter Rules

## RULE-001: EventType property required

Every message published to MainTopic for webhook delivery **must** include a user property `EventType` matching the registered VET type name exactly (case-sensitive unless broker configured otherwise).

## RULE-002: Filter derivation

Webhook registration for event type `T` **must** produce filter expression `EventType = "T"` on the subscription grouping that URL.

## RULE-003: URL grouping

Webhooks sharing the same target URL **must** share one subscription; event type discrimination happens via filter union and handler logic.

## RULE-004: Manager isolation

Meta event types (`WebHookCreated`, `WebHookUpdated`, `WebHookDeleted`) **must** be consumed by the manager subscription—not mixed into business subscriber handlers without explicit branch.

## RULE-005: No silent filter broadening

Operators **must not** add catch-all filters (e.g. `1=1`) on production subscriptions—causes cross-tenant data leakage risk if systems share broker objects incorrectly.

## Violation impact

Filter violations cause **silent non-delivery** or **over-delivery**—both are high-severity integration failures.

## Relationships

- [[subscription-filter-rules]] --validates--> [[routing-and-filter-design]]
- [[subscription-filter-rules]] --requires--> [[event-subscription-concept]]
- [[subscription-filter-rules]] --supports--> [[subscription-grouping-by-target-url]]

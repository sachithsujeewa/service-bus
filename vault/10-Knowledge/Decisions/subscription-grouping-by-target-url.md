---
type: decision
id: DEC-002
name: Subscription Grouping by Target URL
status: active
approval_required: true
---

# Decision: Group Webhooks by Target URL per Subscription

## Context

Each webhook registration could theoretically create a dedicated broker subscription. At scale, subscription count and HTTP client pools grow unwieldy.

## Decision

Webhooks targeting the **same remote URL** for a system share **one subscription**. Event types merge via filter union; subscriber handler dispatches by `EventType`.

## Rationale

- One HTTP connection pipeline per endpoint
- Unified retry state per partner URL
- Aligns with partner mental model (one endpoint, many event types)

## Trade-offs

| Benefit | Cost |
|---------|------|
| Fewer subscriptions | Handler must multiplex types |
| Per-URL retry | Filter union complexity on updates |
| Simpler partner scaling | Ordering only per URL, not per webhook row |

## Mitigations

- Manager updates filters atomically on webhook changes
- Subscriber logs EventType on every delivery
- Partners implement per-type handlers behind one URL

## Relationships

- [[subscription-grouping-by-target-url]] --affects--> [[event-subscription-concept]]
- [[subscription-grouping-by-target-url]] --requires--> [[subscription-filter-rules]]
- [[subscription-grouping-by-target-url]] --implemented_by--> [[subscriber-and-manager-design]]

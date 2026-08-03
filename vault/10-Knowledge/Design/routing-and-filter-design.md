---
type: design
id: DES-003
name: Routing and Filter Design
status: active
---

# Routing and Filter Design

## Filter derivation rule

For a webhook registered with event type `T`, the platform derives a subscription filter equivalent to:

```text
EventType = "T"
```

Multiple webhooks on the same URL with different event types produce a **union of filters** on one subscription. The subscriber handler must still dispatch based on `EventType` in the message.

## Why filters matter

Without filters, every subscription would receive every message on MainTopic—unscalable and insecure. Filters push selection into the broker so only relevant messages lock subscription cursors.

## Manager subscription filters

The manager subscription uses a distinct filter set capturing meta event types only—preventing business traffic from invoking topology logic incorrectly.

## Filter update dynamics

When `WebHookUpdated` changes event type interest:

1. Manager receives meta event
2. Old filter rule removed or superseded
3. New filter rule added
4. In-flight messages may use old rules briefly—subscribers tolerate overlap

## Misconfiguration symptoms

| Symptom | Likely cause |
|---------|--------------|
| No deliveries | Filter typo; EventType property missing on publish |
| Wrong events received | Over-broad filter; union too permissive |
| Duplicate deliveries | Multiple subscriptions same URL misconfigured |

## Design constraints

- Filter expressions are broker-limited in size and complexity—prefer one rule per event type pattern.
- Property names must match publisher mapping exactly (`EventType` casing).

## Relationships

- [[routing-and-filter-design]] --filtered_by--> [[event-subscription-concept]]
- [[routing-and-filter-design]] --requires--> [[subscription-filter-rules]]
- [[routing-and-filter-design]] --affects--> [[subscriber-and-manager-design]]
- [[routing-and-filter-design]] --produces--> [[event-type-registration-rules]]

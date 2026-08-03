---
type: concept
id: CON-002
name: Webhooks Concept
domain: service-bus
status: active
---

# Webhooks Concept

## Definition

A **webhook** is a registered subscription that binds:

1. A **target URL** (subscriber HTTPS endpoint)
2. One or more **event types** the subscriber cares about
3. **Credentials and system context** so RamBase can authorize and scope delivery

When a matching event is published to the bus, the subscriber component delivers an HTTP request to the target URL carrying a documented payload format.

## Push callback pattern

Webhooks implement the industry **HTTP callback** pattern:

```text
RamBase (source) ──► Service Bus ──► Subscriber ──► POST https://partner.example/hooks/rambase
```

The partner **does not** open a connection into RamBase for each notification; RamBase infrastructure initiates outbound HTTP.

## Webhook vs. generic message consumer

| Webhook | Generic consumer |
|---------|------------------|
| HTTPS POST to partner URL | Custom protocol or queue client |
| Filtered by event type per registration | Often single queue per app |
| Managed in WHA/WHT archives | External ops concern |
| RamBase subscriber owns retry to URL | Partner may pull with own SDK |

This platform standardizes on HTTPS webhooks for integrator accessibility.

## Lifecycle events

Special event types exist for webhook **meta-operations**:

- WebHookCreated
- WebHookUpdated
- WebHookDeleted

These are routed to the **Manager** rather than the standard subscriber path so subscription topology updates without manual broker reconfiguration.

## Security model (conceptual)

- Target URLs must be reachable from the bus network zone.
- TLS is expected for production endpoints.
- Payloads may include signatures or tokens — exact mechanism belongs in operator guidance; secrets never stored in this vault.
- API client credentials for ERP access are distinct from webhook URL authentication.

## Output format

Payload structure, headers, and serialization rules are documented under [[event-payload-and-formats]] and operator notes on webhook output formats. Subscribers should version their handlers when format fields expand.

## Failure semantics

A webhook registration does not guarantee delivery if:

- Target URL is down beyond retry budget
- Event type is deprecated
- System is deactivated on the bus
- Filter mismatch after configuration change

Operators distinguish **registration health** vs. **delivery health**.

## Relationships

- [[webhooks-concept]] --requires--> [[events-concept]]
- [[webhooks-concept]] --subscribes_to--> [[event-subscription-concept]]
- [[webhooks-concept]] --delivers_to--> [[subscriber-and-manager-design]]
- [[webhooks-concept]] --managed_by--> [[webhook-lifecycle]]
- [[webhooks-concept]] --filtered_by--> [[routing-and-filter-design]]

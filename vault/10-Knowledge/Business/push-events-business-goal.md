---
type: business
id: BUS-001
name: Push Events Business Goal
status: active
approval_required: false
---

# Push Events Business Goal

## Problem statement

Enterprise integrations traditionally **poll** RamBase APIs on a schedule. Polling wastes capacity, increases latency, and complicates error handling when the ERP state changes between polls. Integrators need **push semantics**: when meaningful business activity occurs inside RamBase, interested external systems should be notified without polling.

## Desired outcome

RamBase ERP should **emit push events** to registered subscribers when documented business triggers occur. Subscribers implement their own endpoints (webhooks) or message consumers and react in near real time.

## Value proposition

| Stakeholder | Value |
|-------------|-------|
| Integration partners | Lower latency, simpler integration loops |
| Internal product teams | Decouple ERP core from notification delivery |
| Operations | Centralized delivery, retry, and observability |
| Security | Credential boundaries between ERP and delivery plane |

## Scope boundaries

**In scope**

- Recording business-meaningful changes as events inside RamBase
- Publishing those events through a dedicated Service Bus plane
- Delivering to HTTP webhook endpoints per subscription rules
- Operational lifecycle: activate system, deploy bus version, manage subscriptions

**Out of scope (by design of push layer)**

- Guaranteed transactional coupling with subscriber business logic
- Replacing RamBase API for bulk data extraction
- User-facing UI inside RamBase for every subscriber's logic

## Success criteria

1. A registered event type fires when its business trigger occurs.
2. Matching subscriptions receive payloads without manual polling.
3. Failures are retried according to policy without silent loss.
4. Operators can trace a delivery from event creation to HTTP response.

## Relationships

- [[push-events-business-goal]] --triggers--> [[events-concept]]
- [[push-events-business-goal]] --requires--> [[webhooks-concept]]
- [[push-events-business-goal]] --supported_by--> [[architecture-overview]]
- [[push-events-business-goal]] --explained_by--> [[stakeholders-and-use-cases]]

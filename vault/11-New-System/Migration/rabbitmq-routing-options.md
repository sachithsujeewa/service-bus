---
type: migration
name: RabbitMQ Routing Options
status: active
chapter: 11-New-System
candidate_only: true
---

# RabbitMQ Routing Options (Candidate Analysis)

**Status:** Exploratory — applies **only if** RabbitMQ is selected. Not a program decision.

## Problem

Azure Service Bus **SqlFilter** rules implement webhook routing in the broker. RabbitMQ has no equivalent SQL predicate engine. This is the **largest single migration work item** (~3–5 days for filter engine alone in internal estimate).

## Option A — Fan-out + in-process filtering

Publish every event to all consumer queues; each subscriber evaluates filters locally.

| Pro | Con |
|-----|-----|
| Simple; supports arbitrary `ParameterFilter` | All subscribers receive all messages — bandwidth waste at scale |
| No broker plugin | CPU cost on discard |

**ASUC stress:** High webhook count, high event rate systems.

## Option B — Headers exchange routing (recommended in POC doc)

Use RabbitMQ **headers exchange** with `EventType` and `Database` as headers; bind queues with matching arguments.

| Pro | Con |
|-----|-----|
| Server-side routing for ~90% of webhooks | Cannot evaluate arbitrary `ParameterFilter` server-side |
| Mirrors current architecture | Needs in-process fallback for complex predicates |

**Hybrid:** Headers for type+database; in-process for `ParameterFilter` and ignore-users.

## Option C — MassTransit (or similar)

.NET abstraction over RabbitMQ with routing middleware.

| Pro | Con |
|-----|-----|
| Less boilerplate | Framework dependency; adapt existing retry/HA code |

## Effort summary (RabbitMQ path)

| Area | Estimate |
|------|----------|
| Pub/sub plumbing | 1–2 days |
| Filter evaluation engine | 3–5 days |
| Dynamic webhook CRUD | 2–3 days |
| HA coordinator reuse | 0.5 days |
| Duplicate detection (Redis/DB) | 1–2 days |
| Testing & integration | 5–7 days |
| **Total** | **~2–4 weeks** |

## Maps directly (low effort)

Retry/backoff (`BaseSubscriber`), HMAC (`Poster`), logging (`SBEventLogger`), HA SQL logic (`CloudStateMonitor`) — broker-agnostic.

## Must rebuild

- SQL filter routing → app or headers exchange
- Unlimited redelivery → explicit `nack(requeue)` + backoff policy

## Relationships

- [[rabbitmq-routing-options]] --compared_with--> [[routing-strategy-options]]
- [[rabbitmq-routing-options]] --candidate_for--> [[broker-decision-pending]]
- [[rabbitmq-routing-options]] --replaces--> [[10-Knowledge/Design/routing-and-filter-design]] (if adopted — via new chapter only)

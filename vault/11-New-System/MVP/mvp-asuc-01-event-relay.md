---
type: use-case
id: ASUC-MVP-01
name: MVP Event Relay
chapter: 11-New-System
scope: mvp
status: active
---

# ASUC-MVP-01: End-to-End Event Relay

## Statement

When an authorized source system emits a business event, the bus delivers exactly one HTTP notification to each matching registered partner webhook within the MVP SLO.

## Actors

- Source App (RamBase stand-in)
- Service Bus API + Dispatcher
- Partner App (integrator stand-in)

## Preconditions

- Partner webhook registered for `EventType` and `Database`
- RabbitMQ and Postgres healthy
- Partner endpoint reachable from dispatcher container network

## Main flow

1. Source POST `/api/events` with API key `source`
2. API validates auth, normalizes envelope, persists cursor
3. API publishes JSON to exchange `rambase.events`
4. Dispatcher consumes, loads webhooks, applies filter engine
5. Dispatcher POST JSON to `targetUrl` with `X-Rambase-EventID`
6. Partner returns 200 → dispatcher acks message

## Acceptance criteria

- [ ] HTTP 202 from API on publish
- [ ] Partner receives body with `eventType`, `ramBaseEventId`, `parameters`
- [ ] Header `X-Rambase-EventID` equals `{systemId}-{ramBaseEventId}`
- [ ] p99 latency < 5s on docker-compose (local)

## Quality scenarios

| Scenario | Expected |
|----------|----------|
| Duplicate broker redelivery | Partner may see duplicate; logs show same EventID |
| Invalid auth | 401, no broker message |

## Relationships

- [[mvp-asuc-01-event-relay]] --validated_by--> [[mvp-deployment-architecture]]

---
type: skill-execution-output
name: MVP Discovery and Prototype Scaffold
status: completed
created: 2026-07-30
---

# MVP Discovery and Prototype Scaffold

## Delivered

1. Vault discovery: `11-New-System/MVP/` — use cases, tech stack, deployment architecture
2. Runnable prototype: `prototype/` — docker-compose stack

## Use cases documented

- ASUC-MVP-01..07 + functional F-01..F-06 in [[mvp-use-case-catalog]]
- Expanded: [[mvp-asuc-01-event-relay]], [[mvp-asuc-02-webhook-registration]], [[mvp-asuc-05-retry-dlq]]

## Prototype components

| Container | Role |
|-----------|------|
| source-app | Mock RamBase emitter |
| partner-app | Mock integrator + webhook receiver |
| service-bus-api | Auth, registry, publish |
| service-bus-dispatcher | RabbitMQ consume, filter, HTTP POST |
| rabbitmq | Broker |
| postgres | Webhooks, cursor, DLQ |

## Auth model implemented

- `X-Api-Key` roles: source, partner, admin
- RabbitMQ: rambase/rambase vhost rambase
- Partner webhook HMAC verification optional

## Relationships

- [[mvp-discovery-prototype-scaffold]] --produces--> [[mvp-deployment-architecture]]
- [[mvp-discovery-prototype-scaffold]] --implements--> [[mvp-use-case-catalog]]

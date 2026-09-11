# Session — 2026-08-14 visual architecture & RabbitMQ

## Goal

Visual-first explanation of Service Bus 2.0 architecture and why RabbitMQ is the middleman; persist preference for future sessions.

## User requests

| # | Category | You asked | Outcome |
|---|----------|-----------|---------|
| U-031 | architecture | Visualize new architecture; explain why RabbitMQ as middleman (text too heavy) | AI diagrams in `diagrams/mvp/visuals/`; vault note [[mvp-visual-architecture-guide]]; cursor rule `visual-first-explanations.mdc` |

## Decisions

- Explanations should **lead with AI-generated diagrams**, minimal prose.
- Visual assets live under `diagrams/*/visuals/`; vault links from `11-New-System/`.

## Artifacts

- `diagrams/mvp/visuals/service-bus-2-architecture.png`
- `diagrams/mvp/visuals/why-rabbitmq-middleman.png`
- `vault/11-New-System/MVP/mvp-visual-architecture-guide.md`
- `.cursor/rules/visual-first-explanations.mdc`
- Updated `diagrams/README.md`, `mvp-deployment-architecture.md`

| U-033 | architecture | Entity model: archives → logical architecture | `entity-model-archives-to-architecture.png`, vault entity model note |

- Production broker decision still pending ([[broker-decision-pending]])

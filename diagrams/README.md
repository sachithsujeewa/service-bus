# Architecture & MVP Diagrams

Standalone diagram library for the Service Bus workspace. **Source of truth for visuals** — vault notes link here for diagrams; avoid duplicating large ASCII blocks across many notes.

## Folder structure

```text
diagrams/
├── README.md                 ← this index
├── legacy-baseline/            ← 10-Knowledge / historical synthesized model
├── new-system/               ← 11-New-System modernization & as-implemented discovery
├── mvp/                      ← prototype / local runnable stack
├── elevate/                  ← ELEVATE 2026 solution-writeup visuals
└── chat-history/             ← Cursor session summaries (maintain over time)
```

## Diagram index

### Legacy baseline (`10-Knowledge`)

| File | Title |
|------|--------|
| [01-canonical-pipeline.md](legacy-baseline/01-canonical-pipeline.md) | EVR → Publisher → Topic → Subscriber → URL |
| [02-architecture-component-map.md](legacy-baseline/02-architecture-component-map.md) | Pipeline + data stores |
| [03-knowledge-chain.md](legacy-baseline/03-knowledge-chain.md) | Business → domain → arch → design |
| [04-agentic-vault-flow.md](legacy-baseline/04-agentic-vault-flow.md) | base/ skill pipeline |

### Legacy visuals (AI-generated)

| File | Title |
|------|--------|
| [visuals/archive-structures-field-reference.png](legacy-baseline/visuals/archive-structures-field-reference.png) | VET / EVR / WHT / WHA — fields + types |

Vault note: `vault/10-Knowledge/Architecture/archive-structures-visual.md`

### New system (`11-New-System`)

| File | Title |
|------|--------|
| [01-as-implemented-runtime.md](new-system/01-as-implemented-runtime.md) | ServiceBusManager component tree |
| [02-target-k8s-platform.md](new-system/02-target-k8s-platform.md) | Service Bus 2.0 K8s direction |
| [03-component-relationships.md](new-system/03-component-relationships.md) | Target component wiring |
| [04-three-planes.md](new-system/04-three-planes.md) | Control / data / delivery / observability |
| [05-chapter-isolation.md](new-system/05-chapter-isolation.md) | 10-Knowledge vs 11-New-System |
| [06-broker-evaluation-landscape.md](new-system/06-broker-evaluation-landscape.md) | Migration broker options |

### New system visuals (AI-generated)

| File | Title |
|------|--------|
| [visuals/entity-model-archives-to-architecture.png](new-system/visuals/entity-model-archives-to-architecture.png) | RamBase archives → logical architecture → MVP entities |

Vault note: `vault/11-New-System/Target-Architecture/entity-model-archives-architecture.md`

### MVP prototype

| File | Title |
|------|--------|
| [00-proposed-mvp-architecture.md](mvp/00-proposed-mvp-architecture.md) | **Proposed** MVP — five planes, MOCK/REAL legend, pillars |
| [01-platform-overview.md](mvp/01-platform-overview.md) | Docker Compose full stack (as-built) |
| [02-component-relationships.md](mvp/02-component-relationships.md) | Source → bus → partner |
| [03-happy-path-sequence.md](mvp/03-happy-path-sequence.md) | Register → emit → deliver |
| [04-auth-flow.md](mvp/04-auth-flow.md) | API keys + HMAC |
| [05-rabbitmq-topology.md](mvp/05-rabbitmq-topology.md) | Exchanges, queues, DLQ |
| [06-deployment-modes.md](mvp/06-deployment-modes.md) | Docker, local dev, K8s target |

### MVP visuals (AI-generated)

| File | Title |
|------|--------|
| [visuals/service-bus-2-architecture.png](mvp/visuals/service-bus-2-architecture.png) | Service Bus 2.0 — five planes |
| [visuals/why-rabbitmq-middleman.png](mvp/visuals/why-rabbitmq-middleman.png) | Why RabbitMQ in the middle (with vs without) |
| [visuals/rabbitmq-connections-mvp.png](mvp/visuals/rabbitmq-connections-mvp.png) | Full stack connections + RabbitMQ exchanges/queues |
| [visuals/service-bus-complete-visual.png](mvp/visuals/service-bus-complete-visual.png) | **One-page** archives → services → RabbitMQ → delivery |
| [visuals/one-event-journey-flow.png](mvp/visuals/one-event-journey-flow.png) | **Example flow** — ItmShipped EVR → broker → partner |

Vault note: `vault/11-New-System/MVP/mvp-visual-architecture-guide.md`

### Elevate 2026 solution writeup

| File | Title |
|------|--------|
| [../SOLUTION.md](../SOLUTION.md) | ELEVATE 2026 solution writeup (template filled) |
| [elevate/visuals/agentic-vault-before-after.png](elevate/visuals/agentic-vault-before-after.png) | Before (vibe loop) vs after (durable pipeline) |
| [elevate/visuals/agentic-vault-solution-pipeline.png](elevate/visuals/agentic-vault-solution-pipeline.png) | Inbox → skill pipeline → vault/graph → execute |

### Chat history

| File | Title |
|------|--------|
| [INDEX.md](chat-history/INDEX.md) | Master timeline |
| [USER-REQUEST-LOG.md](chat-history/USER-REQUEST-LOG.md) | **Your questions** (U-001 …) |
| — | [README.md](chat-history/README.md) | How to maintain logs |
| — | [SESSION-CHECKLIST.md](chat-history/SESSION-CHECKLIST.md) | Agent end-of-session checklist |
| — | [_TEMPLATE-session.md](chat-history/_TEMPLATE-session.md) | New session file template |
| [2026-07-30-service-bus-vault-and-mvp.md](chat-history/2026-07-30-service-bus-vault-and-mvp.md) | Vault + MVP scaffold |
| [2026-08-03-mvp-demo-docker-and-uis.md](chat-history/2026-08-03-mvp-demo-docker-and-uis.md) | Docker demo, UIs, fixes |
| [2026-08-11-webhook-routing-retry.md](chat-history/2026-08-11-webhook-routing-retry.md) | Custom host webhook; partner inbox retry fix |
| [2026-08-12-vault-architecture-overview.md](chat-history/2026-08-12-vault-architecture-overview.md) | Explained Obsidian vault architecture |
| [2026-08-12-webhook-vs-breakpoint.md](chat-history/2026-08-12-webhook-vs-breakpoint.md) | VS ashx breakpoint / host webhook delivery |
| [2026-08-21-elevate-solution-writeup.md](chat-history/2026-08-21-elevate-solution-writeup.md) | Elevate `SOLUTION.md` writeup |

## Maintenance rules

1. **New diagram** → add file under the right subfolder + row in this README.
2. **Update diagram** → edit file here; vault notes keep a one-line link only.
3. **Chat session** → follow [chat-history/SESSION-CHECKLIST.md](chat-history/SESSION-CHECKLIST.md); rule `.cursor/rules/chat-history-logging.mdc`.
4. Prefer ASCII in `.md` for Obsidian/git diff; optional Mermaid block in same file.

## Vault links

- Legacy maps: `vault/30-Maps/01-master-knowledge-map.md` → points here
- New system: `vault/11-New-System/MVP/mvp-deployment-architecture.md` → `diagrams/mvp/`
- Prototype: `prototype/README.md` → `diagrams/mvp/`

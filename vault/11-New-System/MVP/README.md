---
type: mvp-index
name: MVP Discovery Index
chapter: 11-New-System
status: active
---

# MVP — Discovery Index

Local runnable prototype: `prototype/` at workspace root.

## Documents

| Doc | Purpose |
|-----|---------|
| [[mvp-visual-architecture-guide]] | **Visual** architecture + why RabbitMQ (AI diagrams) |
| [[mvp-use-case-catalog]] | Architectural + functional ASUCs for MVP |
| [[mvp-technology-stack]] | Languages, libraries, auth, messaging |
| `diagrams/mvp/00-proposed-mvp-architecture.md` | **Proposed** MVP architecture (five planes, MOCK/REAL) |
| [[mvp-deployment-architecture]] | Containers, networks, local test flow (as-built) |

## Runnable stack

```text
source-app → service-bus-api → RabbitMQ → service-bus-dispatcher → partner-app
                  ↕ Postgres (webhooks, cursor)
```

Implementation: `../../prototype/` (workspace root).

## Quick start

```bash
cd prototype
docker compose up --build
./scripts/demo.sh
```

## Relationships

- [[README]] --feeds--> [[use-case-catalog-for-architects]]
- [[mvp-deployment-architecture]] --implements--> [[proposed-component-model]]

---
type: program
name: Milestone Roadmap M1-M5
status: active
chapter: 11-New-System
---

# Milestone Roadmap (M1–M5)

Program delivery phases from POP Service Bus modernization wiki.

## Overview

```text
M1 Discovery → M2 Test env → M3 Baseline metrics → M4 Broker POC → M5 Build & rollout
```

## M1 — Current-state discovery

**Goal:** Understand as-is architecture, dependencies, and integration contracts.

**Deliverables:** Architecture documentation, interface inventory, data model capture, risk identification.

**Vault mapping:** [[as-implemented-contract-summary]], [[migration-drivers-and-concerns]]

## M2 — Test environment readiness

**Goal:** Stand up environments mirroring production topology for safe experimentation.

**Deliverables:** Dev/test broker namespaces, RamBase system sandboxes, deployment scripts.

**ASUC seed:** [[uc-broker-migration-poc]]

## M3 — Monitoring and baseline measurements

**Goal:** Quantify current performance before change.

**Deliverables:**

- Resource utilization dashboards (Grafana exists for current bus)
- Functional metrics: publish rate, delivery latency, POST success, queue depth proxies
- Baseline report for before/after comparison
- Alerting improvements

**Concern link:** [[observability-gaps]] — baseline needed to close gap.

## M4 — Broker evaluation and POC

**Goal:** Prove broker layer can be replaced; **not assume RabbitMQ**.

**Deliverables:**

- Broker comparison (MS SB, RabbitMQ, Kafka, NATS+JetStream, Artemis, Redpanda)
- POC implementation
- Test results: ordering, guaranteed delivery, duplicates, broker restart, subscriber failure, backlog recovery
- Go/no-go recommendation

**Vault mapping:** [[broker-evaluation-summary]], [[rabbitmq-routing-options]]

## M5 — Modernization and rollout

**Goal:** Build and migrate to new Service Bus version on Linux/K8s with pipelines.

**Deliverables:**

- Target architecture document
- K8s/Linux deployment design
- CI/CD + GitOps, secrets model
- MVP implementation, E2E tests
- Rollout + rollback plans, handover docs

**Vault mapping:** [[target-platform-direction]], [[proposed-component-model]]

## RACI note

Solution Architect role marked TBD in program export — ASUC and NFR ownership needs explicit assignment before M5 sign-off.

## Relationships

- [[milestone-roadmap-m1-m5]] --belongs_to--> [[modernization-charter]]
- [[milestone-roadmap-m1-m5]] --produces--> [[use-case-catalog-for-architects]]

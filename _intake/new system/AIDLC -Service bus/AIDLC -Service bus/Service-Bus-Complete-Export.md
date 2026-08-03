# Service Bus — Complete Documentation Export

> Exported from: [Service bus](https://rambase.atlassian.net/wiki/spaces/POP/pages/5270208513/Service+bus)
> Space: Platform and Operations (POP)
> Export date: 2026-07-16

---

## Table of Contents

1. [Project Charter](#project-charter)
2. [Role and Responsibilities](#role-and-responsibilities)
3. [RACI Model for Each Milestone - draft](#raci-model-for-each-milestone---draft)
4. [Communication Plan and Decision Records](#communication-plan-and-decision-records)
5. [Project Risk Register](#project-risk-register)
6. [M1 - Current-state discovery](#m1---current-state-discovery)
7. [M2 - Test environment readiness](#m2---test-environment-readiness)
8. [M3 - Monitoring and baseline measurements](#m3---monitoring-and-baseline-measurements)
9. [M4 - Broker evaluation and POC](#m4---broker-evaluation-and-poc)
10. [M5 - Modernization and rollout](#m5---modernization-and-rollout)
11. [Service Bus Architecture (development)](#service-bus-architecture-development)
12. [Service Bus Services & components](#service-bus-services--components)
13. [Service Bus Interfaces / APIs](#service-bus-interfaces--apis)
14. [Service Bus Data & storage](#service-bus-data--storage)
15. [Service Bus Security design](#service-bus-security-design)
16. [Service Bus Observability design](#service-bus-observability-design)
17. [Service Bus NFR = Non-Functional Requirements](#service-bus-nfr--non-functional-requirements)
18. [Service Bus CI/CD + GitOps](#service-bus-cicd--gitops)
19. [Service Bus Secrets & identity](#service-bus-secrets--identity)
20. [Current architecture](#current-architecture)
21. [Current monitoring](#current-monitoring)
22. [Broker Evaluation Scorecard](#broker-evaluation-scorecard)
23. [Evaluation RabbitMQ vs other options](#evaluation-rabbitmq-vs-other-options)
24. [Service Bus Test strategy](#service-bus-test-strategy)
25. [Service Bus Evidence & reports](#service-bus-evidence--reports)
26. [Service Bus Runbooks](#service-bus-runbooks)
27. [Service Bus Monitoring & alerts](#service-bus-monitoring--alerts)
28. [Service Bus Backup & DR](#service-bus-backup--dr)
29. [Service Bus Troubleshooting](#service-bus-troubleshooting)
30. [Service Bus Threat model](#service-bus-threat-model)
31. [Service Bus Secure reviews](#service-bus-secure-reviews)
32. [Service Bus Release notes](#service-bus-release-notes)

---

# Project Charter

> Source: [Project Charter](https://rambase.atlassian.net/wiki/spaces/POP/pages/5435883736/Project+Charter)
> Author: Sajith Hettiarachchi | Last modified: 2026-07-10

| **Field** | **Value** |
| --- | --- |
| **Project Name** | RamBase Infrastructure Modernization: Service Bus Redesigning |
| **Sponsor** | Jonas Pettersson |
| **Technical Owner** | Ragnar Wiik Johansen |
| **Product Manager** | Sajith Hettiarachchi |
| **Project Manager** | Sajith Hettiarachchi |
| **Architect** | Kamil Polikiewicz |
| **Objective** | Redesign and modernize the RamBase ServiceBus by migrating from the legacy Microsoft Service Bus 1.1 to New Service Bus and porting the internal RambaseServiceBus.exe to .NET 8, enabling containerization, cross-platform compatibility, and alignment with the Kubernetes infrastructure strategy — while ensuring zero disruption to existing message flows and integrations. |
| **Status** | Started |

## Supportive Objectives

- **Focuses on the ServiceBus domain** rather than the general VM-to-K8s migration
- **Addresses the actual technical blocker** — the Windows-only dependency of MS Service Bus 1.1x that prevents containerization
- **Includes the .NET 8 modernization** of the custom `RambaseServiceBus.exe`
- **Ties back to the parent initiative** (Kubernetes readiness) without duplicating it
- **Sets a quality constraint** (zero disruption to existing integrations)

## Key Outcomes

### Technical Outcomes

#### 1. Decoupled Architecture
- Systems become **loosely coupled** rather than tightly integrated point-to-point.
- Producers and consumers communicate via the service bus instead of direct dependencies.
- Enables independent development, testing, and deployment of services.

✅ Outcome: Reduced system interdependencies, Improved maintainability and flexibility

#### 2. Standardized Integration Layer
- Common protocols (REST, AMQP, Kafka, etc.) and data formats (JSON, XML).
- Centralized handling of: Routing, Data transformation, Protocol mediation

✅ Outcome: Consistent integration patterns across RamBase ecosystem, Easier onboarding of new services and partners

#### 3. Improved Scalability
- Service bus introduces **asynchronous messaging and event-driven flows**.
- Supports horizontal scaling of services and message brokers.

✅ Outcome: Better handling of peak loads, Reduced risk of system bottlenecks

#### 4. Enhanced Reliability & Fault Tolerance
- Features such as: Message queues, Retry mechanisms, Dead-letter queues

✅ Outcome: No data loss during failures, Increased resilience of business processes

#### 5. Observability & Monitoring
- Central place to track: Message flow, Failures, Latency

✅ Outcome: Faster troubleshooting, Improved system visibility and diagnostics

#### 6. Security Improvements
- Centralized enforcement of: Authentication, Authorization, Encryption

✅ Outcome: Stronger and consistent security governance across integrations

#### 7. Faster Integration Development
- Reusable connectors and integration templates.
- Reduced need to build custom integration logic repeatedly.

✅ Outcome: Shorter development cycles, Lower technical debt

### Business & Operational Outcomes

#### 1. Faster Time-to-Market
✅ Impact: Faster rollout of customer-facing capabilities, Competitive advantage

#### 2. Increased Operational Efficiency
✅ Impact: Reduced operational overhead, Improved process efficiency

#### 3. Improved Customer Experience
✅ Impact: Faster order processing, Better service reliability

#### 4. Reduced Integration Costs
✅ Impact: Lower total cost of ownership (TCO), Reduced long-term IT costs

#### 5. Better Business Agility
✅ Impact: Enables innovation and experimentation, Faster response to market changes

#### 6. Improved Data Consistency & Governance
✅ Impact: Fewer data inconsistencies, Better compliance and reporting

#### 7. Vendor & Ecosystem Enablement
✅ Impact: Stronger partner network, Increased platform adoption

#### 8. Reduced Downtime & Business Risk
✅ Impact: Higher system uptime, Reduced revenue loss from outages

## 🎯 Scope

### In Scope
Modernize the current Service Bus platform by evaluating and implementing a more robust and scalable messaging solution, with RabbitMQ as the primary candidate. The project will improve reliability through enhanced failure handling and recovery mechanisms, increase visibility with better logging and monitoring, support future growth through improved scalability, and establish a streamlined deployment approach for safer and more efficient rollouts of new versions.

### Out of Scope
Implementing new business features or integration capabilities that do not exist in the current Service Bus solution. The project is focused on modernization, reliability, scalability, and operational improvements of existing functionality.

---

# Role and Responsibilities

> Source: [Role and Responsibilities](https://rambase.atlassian.net/wiki/spaces/POP/pages/5435752717/Role+and+Responsibilities)
> Author: Sajith Hettiarachchi | Last modified: 2026-06-03

## 📋 Overview
Identify and discuss team responsibilities by following the instructions for the [Roles and Responsibilities Play](https://www.atlassian.com/team-playbook/plays/roles-and-responsibilities).

## RamBase

| **Role** | **Person** |
| --- | --- |
| Project Sponsor | Jonas Pettersson |
| Development Manager | Kamil Polikiewicz |
| Project Manager | Sajith Hettiarachchi |
| Product Manager | Sajith Hettiarachchi |
| Solution Architect | _(TBD)_ |
| Technical Product Owner | Ragnar Wiik Johansen |

## Steering Committee

| **Person** | **Title** |
| --- | --- |
| Jonas Pettersson | CPO |
| Kamil Polikiewicz | Development Manager |
| Sajith Hettiarachchi | Product Manager Technology |
| Roger Gullhaug | Director Technology |

## 📘 Roles and Responsibilities

| **Stakeholder** | **Role** | **Responsibilities** |
| --- | --- | --- |
| Jonas Pettersson | Main project sponsor | Budget approvals and product feature approvals |
| _(TBD)_ | Solution Architect | Standards, Governance & Quality; Technical standards, architectural guidelines, deployment patterns; Technical risks; Performance and failover test strategies; Technical direction |
| Kamil Polikiewicz | Development Manager | Delivery, Execution & Planning; Application code changes; Development activities planning; Technical standards enforcement; Delivery risks; Performance and failover testing support; Culture of learning |
| Sajith Hettiarachchi | Product Owner | Vision & Value Ownership; Backlog Ownership & Prioritization; Requirements & Stakeholder Collaboration; Decision-Making & Scope Control; Customer & Business Outcomes; Readiness & Adoption |
| Sajith Hettiarachchi | Project Manager | Project Planning & Execution; Risk, Issue & Dependency Management; Communication & Steering; Resource & Capacity Management; Governance & Compliance |
| Bartlomiej Sweklej | DevOps Engineer | _(Details TBD)_ |
| Tor Håkon Haugen | Operations Manager | _(Details TBD)_ |

---

# RACI Model for Each Milestone - draft

> Source: [RACI Model for Each Milestone - draft](https://rambase.atlassian.net/wiki/spaces/POP/pages/5435752757/RACI+Model+for+Each+Milestone+-+draft)
> Author: Sajith Hettiarachchi | Last modified: 2026-06-03

_(This page is currently empty / placeholder for future content)_

---

# Communication Plan and Decision Records

> Source: [Communication Plan and Decision Records](https://rambase.atlassian.net/wiki/spaces/POP/pages/5439455367/Communication+Plan+and+Decision+Records)
> Author: Sajith Hettiarachchi | Last modified: 2026-06-08

## POC Phase Communication Plan (Completed)

| **Meeting** | **Frequency** | **Stakeholders** | **Format** | **Agenda** |
| --- | --- | --- | --- | --- |
| Status call - Internal - 15-20 mins | Daily | _(internal team)_ | Teams Meeting (Online) | Yesterday's Progress, Today's Plan, Blockers / Risks, Dependencies, Notable Incidents or Findings |
| Status call - External - 30 mins | Weekly (Friday) | T&O Team Leads | Teams Meeting (Online) + In person | Progress of internal Activities, Progress of internal activities with dependencies, Progress of external activities with dependencies, Review risk register |
| Steering Meeting - 30 mins | Monthly | Steering committee | Teams Meeting (online) | Provide overall status of Project items and milestones, Highlight any risk for the Project, Review upcoming activities |

---

# Project Risk Register

> Source: [Project Risk Register](https://rambase.atlassian.net/wiki/spaces/POP/pages/5439455353/Project+Risk+Register)
> Author: Sajith Hettiarachchi | Last modified: 2026-06-03

| **Phase / Milestone** | **Risk** | **Description** | **Severity** | **Action Owner** | **Action** | **Status** | **Comment** |
| --- | --- | --- | --- | --- | --- | --- | --- |
| _(No entries yet)_ | | | | | | | |

---

# M1 - Current-state discovery

> Source: [M1 - Current-state discovery](https://rambase.atlassian.net/wiki/spaces/POP/pages/5486215188/M1+-+Current-state+discovery)
> Author: Kamil Polikiewicz | Last modified: 2026-07-09

### Goal
Understand and document how the existing Service Bus works today

**EPIC:** [RPS-6783](https://rambase.atlassian.net/browse/RPS-6783)

### Key Deliverables
- Current architecture diagram, event-flow diagram, EVR processing explanation, DB/table overview, service/process inventory, known failure modes

### Purpose
Create a reliable shared understanding of the existing Service Bus before changing it.

The knowledge-transfer session showed that several critical parts are known operationally, but not fully documented. For example, the Service Bus reads from EVR, tracks the latest processed event, uses Service Bus database tables, and has active/passive behavior between instances. However, some details, such as the exact behavior of the **notifysb.cpp** primitive (rbs code), still need clarification.

### Deliverables

- **Current-state architecture diagram**
- **End-to-end event-flow diagram:**
    - RamBase event creation → EVR entry → Notify Service Bus primitive → Service Bus pickup → Microsoft Service Bus → subscriber/webhook/print receiver delivery
- **Database/table overview:**
    - EVR, published/latest handled events, Service Bus hosts, leader/heartbeat-related tables, log tables
- **Production service inventory:**
    - servers, Windows services, config files, connection strings, scheduled tasks
- **Known failure modes:**
    - out-of-sync latest event ID, failover timing issues, service restart problems, memory growth, missing SQL access after migration
- **Current deployment and restart runbook**

---

# M2 - Test environment readiness

> Source: [M2 - Test environment readiness](https://rambase.atlassian.net/wiki/spaces/POP/pages/5484707889/M2+-+Test+environment+readiness)
> Author: Kamil Polikiewicz | Last modified: 2026-07-09

### Goal
Establish a safe, production-like environment for experiments and validation

**EPIC:** [RPS-6796](https://rambase.atlassian.net/browse/RPS-6796)

### Key Deliverables
Dedicated test Service Bus setup, connected test RamBase systems, load-balancing/routing setup, test-event generator, environment validation report

### Purpose
Create a proper non-production environment where the team can test safely without depending on production Service Bus.

### Deliverables
- Dedicated test Service Bus environment
- Test systems connected to the test Service Bus
- Load-balancer or routing setup similar to production
- Clear configuration guide for moving a system to the test Service Bus
- Test-event generation mechanism
- Test scenarios for: webhook events, print-cloud events, high-volume event generation, service restart, broker restart, subscriber failure
- Environment validation report

### Existing Environments

**Production:**
- ServiceBusProd1 → vmrbsbus03.rambase.local
- ServiceBusProd2 → vmrbsbus04.rambase.local

**Testing:**
- ServiceBusDev1 → vmdevsbus01.rambase.local
- ServiceBusDev2 → vmdevsbus02.rambase.local

RamBase system TEST14 has been used for testing service bus.

---

# M3 - Monitoring and baseline measurements

> Source: [M3 - Monitoring and baseline measurements](https://rambase.atlassian.net/wiki/spaces/POP/pages/5485723677/M3+-+Monitoring+and+baseline+measurements)
> Author: Kamil Polikiewicz | Last modified: 2026-07-09

### Goal
Improve current observability and capture baseline metrics before modernization

**EPIC:** [RPS-6797](https://rambase.atlassian.net/browse/RPS-6797)

### Key Deliverables
Grafana/resource dashboard, CPU/memory/disk/network baseline, event throughput baseline, queue/backlog baseline, service restart/failover baseline, alerting improvements

### Purpose
Improve current monitoring and capture measurable baselines from the existing Service Bus before building the next version.

### Deliverables

#### 1. Current resource-utilization dashboard
Track at minimum: CPU usage per server, memory usage per process, memory growth over time, disk usage, network traffic, process uptime, service restart history, Windows service status, scheduled restart execution status

#### 2. Service Bus functional metrics
Track: events received from RamBase systems, EVR entries picked up, latest processed event per system, event lag per system, messages published to broker, subscriber delivery count, failed delivery count, retry count, backlog size, difference between sent and received messages, leader instance status, heartbeat status

#### 3. Baseline report
Capture over 1–2 weeks: average and peak CPU, average and peak memory, memory growth trend, average/peak event throughput, average event processing delay, largest observed backlog, restart frequency, failover behavior, known bottlenecks

#### 4. Alerting improvements
- Updated Grafana alerts
- Teams workflow alerts replacing outdated Teams notification mechanism
- Alert thresholds for: high memory, service down, leader missing, heartbeat stale, event lag too high, delivery failures, no events processed for suspiciously long time
- Incident checklist

---

# M4 - Broker evaluation and POC

> Source: [M4 - Broker evaluation and POC](https://rambase.atlassian.net/wiki/spaces/POP/pages/5485035563/M4+-+Broker+evaluation+and+POC)
> Author: Kamil Polikiewicz | Last modified: 2026-07-09

### Goal
Evaluate replacement options and prove whether the current broker layer can be replaced

**EPIC:** [RPS-6798](https://rambase.atlassian.net/browse/RPS-6798)

### Key Deliverables
Broker comparison, POC implementation, ordering/delivery validation, failure-mode test results, go/no-go decision

### Purpose
Evaluate whether the current Microsoft Service Bus dependency can be replaced or abstracted. RabbitMQ was discussed as a natural candidate, but the team agreed that this should be investigated rather than assumed.

### Deliverables
- **Broker comparison:** current Microsoft Service Bus, RabbitMQ, other candidates (Apache Kafka, NATS + JetStream, Apache ActiveMQ Artemis, Redpanda)
- **Evaluation criteria:** message ordering, guaranteed delivery, persistence, retry behavior, dead-letter support, subscriber model, Kubernetes compatibility, operational complexity, monitoring support, migration effort
- **POC implementation**
- **POC test results:** event ordering, guaranteed delivery, duplicate handling, broker restart, subscriber failure, Service Bus restart, backlog recovery
- **Recommendation and go/no-go decision**

---

# M5 - Modernization and rollout

> Source: [M5 - Modernization and rollout](https://rambase.atlassian.net/wiki/spaces/POP/pages/5486149650/M5+-+Modernization+and+rollout)
> Author: Kamil Polikiewicz | Last modified: 2026-07-09

### Goal
Build, validate, and migrate to the new Service Bus version

**EPIC:** [RPS-6799](https://rambase.atlassian.net/browse/RPS-6799)

### Key Deliverables
Target architecture, Kubernetes/Linux design, CI/CD pipeline, MVP implementation, E2E test report, rollout plan, rollback plan, handover documentation

### Purpose
Build and migrate to the future Service Bus version, using the test environment and baseline metrics to validate improvement. Target direction: move away from low-level Windows VM operations toward Linux/Kubernetes with proper pipelines and monitoring.

### Deliverables
- Target architecture document
- Kubernetes/Linux deployment design
- CI/CD pipeline
- Configuration and secrets model
- Health checks: liveness, readiness, broker connectivity, SQL connectivity, event-processing health
- Graceful shutdown/draining design
- MVP implementation (Container image, Helm chart or deployment manifests)
- End-to-end validation report
- Performance comparison against current baseline: CPU, memory, throughput, event lag, restart behavior, recovery behavior
- Pilot rollout plan
- Rollback plan
- Final handover documentation
- Emergency team training (Ops)

---

# Service Bus Architecture (development)

> Source: [Service Bus Architecture (development)](https://rambase.atlassian.net/wiki/spaces/POP/pages/5439456088/Service+Bus+Architecture+development)
> Author: Sajith Hettiarachchi | Last modified: 2026-06-03

## High level architecture

_(This page contains architecture diagrams as image attachments. Refer to the Confluence page for visual Mermaid diagrams.)_

**Attachments:** mermaid_1771502759442.png, mermaid_1771502865104.png, mermaid_1771502981682.png, mermaid_1771503049883.png

---

# Service Bus Services & components

> Source: [Service Bus Services & components](https://rambase.atlassian.net/wiki/spaces/POP/pages/5439456176/Service+Bus+Services+components)
> Author: Sajith Hettiarachchi | Last modified: 2026-06-03

- Each service gets a page

_(Placeholder — individual service pages to be added)_

---

# Service Bus Interfaces / APIs

> Source: [Service Bus Interfaces / APIs](https://rambase.atlassian.net/wiki/spaces/POP/pages/5439456188/Service+Bus+Interfaces+APIs)
> Author: Sajith Hettiarachchi | Last modified: 2026-06-03

- REST/gRPC/event contracts

_(Placeholder — detailed API contracts to be documented)_

---

# Service Bus Data & storage

> Source: [Service Bus Data & storage](https://rambase.atlassian.net/wiki/spaces/POP/pages/5439456200/Service+Bus+Data+storage)
> Author: Sajith Hettiarachchi | Last modified: 2026-06-03

- DB schemas
- message/event formats

_(Placeholder — detailed data model to be documented)_

---

# Service Bus Security design

> Source: [Service Bus Security design](https://rambase.atlassian.net/wiki/spaces/POP/pages/5439456212/Service+Bus+Security+design)
> Author: Sajith Hettiarachchi | Last modified: 2026-06-03

- auth/authz model
- secrets management
- threat model (if relevant)

_(Placeholder — detailed security design to be documented)_

---

# Service Bus Observability design

> Source: [Service Bus Observability design](https://rambase.atlassian.net/wiki/spaces/POP/pages/5439456224/Service+Bus+Observability+design)
> Author: Sajith Hettiarachchi | Last modified: 2026-06-03

_(This page is currently empty / placeholder for future content)_

---

# Service Bus NFR = Non-Functional Requirements

> Source: [Service Bus NFR = Non-Functional Requirements](https://rambase.atlassian.net/wiki/spaces/POP/pages/5439456236/Service+Bus+NFR+Non-Functional+Requirements)
> Author: Sajith Hettiarachchi | Last modified: 2026-06-03

- performance
- scaling
- availability
- DR/backup
- logging/observability

_(Placeholder — detailed NFRs to be documented)_

---

# Service Bus CI/CD + GitOps

> Source: [Service Bus CI/CD + GitOps](https://rambase.atlassian.net/wiki/spaces/POP/pages/5439456415/Service+Bus+CI+CD+GitOps)
> Author: Sajith Hettiarachchi | Last modified: 2026-06-03

## Delivery Automation

- CI (build/test)
- CD (deploy)
- GitOps (desired state + reconciliation)

_(Placeholder — detailed CI/CD pipeline design to be documented)_

---

# Service Bus Secrets & identity

> Source: [Service Bus Secrets & identity](https://rambase.atlassian.net/wiki/spaces/POP/pages/5439456427/Service+Bus+Secrets+identity)
> Author: Sajith Hettiarachchi | Last modified: 2026-06-03

_(This page is currently empty / placeholder for future content)_

---

# Current architecture

> Source: [Current architecture](https://rambase.atlassian.net/wiki/spaces/POP/pages/5537529862/Current+architecture)
> Author: Ragnar Wiik Johansen | Last modified: 2026-07-10

_(This page is currently empty / architecture documentation in progress)_

---

# Current monitoring

> Source: [Current monitoring](https://rambase.atlassian.net/wiki/spaces/POP/pages/5527699506/Current+monitoring)
> Author: Ragnar Wiik Johansen | Last modified: 2026-07-07

Currently we have this monitoring in Grafana.

### General overviews for the service bus
[http://dashboards.rambase.net/dashboards/f/c2e947d6-5155-44d3-9890-608c4a0e6734/service-bus](http://dashboards.rambase.net/dashboards/f/c2e947d6-5155-44d3-9890-608c4a0e6734/service-bus)

### Search to see last POST entry for service bus log
_(Refer to Confluence page for detailed search queries)_

---

# Broker Evaluation Scorecard

> Source: [Broker Evaluation Scorecard](https://rambase.atlassian.net/wiki/spaces/POP/pages/5546180610/Broker+Evaluation+Scorecard)
> Author: Obhasha Priyankara | Last modified: 2026-07-15

Comparative evaluation of Azure Service Bus (current baseline) against RabbitMQ, Apache Kafka, NATS + JetStream, Apache ActiveMQ Artemis, and Redpanda. Scores reflect a Microsoft-centric, enterprise integration context with on-prem / hybrid deployment options.

Baseline context: infrastructure inventory suggests dedicated Service Bus VMs in a Rambase-style on-prem environment. Re-weight criteria once actual queue/topic volumes and streaming needs are documented from production telemetry.

## Executive Summary — Weighted Rankings

| **Rank** | **Broker** | **Weighted Score** | **Summary** |
| --- | --- | --- | --- |
| 1 | NATS + JetStream | _85_ | Best overall on ops, performance, TCO, and portability; smaller .NET ecosystem |
| 2 | Redpanda | _82_ | Kafka API with simpler ops; strong for streaming and replay |
| 3 | Apache Kafka | _79_ | Industry-standard event log; heavier day-2 operations |
| 4 | Azure Service Bus _(current)_ | _78_ | Strong for managed queues/topics and .NET; limited replay and portability |
| 5 | RabbitMQ | _76_ | Best classic messaging semantics; self-managed ops at scale |
| 6 | ActiveMQ Artemis | _70_ | Best for JMS legacy; moderate throughput vs Kafka/Redpanda |

## Evaluation Criteria and Weights

| **Criterion** | **Weight** | **What it measures** |
| --- | --- | --- |
| Messaging model | 15% | Queues, pub/sub, routing, sessions, request-reply, DLQ, and message semantics |
| Streaming & replay | 10% | Durable log, consumer groups, time-based replay, event-sourcing readiness |
| Reliability & HA | 15% | Clustering, failover, geo/DR, ordering guarantees, production track record |
| Performance & scale | 15% | Sustained throughput, tail latency, horizontal scale, backpressure behavior |
| Operational ease | 15% | Day-2 ops burden: upgrades, observability, tuning complexity, staffing needs |
| TCO & licensing | 10% | License cost, infra footprint, managed-service pricing, cost curve at volume |
| .NET / Azure fit | 10% | First-class SDKs, Azure identity/integration, Microsoft-centric stack alignment |
| Portability | 10% | Protocol openness, multi-cloud/on-prem flexibility, vendor lock-in risk |

## Detailed Score Matrix

| Criterion (weight) | Azure Service Bus | RabbitMQ | Apache Kafka | NATS + JetStream | ActiveMQ Artemis | Redpanda |
| --- | --- | --- | --- | --- | --- | --- |
| Messaging model (15%) | 5 — Excellent | 5 — Excellent | 3 — Adequate | 4 — Strong | 4 — Strong | 3 — Adequate |
| Streaming & replay (10%) | 2 — Weak | 2 — Weak | 5 — Excellent | 4 — Strong | 3 — Adequate | 5 — Excellent |
| Reliability & HA (15%) | 5 — Excellent | 4 — Strong | 5 — Excellent | 4 — Strong | 4 — Strong | 4 — Strong |
| Performance & scale (15%) | 3 — Adequate | 4 — Strong | 5 — Excellent | 5 — Excellent | 3 — Adequate | 5 — Excellent |
| Operational ease (15%) | 5 — Excellent | 3 — Adequate | 2 — Weak | 4 — Strong | 3 — Adequate | 4 — Strong |
| TCO & licensing (10%) | 3 — Adequate | 4 — Strong | 4 — Strong | 5 — Excellent | 4 — Strong | 4 — Strong |
| .NET / Azure fit (10%) | 5 — Excellent | 4 — Strong | 4 — Strong | 3 — Adequate | 3 — Adequate | 4 — Strong |
| Portability (10%) | 2 — Weak | 4 — Strong | 4 — Strong | 5 — Excellent | 4 — Strong | 4 — Strong |
| _Weighted total_ | _**78**_ | _**76**_ | _**79**_ | _**85**_ | _**70**_ | _**82**_ |

## Workload Pattern Fit

| **Workload pattern** | **Best fit** | **Runner-up** | **Rationale** |
| --- | --- | --- | --- |
| Task queues & work distribution | RabbitMQ | Azure Service Bus | Competing consumers, ack/nack, DLQ, flexible routing |
| Enterprise pub/sub (topics) | Azure Service Bus | RabbitMQ | Subscriptions, filters, sessions |
| Event sourcing / audit replay | Apache Kafka | Redpanda | Immutable log with time-based replay |
| High-throughput analytics pipeline | Redpanda | Apache Kafka | Kafka API with lower ops overhead |
| Low-latency internal signals | NATS + JetStream | RabbitMQ | Sub-ms fan-out; JetStream adds persistence |
| Hybrid cloud + on-prem | RabbitMQ | NATS + JetStream | Runs anywhere; avoids Azure-only coupling |
| Minimal ops / managed PaaS | Azure Service Bus | Redpanda Cloud | Service Bus is zero-infra on Azure |
| Legacy JMS migration | ActiveMQ Artemis | RabbitMQ | Artemis is ActiveMQ successor with JMS-first design |

## Scenario-based Shortlist

### Integration & Task Queues
- **Primary Pick:** RabbitMQ
- **Alternative:** Azure Service Bus

### Event Streaming & Replay
- **Primary Pick:** Redpanda
- **Alternative:** Apache Kafka

### Hybrid / Multi-site
- **Primary Pick:** NATS + JetStream
- **Alternative:** RabbitMQ

## Broker Profiles

### Azure Service Bus — Current Baseline (Score: 78)
- Deployment: Azure PaaS or Premium dedicated clusters
- Best for: Enterprise queue/topic workloads with minimal ops, strong .NET SDK, Azure-native identity
- Watch out: Cost grows with throughput; limited replay; Azure-centric

### RabbitMQ — Classic AMQP broker (Score: 76)
- Deployment: Self-hosted VMs/K8s or managed (CloudAMQP, Amazon MQ)
- Best for: Complex routing, task queues, RPC, polyglot microservices
- Watch out: Not a durable event log; clustering needs careful design; ops at scale

### Apache Kafka — Industry-standard event log (Score: 79)
- Deployment: Self-hosted or Confluent / Azure Event Hubs (Kafka API)
- Best for: High-volume event streaming, CDC pipelines, analytics fan-out
- Watch out: Heavier ops (ZooKeeper/KRaft); weaker for classic point-to-point task queues

### NATS + JetStream — Lightweight cloud-native (Score: 85)
- Deployment: Self-hosted, K8s, or Synadia NGS cloud
- Best for: Low-latency pub/sub, edge/IoT, simple ops, lightweight persistence
- Watch out: Smaller .NET ecosystem; JetStream maturity vs Kafka

### ActiveMQ Artemis — JMS / Multi-protocol (Score: 70)
- Deployment: Self-hosted JVM on VMs or K8s
- Best for: JMS workloads, AMQP/MQTT/STOMP polyglot
- Watch out: JVM tuning; moderate throughput; smaller mindshare

### Redpanda — Kafka-compatible, No JVM (Score: 82)
- Deployment: Self-hosted or Redpanda Cloud
- Best for: Kafka API compatibility with simpler ops, lower tail latency
- Watch out: Younger ecosystem vs Apache Kafka

## Decision Guidance

### When to Stay on Azure Service Bus
Remain if workloads are Azure-centric queues/topics, ops headcount is limited, and you do not need multi-day event replay or multi-cloud portability.

### When to Migrate
Consider change if Azure cost at scale is high, you need on-prem/hybrid without Premium lock-in, require Kafka-style replay, or want lighter self-hosted ops.

| **Target** | **Description** |
| --- | --- |
| **RabbitMQ** | Integration and task queues (closest semantic match) |
| Redpanda / Kafka | Event streaming and replay |
| NATS | Low-latency hybrid deployments |
| Dual-broker | Messaging + streaming when both are first-class |

---

# Evaluation RabbitMQ vs other options

> Source: [Evaluation RabbitMQ vs other options](https://rambase.atlassian.net/wiki/spaces/POP/pages/5537529870/Evaluation+RabbitMQ+vs+other+options)
> Author: Ragnar Wiik Johansen | Last modified: 2026-07-10

## Broker comparison:
- current Microsoft Service Bus
- RabbitMQ
- other relevant candidates, if needed (Apache Kafka, NATS + JetStream, Apache ActiveMQ Artemis, Redpanda)

---

# Service Bus Test strategy

> Source: [Service Bus Test strategy](https://rambase.atlassian.net/wiki/spaces/POP/pages/5439456444/Service+BusTest+strategy)
> Author: Sajith Hettiarachchi | Last modified: 2026-06-03

_(This page is currently empty / placeholder for future content)_

---

# Service Bus Evidence & reports

> Source: [Service Bus Evidence & reports](https://rambase.atlassian.net/wiki/spaces/POP/pages/5439456456/Service+BusEvidence+reports)
> Author: Sajith Hettiarachchi | Last modified: 2026-06-03

_(This page is currently empty / placeholder for future content)_

---

# Service Bus Runbooks

> Source: [Service Bus Runbooks](https://rambase.atlassian.net/wiki/spaces/POP/pages/5439456473/Service+Bus+Runbooks)
> Author: Sajith Hettiarachchi | Last modified: 2026-06-03

_(This page is currently empty / placeholder for future content)_

---

# Service Bus Monitoring & alerts

> Source: [Service Bus Monitoring & alerts](https://rambase.atlassian.net/wiki/spaces/POP/pages/5439456485/Service+Bus+Monitoring+alerts)
> Author: Sajith Hettiarachchi | Last modified: 2026-06-03

_(This page is currently empty / placeholder for future content)_

---

# Service Bus Backup & DR

> Source: [Service Bus Backup & DR](https://rambase.atlassian.net/wiki/spaces/POP/pages/5439456497/Service+Bus+Backup+DR)
> Author: Sajith Hettiarachchi | Last modified: 2026-06-03

_(This page is currently empty / placeholder for future content)_

---

# Service Bus Troubleshooting

> Source: [Service Bus Troubleshooting](https://rambase.atlassian.net/wiki/spaces/POP/pages/5439456509/Service+Bus+Troubleshooting)
> Author: Sajith Hettiarachchi | Last modified: 2026-06-03

_(This page is currently empty / placeholder for future content)_

---

# Service Bus Threat model

> Source: [Service Bus Threat model](https://rambase.atlassian.net/wiki/spaces/POP/pages/5440602123/Service+Bus+Threat+model)
> Author: Sajith Hettiarachchi | Last modified: 2026-06-03

_(This page is currently empty / placeholder for future content)_

---

# Service Bus Secure reviews

> Source: [Service Bus Secure reviews](https://rambase.atlassian.net/wiki/spaces/POP/pages/5440602135/Service+Bus+Secure+reviews)
> Author: Sajith Hettiarachchi | Last modified: 2026-06-03

_(This page is currently empty / placeholder for future content)_

---

# Service Bus Release notes

> Source: [Service Bus Release notes](https://rambase.atlassian.net/wiki/spaces/POP/pages/5439456521/Service+Bus+Release+notes)

_(This page is currently empty / placeholder for future content)_

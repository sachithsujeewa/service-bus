# Target Documentation Structure

## Core idea

The existing documentation should become a chain of reasoning: **why the capability exists**, **what it is expected to do**, **how it is architected and designed**, **which technology realizes it**, and **how it is implemented and used**. This model preserves original meaning and exposes missing links; it does not manufacture requirements.

## Proposed hierarchy

```text
01 Business Context and Goals/
  Purpose and Business Problem.md
  Stakeholders and Users.md
  Business Capabilities and Use Cases.md
  Scope Constraints and Historical Context.md

02 Requirements/
  Business Requirements.md
  Functional Requirements.md
  Quality and Operational Requirements.md
  Security and Access Requirements.md
  Requirement Questions and Gaps.md

03 Domain and Concepts/
  Events Concept.md
  Webhooks Concept.md
  Event Types and Parameters.md
  Subscription Concept.md
  Terminology.md

04 Architecture/
  Architecture Overview.md
  System Context.md
  Component Responsibilities.md
  Event and Delivery Flow.md
  Data Architecture.md
  Deployment Architecture.md
  Architecture Decisions and Limitations.md

05 Design/
  Publisher Design.md
  Routing and Subscription Design.md
  Subscriber and Manager Design.md
  Event and Payload Design.md
  Retry Error and Sequence Handling.md
  Deployment Event Design.md
  State and Interaction Flows.md

06 Technology/
  Service Bus Technology.md
  RamBase API Integration.md
  Data Stores and Tables.md
  Services Processes and Configuration.md
  Technology Limitations and Upgrade Notes.md

07 Implementation/
  Event Archives EVR and VET.md
  Webhook Archives WHT and WHA.md
  Event COFs and Resources.md
  Creating an Event.md
  Activating a System.md
  Release and Deployment Procedures.md
  Environment and Knowledge Transfer.md

08 User and Operator Guidance/
  Creating and Managing Webhooks.md
  Webhook Output Formats.md
  Troubleshooting and Error Handling.md
  Operational Procedures.md

09 Traceability and Maturity/
  Source Content Register.md
  Requirement-to-Implementation Matrix.md
  Decision and Question Register.md
  Documentation Maturity Assessment.md
  Missing Information Log.md

10 Historical Source Archive/
  Original exported Markdown and attachments
```

## Source-to-target map

| Existing source | Primary destination | Related layers |
|---|---|---|
| `Goal.md` | Business Context and Goals | Requirements |
| `What is web hooks-.md` | Domain and Concepts | Business Context |
| `How RamBase could support web hooks.md` | Business Context and Goals | Architecture, Design |
| `Events Concept.md` | Domain and Concepts | Requirements |
| `Event-driven development and code hooks.md` | Domain and Concepts | Business Context, boundary notes |
| `Design.md` | Architecture | Design, User Guidance |
| `Service Bus Design 2.0.md` | Architecture | Design, Technology, Implementation |
| `DataModel.md` and `Event Archives.md` | Data Architecture | Technology, Implementation |
| `Event Formats.md` | Event and Payload Design | User Guidance |
| `Service Bus Error handling.md` | Retry Error and Sequence Handling | Troubleshooting |
| `Study Windows service bus.md` | Service Bus Technology | Decisions and Limitations |
| `Things that should be updated...` | Decisions and Limitations | Question Register |
| Deployment-through-Service-Bus pages | Deployment Event Design | Architecture, Implementation |
| Event COFs/resources/create-event pages | Implementation | Domain and Concepts |
| WHA user pages/output format | User and Operator Guidance | Requirements, Payload Design |
| Release, activation, knowledge transfer | Implementation | Technology, Operations |

## Classification rules

1. **Business Context:** problem, value, user, outcome, scope, or constraint.
2. **Requirements:** only explicit needs, rules, expected behaviour, or constraints. Preserve whether the source says must, should, could, proposal, or question.
3. **Domain and Concepts:** terminology and technology-independent explanation.
4. **Architecture:** boundaries, components, responsibilities, data movement, and deployment topology.
5. **Design:** internal behaviour, interactions, algorithms, states, errors, ordering, and retry handling.
6. **Technology:** products, protocols, frameworks, databases, configuration mechanisms, and hosting.
7. **Implementation:** tables, fields, classes, COFs, resources, concrete configuration, and procedures.
8. **Guidance:** instructions and explanations for users and operators.
9. **Questions:** unresolved statements remain questions; editorial inference must not answer them.

## Traceability identifiers

- `BUS-###` — business goal or need
- `REQ-###` — requirement explicitly supported by source
- `CON-###` — domain concept
- `ARC-###` — architecture statement
- `DES-###` — design statement
- `TEC-###` — technology statement
- `IMP-###` — implementation statement
- `OPS-###` — user or operating procedure
- `QUE-###` — unresolved source question

Each record contains its source page and section, faithful summary, original wording strength, target location, linked upstream/downstream records, editorial changes, confidence, and reviewer.

## Illustrative traceability chain

`BUS-001 Push events from RamBase ERP to subscribers`

→ `REQ-001 Documented expectation to send an event when its business trigger occurs`

→ `ARC-001 EVR → Publisher → Topic → Subscription → Subscriber → Target URL`

→ `DES-001 Publisher reads events and publishes broker messages`

→ `TEC-001 Windows Service Bus topic and subscriptions`

→ `IMP-001 EVR/VET/WHA structures, publisher/subscriber classes, and configuration`

This is a classification example. Every real link must be confirmed against the original pages during restructuring.

## Regeneration method

Use [[09-Content-Discovery-and-Regeneration-Framework]] to convert each source unit into questions, answer them from the source, route the answers into this hierarchy, and verify the regenerated sections against the original content.

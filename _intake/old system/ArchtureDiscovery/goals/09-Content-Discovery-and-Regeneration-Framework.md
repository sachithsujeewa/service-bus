# Content Discovery and Regeneration Framework

![Content Discovery and Regeneration Framework](images/content-discovery-regeneration-framework.png)

## Purpose

This framework turns the existing event, webhook, and Service Bus documentation into structured questions, uses the answers already present in the source to build understanding, and then regenerates the content into the logical hierarchy defined in [[00-Target-Documentation-Structure]].

The framework improves structure, explanation, and traceability. It does not invent requirements. If a question cannot be answered from the source, the answer is **Not specified in source documentation**, with a link to [[05-Missing-Information-Register]].

## The discovery cycle

`Content → Questions → Source-backed answers → Concepts and relationships → Documentation layer → Regenerated section → Traceability review → Gap update`

### 1. Capture

Divide each source page into meaningful content units: goal, statement, rule, component description, flow step, field, example, procedure, limitation, proposal, or question.

### 2. Question

Convert each unit into one or more questions that reveal:

- **Why:** purpose, business value, actor, outcome, problem, constraint.
- **What:** capability, behaviour, information, rule, interface, result.
- **How:** architecture, design, technology, implementation, procedure.
- **Evidence:** source page, section, table, diagram, example, or code/configuration description.
- **Connection:** upstream reason and downstream realization.

### 3. Answer from source

Answer using only the exported material. Record:

- source page and section;
- faithful answer or summary;
- wording strength: fact, must, should, could, proposal, or question;
- confidence: explicit, inferred from multiple passages, ambiguous, or missing;
- contradictions and related pages.

### 4. Build understanding

Combine compatible answers into a topic model:

- purpose and users;
- domain concepts and terminology;
- expected behaviour;
- components and responsibilities;
- information and state;
- flows and interactions;
- technology and implementation;
- operation, failure, and limitations;
- original questions and proposed improvements.

### 5. Regenerate

Write a professional target section in this order:

1. Purpose and context.
2. Scope and terminology.
3. Source-supported requirements or rules.
4. Architecture explanation.
5. Detailed behaviour and flows.
6. Technology and implementation detail.
7. Examples and procedures.
8. Limitations, source questions, and missing information.
9. Traceability to original pages and related target sections.

### 6. Verify

Compare the regenerated section against the source units. Confirm that:

- nothing unique was omitted;
- no statement became stronger or weaker;
- proposals and questions did not become requirements;
- technology details did not become business requirements;
- duplicates were consolidated with all provenance retained;
- images, tables, examples, and procedures remain linked;
- sensitive values were not reproduced.

## Question-to-document routing

| Question type | Target layer |
|---|---|
| Why does the capability exist? Who benefits? | Business Context and Goals |
| What must, should, or may happen? | Requirements |
| What do the core terms mean? | Domain and Concepts |
| What components exist and how are responsibilities divided? | Architecture |
| How does behaviour work step by step? | Design |
| Which products, protocols, databases, and APIs are used? | Technology |
| Which tables, fields, classes, resources, settings, or procedures realize it? | Implementation |
| How does a user or operator perform a task? | User and Operator Guidance |
| What is unclear, contradictory, proposed, or missing? | Traceability, Questions, and Gaps |

## Content-derived question catalogue

The following questions come directly from themes and statements found in the exported pages.

### A. Goal and business understanding

1. What original goal is stated for subscription events and webhooks?
2. What problem does push delivery solve compared with a consumer polling RamBase?
3. Which examples demonstrate the intended business use of events?
4. Who are the documented actors: RamBase, customer, publisher, subscriber, manager, administrator, or target system?
5. Which content concerns external customer webhooks, internal asynchronous events, synchronous code hooks, or deployment synchronization?
6. Are those four concerns described as one capability or as related but distinct concepts?
7. Which business outcomes are explicit, and which are only implied by examples?

### B. Event concept and lifecycle

1. What is an event according to the source?
2. Which business or system action creates an event?
3. Where is an event registered, and what makes its identifier unique?
4. What roles do EVR and VET have?
5. What information belongs to an event record?
6. How are event parameters defined, typed, required, and exposed publicly?
7. What does “Allow webhook” mean for an event type?
8. Which COF or shared method creates an event?
9. When should event creation occur relative to the underlying business change?
10. How do event-triggered forms and synchronous code hooks differ from external webhooks?
11. Which event lifecycle rules are explicit, ambiguous, or absent?

### C. Webhook and subscription concept

1. How does the source define a webhook?
2. What is stored in WHT and WHA?
3. Which fields identify a subscription, customer/system, event type, target URL, format, status, and resource?
4. What activates or deactivates a webhook?
5. How are webhooks grouped into Service Bus subscriptions?
6. Why are webhooks with the same remote URL grouped together?
7. What ordering expectations are documented for a shared URL?
8. How does creating, updating, or deleting WHA content change Service Bus subscriptions and filters?
9. Which subscription lifecycle steps are described in user documentation versus system design?
10. What authentication question is left unresolved in the early design?

### D. Architecture and component responsibilities

1. What is the documented end-to-end path from event creation to the target URL?
2. What responsibility belongs to EVR, publisher, topic, subscription, subscriber, manager, API, WHA, and customer endpoint?
3. Which components run per Service Bus, per RamBase system, per subscription, or per URL?
4. How does the Service Bus determine which RamBase systems it handles?
5. How are topics and subscriptions separated for normal webhook events and deployment events?
6. Which state is held in RamBase, the broker, the repository, and the Service Bus database?
7. Which diagrams and flows describe the same architecture, and where do they differ?
8. Which architecture description represents the earlier design and which represents “Design 2.0”?
9. What changes between those generations?

### E. Publisher design

1. How does the publisher discover new events?
2. Is discovery polling-based, notification-based, or described both ways in different periods?
3. How is the latest processed or published event tracked?
4. What data is placed in the broker message?
5. How does the publisher react to missing events, unavailable APIs, sequencing problems, or restart?
6. How does the source describe the ready flag?
7. Which publisher behaviour belongs to architecture, detailed design, configuration, or later improvement proposals?

### F. Routing, subscriber, and delivery design

1. How are event type, database/system, customer, and URL used for filtering?
2. How does a subscriber receive a message?
3. When and why does it request additional data from the RamBase API?
4. How does it find the remote URL and payload format?
5. What response is treated as successful delivery?
6. What happens when the HTTP request fails?
7. When is a message completed, abandoned, retried, or lost through lock expiry?
8. What retry and sleep behaviour is documented for manager, publisher, and subscriber?
9. What happens when resource parameters are missing?
10. How can a long HTTP request cause duplicate delivery?
11. What limitations were found in the Windows Service Bus receive/lock behaviour?
12. Which error-handling statements are current design and which are study notes or proposed improvements?

### G. Payload and format

1. What fields form the event envelope inside RamBase, inside the Service Bus, and at the customer endpoint?
2. How do those three representations differ?
3. Which JSON and XML formats are documented?
4. How are parameters represented?
5. How is API resource data embedded?
6. How are paging details represented?
7. Which examples are normative descriptions and which are illustrative only?
8. Are dates, times, identifiers, casing, and data types consistently described?
9. Which inconsistencies should be preserved as source conflicts and logged for clarification?

### H. Data model and implementation

1. What responsibilities do EVR, VET, WHT, and WHA have?
2. What fields and relationships are documented for each archive/table?
3. Which example records illustrate their use?
4. Which fields are documented as unused, required, public, active, or status-controlled?
5. Which COFs create, read, list, update, or delete event definitions and parameters?
6. Which API resources participate in the implementation?
7. Which classes are described for manager, publisher, webhook subscriber, and deployment subscriber?
8. Which configuration concepts are reusable, and which values are environment-specific or sensitive?
9. How do implementation structures trace back to the documented architecture and requirements?

### I. Deployment-event documentation

1. Why are deployment changes sent through the Service Bus?
2. How are deployment messages produced and routed?
3. Does the event contain SQL/XML or require cached retrieval?
4. What is the role of dictionary/archive revision identifiers?
5. What happens when the target dictionary revision is lower or higher than the event revision?
6. How are added, removed, or renamed fields handled?
7. How is a cloned or newly connected system synchronized from an earlier event ID?
8. What alternative designs are documented?
9. Which deployment questions remain explicitly unanswered?
10. Should this material be presented as a specialized design branch rather than mixed with customer webhook delivery?

### J. Technology and environments

1. Which version of Windows Service Bus is described, and what upgrade notes exist?
2. Which Service Bus features are described as useful or limiting?
3. Which databases, services, servers, and environments appear in the historical topology?
4. How does the repository map systems to a Service Bus instance?
5. Which configuration settings control topics, subscriptions, retry intervals, polling, source systems, and logging?
6. Which historical settings must be described conceptually without reproducing values?
7. What later ideas—Azure migration, SQL state, event-driven receive, logging changes—are proposals rather than implemented design?
8. How should knowledge-transfer content be separated from stable technology reference?

### K. Operations and user guidance

1. How does an administrator create or update a WHA record?
2. Which status or field values activate a webhook?
3. How is a system activated or deactivated for the Service Bus?
4. How is a new Service Bus version released?
5. What environment setup steps are documented?
6. What troubleshooting information exists for manager, publisher, and subscriber failures?
7. Which instructions are stable procedures and which are environment-specific historical notes?
8. Which user-facing output examples must remain with the webhook guide?

### L. Original limitations, proposals, and questions

1. Which pages contain explicit “problem,” “question,” “limitation,” “TODO,” “should update,” or alternative sections?
2. What was the status of each proposal at the time of the page?
3. Is there source evidence that any proposal was later implemented?
4. Which proposals contradict earlier design statements?
5. Which unanswered questions block understanding of the documented design?
6. Which uncertainties can be resolved by another page in the same export?
7. Which must remain in the missing-information register?

## Answer record

Use one record for each question:

```markdown
### Q-[topic]-NNN — Question

- Source answer status: Answered / Partially answered / Contradictory / Not specified
- Faithful answer:
- Source pages and sections:
- Wording strength: Fact / Must / Should / Could / Proposal / Question
- Confidence: Explicit / Cross-page inference / Ambiguous / Missing
- Documentation layer:
- Related identifiers:
- Duplicate or conflicting content:
- Editorial clarification made:
- Gap reference:
```

## Regenerated section template

```markdown
# Topic title

## Purpose
Why this topic exists according to the source.

## Scope and terminology
What the source includes, excludes, and calls its main concepts.

## Documented requirements and rules
Only source-supported needs and rules, with IDs and provenance.

## Architecture
Components, responsibilities, boundaries, and flows.

## Design
Detailed interactions, states, data, success, and failure behaviour.

## Technology and implementation
Products, protocols, stores, tables, classes, resources, and procedures.

## Examples and guidance
Source examples, output, and user/operator procedures.

## Limitations, questions, and missing information
Unresolved source material without invented answers.

## Traceability
Source pages, upstream/downstream IDs, and related target documents.
```

## Quality checklist

- [ ] Every paragraph answers a documented question.
- [ ] Every answer has source provenance.
- [ ] Original wording strength is preserved.
- [ ] Business, requirement, architecture, design, technology, and implementation content are separated.
- [ ] Duplicate information is consolidated without losing unique details.
- [ ] Contradictions and historical changes are explicit.
- [ ] Missing answers are logged rather than invented.
- [ ] Examples, tables, images, and procedures remain available.
- [ ] Sensitive values are redacted from regenerated content.
- [ ] The section links backward to source and forward/backward across documentation layers.

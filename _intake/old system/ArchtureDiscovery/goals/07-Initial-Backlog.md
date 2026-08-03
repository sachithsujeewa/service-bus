# Initial Documentation Rediscovery Backlog

## Workstream 0 — Preserve and protect the source

- Keep the exported Markdown and attachments unchanged as the historical archive.
- Inventory content that contains sensitive values; redact only in newly structured documents.
- Record encoding damage, broken external links, and content that cannot be interpreted confidently.

**Exit:** preserved source, safe working rules, and documented redaction/encoding register.

## Workstream 1 — Build the source content register

- Assign IDs to all 27 pages, their sections, tables, images, examples, questions, and procedural steps.
- Record title, source URL/page ID, date, content type, summary, and status.
- Reconcile the register with `export-manifest.json` and the local attachment folders.

**Exit:** every source item has a registered disposition; nothing is omitted.

## Workstream 2 — Classify and map

- Classify every source unit using [[00-Target-Documentation-Structure]].
- Give each unit a primary target and related secondary layers.
- Record duplicates, contradictions, terminology variants, proposals, and unanswered questions.

**Exit:** complete source-to-target matrix with no unassigned content.

## Workstream 3 — Restructure business context and requirements

- Consolidate the original goal, business rationale, actors, use cases, scope, and constraints.
- Extract only requirements supported by source wording.
- Preserve whether each statement is a fact, must, should, could, proposal, or question.
- Link silence and ambiguity to [[05-Missing-Information-Register]].

**Exit:** professional business and requirement documents with provenance and no invented requirements.

## Workstream 4 — Restructure concepts and architecture

- Consolidate event, webhook, subscription, and code-hook concepts.
- Build coherent descriptions of system context, components, responsibilities, flows, data architecture, and deployment topology from existing content.
- Preserve historical alternatives, limitations, and open architecture questions.

**Exit:** architecture and concept documents cover all relevant source material and link back to it.

## Workstream 5 — Restructure design and technology

- Consolidate publisher, routing, subscription, manager, subscriber, payload, retry, error, sequencing, and deployment-event design.
- Separate design intent from Windows Service Bus, RamBase API, SQL, and other technology-specific detail.
- Preserve original diagrams and examples using local links.

**Exit:** design and technology layers are distinct, readable, and traceable.

## Workstream 6 — Restructure implementation and guidance

- Consolidate EVR, VET, WHT, WHA, COFs, resources, classes, configuration concepts, activation, release, and environment procedures.
- Separate reusable implementation reference from environment-specific knowledge-transfer content.
- Consolidate webhook management, output formats, error guidance, and operations procedures.
- Replace sensitive values with safe placeholders while retaining a redaction note.

**Exit:** implementation and guidance documents cover all procedures, tables, examples, and attachments.

## Workstream 7 — Create traceability and maturity views

- Assign `BUS`, `REQ`, `CON`, `ARC`, `DES`, `TEC`, `IMP`, `OPS`, and `QUE` identifiers.
- Build source-to-target and cross-layer matrices.
- Show the path from each business need through the documented implementation.
- Score every layer with [[03-Maturity-Model]].
- Log missing links instead of inventing content.

**Exit:** all structured statements have provenance; all claimed relationships and gaps are visible.

## Workstream 8 — Editorial review and publication

- Standardize terminology, headings, formatting, and voice.
- Compare every rewritten section with its original source units.
- Validate tables, examples, local images, links, navigation, and traceability.
- Review specifically for changed meaning or accidentally strengthened requirements.
- Publish the structured set alongside the historical archive.

**Exit:** reviewers approve fidelity, completeness, clarity, and traceability.

## Workstream 9 — Maintenance

- Assign content owners and review dates.
- Use [[04-Engineering-Refinement-Loop]] for every later update.
- Treat future requirements or architectural changes as new, explicitly approved content—not historical rediscovery.

**Exit:** documentation remains structured and the historical/new-content boundary stays clear.

## Suggested first four-week flow

### Week 1

Preserve source; build the content register; classify all pages at document and section level.

### Week 2

Complete detailed mapping; restructure business context, requirements, and concepts.

### Week 3

Restructure architecture, design, technology, implementation, and guidance.

### Week 4

Build traceability; assess maturity; conduct source-comparison review; publish the first structured baseline.

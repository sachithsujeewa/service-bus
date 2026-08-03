# Documentation Refinement Loop

## The loop

**Inventory → Understand → Classify → Map → Restructure → Trace → Review → Publish → Maintain → Refine**

Apply this loop to every source page and then to each target documentation layer. It refines documentation, not the system or its requirements.

The detailed question-generation and section-regeneration method is defined in [[09-Content-Discovery-and-Regeneration-Framework]].

## 1. Inventory

**Why:** Ensure nothing is missed.

**Tasks:** list every page, section, paragraph, table, diagram, image, example, link, code/configuration block, and question; assign a source item ID.

**Exit:** source register reconciles with all 27 pages, nine images, and the export manifest.

## 2. Understand

**Why:** Prevent wording improvements from changing meaning.

**Tasks:** read each item in context; identify why, what, and how; record terminology, wording strength, historical date, ambiguity, contradictions, encoding damage, and sensitive content.

**Exit:** each source item has a faithful summary and editorial notes.

## 3. Classify

**Why:** Separate mixed documentation concerns.

**Tasks:** classify each item as business context, requirement, concept, architecture, design, technology, implementation, guidance, historical context, proposal, or question.

**Exit:** every source item has one primary classification and optional secondary relationships.

## 4. Map

**Why:** Give every item a deliberate home.

**Tasks:** map items into [[00-Target-Documentation-Structure]]; connect related items; identify duplicates, contradictions, and missing layers.

**Exit:** complete source-to-target mapping with no unassigned items.

## 5. Restructure

**Why:** Create readable professional documentation.

**Tasks:** group related material; consolidate repetition; improve headings, sequencing, grammar, and terminology; preserve examples and local images; keep original requirement strength and questions.

**Exit:** target documents are clear and faithful, with no invented substantive content.

## 6. Trace

**Why:** Make the reasoning chain visible.

**Tasks:** link business goals to source-supported requirements, architecture, design, technology, implementation, and guidance; link every derived statement to source.

**Exit:** traceability matrix shows provenance and cross-layer relationships; missing links point to gaps.

## 7. Review

**Why:** Detect omissions and semantic drift.

**Tasks:** compare source and rewritten content; confirm requirement strength; validate terminology, links, tables, examples, images, questions, and redactions.

**Exit:** reviewers confirm clarity, fidelity, coverage, and provenance.

## 8. Publish

**Why:** Make the structured set usable without losing history.

**Tasks:** publish target documents with navigation, status, ownership, source references, and change log; retain the original archive.

**Exit:** readers can navigate from purpose to implementation and back to original evidence.

## 9. Maintain

**Why:** Prevent renewed fragmentation.

**Tasks:** assign owners and review dates; update affected trace links when approved content changes; preserve version history.

**Exit:** documents have active ownership and freshness status.

## 10. Refine

**Why:** Improve clarity as readers find problems.

**Tasks:** route feedback through the same loop; improve structure and wording; treat substantive changes as separately approved changes, never editorial corrections.

**Exit:** update is reviewed, traceable, and classified correctly.

## Per-item workflow

`Unprocessed → Inventoried → Understood → Classified → Mapped → Rewritten → Traced → Reviewed → Published`

Every item stores: source ID, source location, content type, faithful summary, original wording strength, target location, related items, editorial changes, omissions/redactions, gaps, reviewer, and status.

## Review gates

| Gate | Pass condition |
|---|---|
| Coverage | Every source item and attachment has a disposition |
| Classification | Business, requirement, architecture, design, technology, and implementation are not mixed |
| Fidelity | Rewritten content preserves meaning and strength |
| Traceability | Provenance and adjacent-layer links exist |
| Editorial quality | Structure, terminology, formatting, links, images, and examples are consistent |
| Publication | Original archive remains available and target navigation works |

---
type: review
name: Bootstrap Review Queue
status: needs_review
created: 2026-07-30
---

# Bootstrap Review Queue

Notes flagged during vault bootstrap for human or expert approval.

## Level 2 — Human approval required

| Note | Reason |
|------|--------|
| [[creating-an-event]] | Operational procedure affects integrators |
| [[subscription-filter-rules]] | Security and delivery policy |
| [[event-type-registration-rules]] | Contract stability |
| [[topic-per-purpose-decision]] | Architecture decision record |
| [[subscription-grouping-by-target-url]] | Architecture decision record |

## New system chapter (2026-07-30)

Pending decisions in `11-New-System/Decisions/` — see [[new-system-discovery-skill-execution]].

## Known gaps (not approval — enrichment needed)

- Field-level COF/WHA table documentation
- Explicit REQ-### requirement IDs from source verification
- Operator troubleshooting runbook
- Webhook HTTP header catalog (operational)

## Action

Review each note, confirm synthesized content matches organizational truth, then set `approval_required: false` in frontmatter when accepted.

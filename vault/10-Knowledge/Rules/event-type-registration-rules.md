---
type: rule
id: RULE-002
name: Event Type Registration Rules
status: active
approval_required: true
---

# Event Type Registration Rules

## RULE-010: Unique type names

Event type names **must** be unique within the VET scope for a product line. Renames require new type + deprecation period—not inline rename.

## RULE-011: COF completeness

All COFs advertised in Event Resources **must** be populated or explicitly nullable before integrators depend on them in production webhooks.

## RULE-012: Ready before publish

Events **must** respect ready-flag semantics; publisher **must not** skip tolerance window without architectural approval.

## RULE-013: Trigger alignment

ERP trigger **must** fire only after business transaction commit—no speculative events on rollback paths.

## RULE-014: Documentation before announce

New types **must** have payload documentation in integrator-facing material before production webhook registrations are encouraged.

## RULE-015: Security classification

COFs carrying PII or secrets **must** be reviewed before inclusion in default webhook payloads—may require separate restricted event type.

## Relationships

- [[event-type-registration-rules]] --validates--> [[creating-an-event]]
- [[event-type-registration-rules]] --affects--> [[event-payload-and-formats]]
- [[event-type-registration-rules]] --stored_in--> [[data-architecture]]

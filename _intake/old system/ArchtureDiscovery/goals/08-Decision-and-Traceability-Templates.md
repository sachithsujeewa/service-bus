# Decision and Traceability Templates

These templates record what the historical source says and how it is reorganized. A blank field remains blank or says **Not specified in source**; it must not be completed with an invented requirement or decision.

## Source content mapping record

- **Source item ID:**
- **Source page and section:**
- **Original content type:** Goal / Requirement / Concept / Architecture / Design / Technology / Implementation / Guidance / Proposal / Question / Historical context
- **Faithful summary:**
- **Original wording strength:** Fact / Must / Should / Could / Proposal / Question
- **Primary target document:**
- **Related target documents:**
- **Upstream/downstream links:**
- **Editorial changes made:**
- **Information omitted or redacted:** None / Explain
- **Confidence and reviewer:**

## Architecture Decision Record

### ADR-NNN — Decision title

- **Status:** Proposed / Accepted / Superseded / Rejected
- **Date:**
- **Owners/deciders:**
- **Related outcomes and requirements:**
- **Context and problem:**
- **Decision drivers:**
- **Options considered:**
- **Decision:**
- **Why:**
- **How it satisfies requirements:**
- **Positive consequences:**
- **Negative consequences and risks:**
- **Security/privacy/operational impact:**
- **Migration and rollback:**
- **Verification evidence:**
- **Review/expiry trigger:**

## Requirement record

### REQ-DOMAIN-NNN — Requirement title

- **Type:** Functional / Reliability / Security / Privacy / Performance / Operations / Compliance / UX
- **Statement:** The system shall…
- **Why/rationale:**
- **Source/stakeholder:**
- **Owner:**
- **Priority:**
- **Preconditions and trigger:**
- **Expected behaviour:**
- **Exception/degraded behaviour:**
- **Acceptance criteria:**
- **Verification method:** Review / Analysis / Demonstration / Test / Operational measure
- **Related ADR/design/contract/test/runbook:**
- **Status/version:**

## Quality-attribute scenario

- **ID and name:**
- **Source:** Who/what produces the stimulus?
- **Stimulus:** What happens?
- **Environment:** Normal, peak, degraded, maintenance, disaster?
- **Artefact:** What part of the system is affected?
- **Response:** What must the system do?
- **Measure:** Exact threshold, duration, percentile, error budget, or recovery objective.
- **Evidence/test:**

## Risk record

- **ID/title:**
- **Cause:**
- **Risk event:**
- **Impact:**
- **Likelihood / severity / exposure:**
- **Affected outcomes/requirements:**
- **Controls already present:**
- **Treatment:** Avoid / Reduce / Transfer / Accept
- **Owner and due date:**
- **Residual risk and approver:**
- **Evidence/monitoring indicator:**

## Evidence record

- **ID/title:**
- **Type:** Code / Config / Telemetry / Test / Interview / Incident / Vendor documentation / Observation
- **Location/reference:**
- **Captured date and environment:**
- **Owner/custodian:**
- **Claim supported:**
- **Confidence:** High / Medium / Low
- **Limitations/counter-evidence:**
- **Sensitive-data handling:**
- **Review/expiry date:**

## Traceability matrix

| Outcome | Requirement | ADR | Design/contract | Verification | Operational evidence | Status |
|---|---|---|---|---|---|---|
| OUT-NNN | REQ-NNN | ADR-NNN | DES-NNN | TEST-NNN | SLI/RUN-NNN | Draft |

## Interview note

- **Stakeholder/role:**
- **Date/interviewer:**
- **Decisions they own:**
- **Outcomes and pain points:**
- **Current workflow:**
- **Failure scenarios:**
- **Constraints/non-negotiables:**
- **Claims requiring evidence:**
- **New requirements/questions/risks:**
- **Actions, owners, due dates:**

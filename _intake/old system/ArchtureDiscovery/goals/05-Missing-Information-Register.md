# Missing Information Register

## Status vocabulary

`Open`, `Investigating`, `Decision required`, `Resolved`, `Accepted risk`, `Obsolete`.

## Initial gaps identified from the historical material

These are **documentation gaps**: information the exported source does not clearly provide. They are not automatically new requirements and do not authorize redesign. A gap may be resolved only by finding overlooked source evidence, explicitly validating current reality in a separate activity, or obtaining an approved new decision.

| ID | Priority | Missing or uncertain information | Why it matters | Evidence/action needed | Suggested owner | Status |
|---|---|---|---|---|---|---|
| GAP-001 | Critical | Whether the described platform is still deployed and which parts are authoritative | All design work depends on current reality | Inventory services, repositories, deployments, databases, owners, and traffic | Platform owner | Open |
| GAP-002 | Critical | Historical pages contain apparent credentials and environment configuration | Potential security exposure | Secret scan; identify validity; revoke/rotate; remove from authoritative docs; preserve only governed audit evidence | Security | Open |
| GAP-003 | Critical | Current authentication/authorization model for services, administrators, and consumers | Prevent unauthorized access and tenant leakage | Inspect identity flows, RBAC, tokens, service principals, and audit logs | Security/identity | Open |
| GAP-004 | Critical | Webhook authenticity mechanism | Consumers need to verify sender and prevent replay | Confirm signing/TLS scheme, rotation, timestamp/nonce rules, and contract | Security/API | Open |
| GAP-005 | Critical | Delivery guarantee, acknowledgement, retry, expiry, dead-letter, replay, and idempotency semantics | Defines correctness | Trace code and broker configuration; run failure experiments; approve normative semantics | Platform architecture | Open |
| GAP-006 | Critical | Tenant isolation and endpoint validation/SSRF controls | Avoid cross-tenant disclosure and internal-network access | Threat model plus configuration/code tests | Security | Open |
| GAP-007 | High | Authoritative inventory and versions of event types and schemas | Required for compatibility and consumer trust | Extract VET/EVR definitions, code usage, payload samples, and owners | Domain/API governance | Open |
| GAP-008 | High | Schema evolution and deprecation policy | Prevent breaking consumers | Decide compatibility rules and enforcement mechanism | API governance | Open |
| GAP-009 | High | Transactional consistency between business change and event creation | Determines whether events can be missed or emitted incorrectly | Trace transaction boundaries and crash scenarios | Domain engineering | Open |
| GAP-010 | High | Ordering scope, especially multiple webhooks sharing an endpoint | Impacts partitioning and concurrency | Gather consumer requirements; test existing behaviour; record decision | Product/architecture | Open |
| GAP-011 | High | Current capacity and demand: rates, bursts, fan-out, size, backlog, subscriptions | Required for sizing and target design | Obtain telemetry and forecasts; define load profiles | SRE/product | Open |
| GAP-012 | High | SLOs, RTO, RPO, retention, backup, and restore behaviour | Required for operational acceptance | Business impact workshop and recovery exercise | Service owner/SRE | Open |
| GAP-013 | High | Observability coverage and end-to-end correlation | Needed for diagnosis and support | Map logs, metrics, traces, event/delivery IDs, dashboards, alerts | SRE | Open |
| GAP-014 | High | Data classification, payload privacy, retention, deletion, and residency | Compliance and privacy risk | Data inventory and legal/privacy review | Data governance/privacy | Open |
| GAP-015 | High | Technology lifecycle and support status of the Service Bus platform | May force migration | Inventory versions/support dates, vendor constraints, and replacement options | Enterprise architecture | Open |
| GAP-016 | High | Boundary between webhook delivery, deployment synchronization, internal async events, and synchronous code hooks | Avoid an over-coupled platform | Domain workshop and capability map | Product/architecture | Open |
| GAP-017 | Medium | Current subscription lifecycle, roles, validation, audit, testing, and rotation | User/admin specification incomplete | Walk through live process with administrators | Product/support | Open |
| GAP-018 | Medium | Enrichment consistency when API data changes or disappears before delivery | Payload may not represent event-time truth | Define snapshot/link semantics and failure handling | Domain/API | Open |
| GAP-019 | Medium | Handling of field rename/removal and dictionary version mismatch for deployment events | Historical notes leave this unresolved | Confirm current dictionary rules and test upgrade/downgrade cases | Deployment engineering | Open |
| GAP-020 | Medium | Clone/catch-up synchronization behaviour and correctness | Risk of missing/duplicating state | Review implementation and conduct controlled recovery test | Platform/DBA | Open |
| GAP-021 | Medium | Customer-facing limits, quotas, support boundaries, and error guidance | Required for dependable integrations | Product policy and consumer research | Product/support | Open |
| GAP-022 | Medium | Current release, rollback, configuration, and secret deployment process | Operational and change risk | Observe one release and inspect pipeline/change records | Release engineering | Open |
| GAP-023 | Medium | Documentation ownership and review cadence | Prevents another documentation drift cycle | Establish RACI and freshness automation | Service owner | Open |
| GAP-024 | Medium | Encoding corruption and fidelity issues in exported historical pages | Can distort technical meaning | Compare critical passages with source and repair only in rewritten docs | Documentation owner | Open |

## Gap record template

### GAP-NNN — Title

- **Status:** Open
- **Priority:** Critical / High / Medium / Low
- **Owner:** Unassigned
- **Question:**
- **Why it matters:**
- **Current hypothesis:**
- **Evidence for:**
- **Counter-evidence:**
- **Decision blocked:**
- **Planned action:**
- **Due date:**
- **Resolution/decision link:**
- **Resolved by and date:**

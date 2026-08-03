---
type: inbox-catalog
name: Source Catalog
status: active
synthesis_policy: no-content-from-intake
---

# Source Catalog (`_intake` archive)

This catalog records **which historical sources exist** in `_intake/` and **which vault notes synthesize their topic**. No valuable export content is duplicated here.

## Archive location

```text
_intake/old system/ArchtureDiscovery/
```

## Source → synthesized knowledge map

| Archive topic (filename) | Primary vault notes | Layer |
|--------------------------|---------------------|-------|
| Goal | [[push-events-business-goal]] | Business |
| What is web hooks | [[webhooks-concept]] | Domain |
| Events Concept | [[events-concept]] | Domain |
| Event Formats | [[event-payload-and-formats]] | Domain |
| Subscription events (Web hooks) | [[event-subscription-concept]] | Domain |
| Event-driven development and code hooks | [[event-driven-integration-patterns]] | Domain |
| Design | [[architecture-overview]] | Architecture |
| Service Bus Design 2.0 | [[architecture-overview]], [[publisher-design]], [[subscriber-and-manager-design]] | Arch + Design |
| DataModel | [[data-architecture]] | Architecture |
| System Documentation Events | [[events-concept]], [[creating-an-event]] | Domain + Process |
| Event Archives / COFs / Resources | [[data-architecture]] | Architecture |
| Service Bus Error handling | [[retry-error-and-sequence-handling]] | Design |
| Study Windows service bus | [[service-bus-technology]] | Technology |
| Deployment through Service Bus | [[deployment-event-design]], [[deployment-architecture]] | Design + Arch |
| How RamBase could support web hooks | [[push-events-business-goal]], [[stakeholders-and-use-cases]] | Business |
| WHA user docs / output format | [[event-payload-and-formats]], [[webhook-lifecycle]] | Domain + Process |
| Release / activation pages | [[release-and-deployment]], [[activating-a-system]] | Process |
| Knowledge transfer | [[release-and-deployment]] | Process |
| goals/* (rediscovery framework) | [[03-traceability-map]], [[What-This-Vault-Is]] | Meta |

## Processing status

```text
All cataloged sources: synthesized at vault bootstrap (2026-07-30)
Reprocess policy: agent reads archive → patches vault notes (never bulk copy)
```

## Agent instruction

When enriching notes from archive:

1. Read source in `_intake/` only in agent session—not copy into vault
2. Patch `10-Knowledge/` with verified facts
3. Record execution trace in `01-Processing/`
4. Never store credentials from legacy exports

## Relationships

- [[00-source-catalog]] --documents--> [[01-master-knowledge-map]]
- [[00-source-catalog]] --derived_from--> `_intake/` (external archive)

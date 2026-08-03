---
type: inbox-catalog
name: New System Source Catalog
chapter: 11-New-System
status: active
---

# New System Source Catalog

Pointers to `_intake/new system/` — **no export content duplicated**.

## Root

```text
_intake/new system/AIDLC -Service bus/AIDLC -Service bus/
```

## Files scanned (2026-07-30)

| Source file | Vault notes | Classification |
|-------------|-------------|----------------|
| `ARCHITECTURE_CONTRACT.md` | [[as-implemented-contract-summary]], concerns | As-implemented discovery |
| `Service Bus 2.0.md` | [[as-implemented-contract-summary]], [[rabbitmq-routing-options]] | Analysis + POC |
| `REASONS_TO_MIGRATE 1.md` | [[migration-drivers-and-concerns]], concern register | Migration rationale |
| `RabbitMQ-Migration-Analysis.md` | [[rabbitmq-routing-options]] | Candidate evaluation |
| `Service-Bus-Complete-Export.md` | Program + broker scorecard notes | Program wiki export |

## Agent policy

Re-scan on file changes; update `11-New-System/` only via [[04-Agent-Skills/13-new-system-discovery-skill]].

## Relationships

- [[01-new-system-source-catalog]] --documents--> `_intake/new system/`
- [[01-new-system-source-catalog]] --produces--> [[04-new-system-map]]

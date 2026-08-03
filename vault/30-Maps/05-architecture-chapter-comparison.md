---
type: map
name: Architecture Chapter Comparison
status: active
---

# Architecture Chapter Comparison

Guide for using **two isolated architecture narratives** in this vault.

## Chapters

| Chapter | Path | Narrative | Use when |
|---------|------|-----------|----------|
| **Legacy / baseline** | `10-Knowledge/` | Synthesized ERP + bus model from historical docs | Onboarding, integrator concepts, generic flows |
| **New system / modernization** | `11-New-System/` | As-implemented discovery + target + migration | ASUC writing, M4/M5 design, broker decision |

## Do not contaminate

- Do not merge broker topology details (per-system topic, property bag, MD5 subs) into `10-Knowledge/Architecture/*` without explicit review.
- Do not copy migration concerns into legacy Design notes.
- Cross-reference using typed links only.

## Cross-chapter link vocabulary

```text
contrasts_with      Different model or era
targets_replacement_of  New supersedes old (future)
preserves_contract_from Stable integrator contract lineage
compared_with       Option or era comparison
```

## Key contrasts

| Topic | Legacy chapter | New system chapter |
|-------|----------------|-------------------|
| Topic naming | MainTopic + DeployTopic (synthesized) | **Per-system topic = NGSystem.Name** (as-implemented) |
| Message payload | Body + COFs (conceptual) | **Properties only; body = "Rambase Event"** |
| HA | NGSystem poll, bus instance | **CloudStateMonitor SQL heartbeat** |
| Broker | MS Service Bus generically | **EOL + migration program** |
| Delivery | At-least-once (general) | **Skip paths, infinite retry, 60s failover** detailed |
| Target | Windows service | **.NET 8 + K8s** |

## For ASUC authors

1. Cite **as-is** behavior from `11-New-System/Discovery/`
2. Cite **constraints** from [[preserve-vs-replace-contract]]
3. Cite **concerns** from [[architectural-concerns-register]]
4. Reference legacy chapter only for **business/domain** context (events, webhooks)

## Relationships

- [[05-architecture-chapter-comparison]] --explains--> [[11-New-System/README]]
- [[05-architecture-chapter-comparison]] --explains--> [[What-This-Vault-Is]]

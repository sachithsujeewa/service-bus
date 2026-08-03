---
type: agent-skill
name: New System Discovery Skill
status: active
chapter: 11-New-System
---

# New System Discovery Skill

## Purpose

Deep-scan `_intake/new system/` for modernization proposals, as-implemented contracts, migration analysis, and program artifacts. **Update only `11-New-System/`** — never patch `10-Knowledge/` architecture notes directly.

## Input contract

```json
{
  "source_root": "_intake/new system/",
  "mode": "full-scan | delta-scan | concern-extract | use-case-seed",
  "output_trace": true
}
```

## Output contract

```json
{
  "status": "passed | needs_review | failed",
  "discovered_concerns": [],
  "updated_notes": [],
  "new_use_case_seeds": [],
  "execution_trace_path": "01-Processing/...",
  "issues": []
}
```

## Procedure

1. Enumerate all markdown under `source_root` (recursive).
2. Classify each file:
   - **As-implemented discovery** (e.g. architecture contract, codebase analysis)
   - **Migration rationale** (drivers, pitfalls, red-team Q&A)
   - **Target / proposed** (charter, K8s direction, placeholder designs)
   - **Evaluation** (broker scorecard, RabbitMQ POC analysis)
   - **Program** (milestones, RACI, risks)
3. Extract **architectural concerns** — assign `CONC-###` ids:
   - Platform EOL, broker lock-in, HA gaps, delivery ambiguity, security debt, observability gaps, scale ceilings, DR gaps
4. Map concerns to quality attributes: availability, reliability, security, operability, portability, performance, maintainability.
5. Synthesize deep notes into `11-New-System/` subfolders — **no bulk copy** from intake.
6. Cross-link to `10-Knowledge/` only via:
   - `contrasts_with`
   - `targets_replacement_of`
   - `preserves_contract_from`
7. Seed ASUC notes in `11-New-System/Use-Cases/` when a scenario implies stakeholder + trigger + quality attribute stress.
8. Record trace in `01-Processing/`.
9. Update `30-Maps/04-new-system-map.md` and `99-Graph/new-system-graph.json`.

## Validation rules

- Never store secrets from source exports.
- Never edit `10-Knowledge/Architecture/*` or `10-Knowledge/Design/*` except adding outbound links from new chapter if explicitly requested.
- Flag broker selection as **pending** until decision record approved.
- Placeholder target pages must say `status: draft` in frontmatter.

## Related

- [[05-Agent-Prompts/02-scan-new-system-folder]]
- [[base/04-Agent-Skills/12-skill-orchestration-skill]]

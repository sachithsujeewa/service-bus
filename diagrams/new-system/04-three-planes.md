# New System — Three Planes

```text
  CONTROL PLANE
  Registry sync + Leader coordinator + Management consumer
        │ orchestrate who runs what
        ▼
  DATA PLANE
  Publisher + Broker topics + Filter engine
        │ move events toward delivery
        ▼
  DELIVERY PLANE
  Webhook dispatcher + Integrator HTTPS + DLQ (target)
        │
        ▼
  OBSERVABILITY (cross-cutting)
  Metrics, logs, traces, health — all planes
```

## Chapter isolation

```text
vault/10-Knowledge/     Legacy baseline (synthesized historical model)
vault/11-New-System/    Modernization + as-implemented discovery + target

Link across chapters only with:
  contrasts_with | targets_replacement_of | preserves_contract_from
```

## Related

- `diagrams/new-system/05-chapter-isolation.md`
- `vault/30-Maps/05-architecture-chapter-comparison.md`

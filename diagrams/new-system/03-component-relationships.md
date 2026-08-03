# New System — Target Component Relationships (MVP-aligned)

**Purpose:** Service Bus 2.0 logical decomposition for modernization

```text
                    ┌─────────────────────┐
                    │ System registry sync│
                    └──────────┬──────────┘
                               │ provisions
         ┌─────────────────────┼─────────────────────┐
         ▼                     ▼                     ▼
  ┌─────────────┐      ┌─────────────┐      ┌──────────────────┐
  │Wake listener│─────►│Event        │      │Management       │
  │ (TCP poke)  │ wake │publisher    │      │consumer         │
  └─────────────┘      └──────┬──────┘      └────────┬─────────┘
                              │ publish              │ CRUD events
                              ▼                      ▼
                       ┌─────────────┐      ┌─────────────┐
                       │ Message     │◄────►│ Filter      │
                       │ broker TBD  │      │ engine      │
                       └──────┬──────┘      └─────────────┘
                              │ consume (active host)
                              ▼
                       ┌─────────────┐
                       │ Webhook     │──────────► Integrator HTTPS
                       │ dispatcher  │
                       └──────┬──────┘
                              │ telemetry
                              ▼
                       ┌─────────────┐
                       │ Audit /     │──────────► Ops / Grafana
                       │ observability│
                       └─────────────┘

        Leader coordinator ──gates──► Webhook dispatcher (HA)
```

## External data (preserved)

```text
RamBase system SQL (EVR, WHA, EVR_T1)
Repository NG_SYSTEM
RambaseServiceBus control DB
```

## Related

- `vault/11-New-System/Target-Architecture/proposed-component-model.md`
- `diagrams/mvp/02-component-relationships.md` (concrete prototype)

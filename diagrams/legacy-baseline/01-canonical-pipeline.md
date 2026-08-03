# Legacy — Canonical Event Pipeline

**Chapter:** `vault/10-Knowledge/` (baseline)  
**Status:** synthesized from historical documentation

## Pipeline

```text
EVR → Publisher → Topic → Subscription → Subscriber → Target URL
```

## Multi-system on one bus host

```text
ServiceBusService (ServiceBusId=1)
   ├── System RIC      → Publisher_RIC + Subscribers...
   └── System SQLRIC   → Publisher_SQLRIC + Subscribers...
```

## Control vs data plane (legacy model)

```text
Data plane:   MainTopic + business event filters + webhook delivery
Control plane: DeployTopic + manager subscription + webhook meta-events
```

## Related notes

- `vault/10-Knowledge/Architecture/architecture-overview.md`
- `diagrams/legacy-baseline/02-architecture-component-map.md`

# Legacy — Architecture Component Map

**Chapter:** `vault/10-Knowledge/`

## Center pipeline

```text
                    ┌─────────────┐
                    │    EVR      │
                    └──────┬──────┘
                           │
                    ┌──────▼──────┐
                    │  Publisher  │
                    └──────┬──────┘
                           │
              ┌────────────┼────────────┐
              ▼            ▼            ▼
         MainTopic    DeployTopic   ManagerSub
              │            │            │
              ▼            ▼            ▼
         Subscription  DeploySub   Manager
              │            │            │
              ▼            ▼            ▼
          Subscriber   DeployHandler Manager
              │                         │
              ▼                         ▼
         Target URL              Filter updates
```

## Data stores

```text
ERP (EVR, VET, WHA, WHT) ──► Publisher
Repository (NGSystem)      ──► Service host activation
Bus DB (PublishedEvent)    ──► Publisher cursor / logs
Broker                     ──► Subscriptions
```

## Related

- `vault/30-Maps/02-architecture-map.md`

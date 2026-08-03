# New System — Target K8s Platform (Draft)

**Chapter:** `vault/11-New-System/Target-Architecture/`  
**Status:** draft — M5 milestone direction

## Stack shift

```text
Current                          Target
─────────────────────────────────────────────────────
.NET Framework 4.6–4.7           .NET 8
Windows Service on VM            Linux containers on Kubernetes
On-prem MS Service Bus 1.1       Broker TBD (see broker landscape)
App.config secrets               GitOps + secret store (CSI)
Manual deploy                    CI/CD pipeline
Event Log + SQL logs             Metrics + traces + SLO dashboards
```

## K8s namespace sketch

```text
Namespace: service-bus
 ├── deployment: publisher-worker
 ├── deployment: webhook-dispatcher
 ├── deployment: management-consumer
 ├── statefulset | external: broker cluster
 ├── secrets: broker, SQL, HMAC (CSI)
 └── configmaps: poll intervals, backoff tuning
```

## Related

- `vault/11-New-System/Target-Architecture/target-platform-direction.md`

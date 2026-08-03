# New System — As-Implemented Runtime (Discovery)

**Chapter:** `vault/11-New-System/Discovery/`  
**Source:** ARCHITECTURE_CONTRACT analysis (codebase truth)

## One-liner

Poll RamBase SQL (EVR) → on-prem Service Bus topics → HTTP webhooks. Multi-host HA via DB leader election.

## Runtime tree

```text
ServiceBusManager
 ├── CloudStateMonitor (leader election)
 ├── ServiceBusManagementSubscriber (webhook CRUD)
 ├── RBSTcpListener (wake publisher)
 └── RambaseSystemClient × N (per NG_SYSTEM row)
      ├── RambaseSystemPublisher (poll EVR)
      └── WebHookSubscriber × M (per URL subscription)
```

## Broker topology (as-implemented)

```text
Per-system topic     = NGSystem.Name  (e.g. "SQLRIC")
Management topic     = ServiceBusManagementTopic (config)
Webhook subscription = MD5("Sub {systemName} {url}")
Message body         = literal "Rambase Event" (payload in Properties)
MessageId            = {SystemID}-{RamBaseEventId}
```

## HA (as-implemented)

```text
Host 1 (ACTIVE)  ──heartbeat──►  CloudHostSync (SQL)  ◄──heartbeat──  Host 2 (PASSIVE)
                                      ~500ms
Only ACTIVE host runs webhook subscribers; failover gap ~60s
```

## Related

- `vault/11-New-System/Discovery/as-implemented-contract-summary.md`

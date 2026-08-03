# Architecture Contract — RamBase Service Bus

**Purpose:** Capture what this codebase actually does so it can be reimplemented against a different message broker.  
**Scope:** Solution `ServiceBus.sln` (library `ServiceBus` + Windows service host `ServiceBusService`).  
**Sources:** Source tree + `graphify-out/` (God nodes: `RambaseSystemClient`, `RambaseSystemPublisher`, `BaseSubscriber`, `ServiceBusManager`, `CloudStateMonitor`).

---

## 1. One-liner

Windows service that **polls RamBase SQL event tables**, publishes events onto **Azure Service Bus for Windows Server (on-prem) Topics**, and **delivers matching events to HTTP webhooks**. Multi-host HA via DB-backed leader election. Only the active host runs subscribers.

---

## 2. Solution layout

| Project | Role |
|---------|------|
| `ServiceBus` (.NET Framework 4.7.2 class library) | All business logic |
| `ServiceBusService` (.NET Framework 4.6.1 Windows Service) | Host: `Service1` → `ServiceBusManager.Startup()` |

**Broker SDK (legacy):** `WindowsAzure.ServiceBus` 2.2.4 + `ServiceBus.v1_1` — classic `NamespaceManager` / `TopicClient` / `SubscriptionClient` / `BrokeredMessage` / `SqlFilter`.

**Other deps:** EF 6.1.3, Newtonsoft.Json 5.0.6, ParallelExtensionsExtras (`QueuedTaskScheduler`).

---

## 3. Clear-cut architecture

```
┌─────────────────────────────────────────────────────────────────┐
│  Windows Service (Service1 / Program console fallback)          │
│                         │                                       │
│                   ServiceBusManager                             │
│         ┌───────────────┼───────────────────┐                   │
│         ▼               ▼                   ▼                   │
│  CloudStateMonitor  ManagementSub      RBSTcpListener           │
│  (leader election)  (webhook CRUD)     (wake publisher)         │
│         │                                                       │
│         ▼                                                       │
│  RambaseSystemClient × N  (one per NG_SYSTEM row for this cloud) │
│         │                                                       │
│    ┌────┴────┐                                                  │
│    ▼         ▼                                                  │
│ Publisher  WebHookSubscriber × M                                │
│ (poll EVR) (SB subscription → HTTP POST)                        │
└─────────────────────────────────────────────────────────────────┘
         │              │                    │
         ▼              ▼                    ▼
   Per-system SQL   On-prem Service Bus   RambaseServiceBus DB
   (EVR / WHA)      Topics+Subscriptions  (cursor, hosts, logs)
```

### 3.1 Data flow (happy path)

1. RamBase writes an event into system DB (`EVR` / related).
2. Optionally, RamBase opens a TCP connection to this host’s `RBSPort` and sends a 4-byte system ID → publisher wakes early.
3. `RambaseSystemPublisher` polls for next event ID (`LatestPublishedEvent + 1`), waits for `ReadyFlag`, publishes `BrokeredMessage` to **topic named = system name**.
4. Management event types (`WebHookCreated/Updated/Deleted`) go to **management topic** instead.
5. `WebHookSubscriber` receives via subscription SQL filters → `Poster` HTTP POSTs JSON/XML to webhook URL (optional HMAC headers).
6. Cursor saved in `PublishedEvent` table; delivery logged via `SBEventLogger`.

### 3.2 Topology (broker contract you must recreate)

| Entity | Naming / rules |
|--------|----------------|
| **System topic** | Topic path = `NGSystem.Name` (system name). Created if missing. `RequiresDuplicateDetection=true`, history **20 min**, `SupportOrdering=true`. |
| **Management topic** | Config `ServiceBusManagementTopic` (prod: `RamBaseServiceBusManagementTopic`). Same duplicate detection / ordering settings. |
| **Webhook subscription** | Name = MD5 hash of `"Sub {systemName} {url}"` (SB name length limit 50). One subscription per unique remote URL; multiple webhooks can share URL and stack as SQL rules. |
| **Management subscription** | `{ServiceBusCloudId}_{ServicebusHostID}_MGMTSUB`. No special SQL filter (receives all mgmt messages). |
| **Subscription delivery** | `MaxDeliveryCount = int.MaxValue` (effectively no dead-letter by count). |
| **Message body** | Literal string `"Rambase Event"` — **payload is entirely in `BrokeredMessage.Properties`**. |
| **MessageId** | `{SystemID}-{RamBaseEventId}` (used with duplicate detection). |

### 3.3 Message property contract

| Property | Meaning |
|----------|---------|
| `EventType` | Event type string |
| `SystemID` | System name |
| `Database` | Company/DB scope |
| `RamBaseEventId` | Monotonic event id |
| `RegisterTime` | When registered |
| `RegisterPid` | Creating process/user pid (filter: ignore users) |
| `ObjectDetailsUri` | API URI for object details |
| `P_{key}` | Event parameters (prefix `P_`); filter columns `[x]` → `P_x` |

Management event types: `WebHookCreated`, `WebHookUpdated`, `WebHookDeleted`.

### 3.4 SQL filter semantics (webhook rules)

Built by `SubscriptionFilters.GenerateWebHookEventTypeFilter`:

- Always: `EventType = '...'`
- If database not empty/`ALL`: also `Database = '...'`
- Optional: `RegisterPid NOT IN (...)` from webhook ignore-users
- Optional: webhook `PARAMETERFILTER` (after `[col]` → `P_col` rewrite)

**Migration note:** New broker must support equivalent property-based filtering (or you push filtering into consumer code).

### 3.5 HA / multi-host

- Hosts registered in `ServicebusHosts` (per `ServiceBusCloudID`): hostname, `RBSPort`, `ManagementPort`, `Priority`.
- `CloudStateMonitor` heartbeats / updates `CloudHostSync`; decides `IsActiveHost`.
- **Only active host starts webhook subscribers.** Passive hosts abandon messages if they receive any.
- Failover waits ~60s (`30 × 2000ms`) before starting subscribers so lock tokens from previous host expire.
- Publishers still start for new systems regardless of active flag (see `UpdateSystemList`) — **gotcha**.

### 3.6 External databases

| Connection key | DB | Used for |
|----------------|-----|----------|
| `NGSystemConnectionstring` / `RepositoryConnectionstring` | NG_SYSTEM / Repository | List of systems (`NGSystems` filtered by `SB_ID` = cloud id) |
| Per-system connection from `NGSystem.ConnectionString` + `UidAndPasswordForSQLRICxx` | Each RamBase system | EVR poll, webhook defs (`WHA`), archive |
| `RambaseServicebusConnectionstring` | RambaseServiceBus | Hosts, cloud sync, `PublishedEvent` cursor, logs |

### 3.7 Webhook HTTP contract (keep stable across migration)

- POST to `POSTURL`
- Body: JSON or XML (`WHA.FORMAT`)
- Headers: `X-Rambase-EventID`; optional `X-Rambase-Signature` (HMAC of body; alg from webhook: MD5/SHA1/SHA256/… )
- Timeout: 30s (`HttpClient`)
- Active webhooks: `WHA.ST = '4'`

### 3.8 System boundaries & stakeholders

| Stakeholder / system | Role | Direction |
|---------------------|------|-----------|
| **RamBase ERP (per system DB)** | Writes events to `EVR`; defines webhooks in `WHA`; may TCP-notify on new events | Inbound to service bus |
| **External integrators** | Receive HTTP POST webhooks (JSON/XML) | Outbound from service bus |
| **Repository / NG_SYSTEM** | Registry of which RamBase systems this cloud instance owns | Config inbound |
| **RambaseServiceBus SQL** | Host registry, leader election, publish cursor, audit logs | Owned persistence |
| **On-prem Service Bus farm** | Topic fan-out between publish and subscribe | Infrastructure |
| **Ops / support** | Windows Event Log, DB logs, MS Teams alerts | Observability outbound |

This service bus is **not** a general-purpose enterprise bus — it is a **RamBase event relay** with webhook delivery as the primary consumer integration pattern.

### 3.9 Delivery semantics (what you actually get)

| Concern | Behavior |
|---------|----------|
| **Publish → broker** | **At-least-once** possible on retry; **MessageId** + duplicate detection (20 min window) suppresses broker duplicates. Cursor update is conditional (`LatestPublishedEvent + 1`) to avoid skipping ahead incorrectly. |
| **Broker → webhook** | **At-least-once**. Failed HTTP POST throws → message **not** completed → SB redelivers after lock expiry. `MaxDeliveryCount = int.MaxValue` → no DLQ by count; can retry indefinitely. |
| **Ordering** | **Per-system topic**, `SupportOrdering=true`, publisher publishes **strictly sequential** event IDs. Subscribers process **one message at a time** (`MaxConcurrentCalls=1`). |
| **Idempotency** | **Not enforced downstream.** Webhook receivers must tolerate duplicate POSTs (same `X-Rambase-EventID`). HMAC helps verify payload but does not dedupe. |
| **Skipped events** | If `ReadyFlag` stays false past tolerance, event is **skipped** (cursor advanced, never published) — **data loss by design** for slow/unready events. |
| **Wrong-filter messages** | Completed (removed from queue) **without** HTTP delivery — logged as error. |

There is **no exactly-once** end-to-end guarantee.

### 3.10 Deployment topology & scaling

**Typical deployment:**

- Multiple **Windows Server** hosts per `ServiceBusCloudId` (e.g. `PROD_1`), each running `ServiceBusService`.
- Each host row in `ServicebusHosts`: hostname, `RBSPort`, `ManagementPort`, **Priority** (lower = higher priority).
- Shared on-prem Service Bus cluster (dual endpoint in connection string: `vmrbsbus01` / `vmrbsbus02`).
- One **active** host runs all webhook subscribers; others are passive (publishers may still run — see debt #5).

**Scaling characteristics:**

| Dimension | Limit / model |
|-----------|----------------|
| RamBase systems | One topic + one publisher per system; systems added via `NGSystems` where `SB_ID` = cloud id |
| Webhooks | One SB subscription per unique URL per system; multiple event-type rules on same subscription |
| Concurrent HTTP delivery | Effectively **1 per subscription** (sequential) |
| Internal task pool | **12** concurrent tasks hardcoded (`QueuedTaskScheduler`) |
| Broker | Topics created on demand; no sharding beyond per-system topic |

**Bottlenecks:** sequential publish per system, sequential webhook POST per URL, single active subscriber host, SQL poll interval, legacy SB SDK throughput.

**Not supported:** horizontal scale-out of webhook delivery for the same URL; cross-region DR; auto-scaling.

### 3.11 Latency budget (order-of-magnitude)

| Stage | Typical delay | Config / driver |
|-------|---------------|-----------------|
| Event → publisher wake | 0–**1000 ms** | TCP notify (unreliable) or `PublisherSleepInterval` (default 1000 ms) |
| ReadyFlag wait | 0–**60000 ms** | `ReadyFlagToleranceTimeInMilliSeconds` |
| Publish → SB → subscriber | SB + lock + filter | Usually sub-second unless backlog |
| HTTP webhook POST | up to **30 s** | `HttpClient` timeout |
| Failover gap | ~**60 s** | CSM delay before passive host starts subscribers |
| New system discovery | up to **60 s** | `NGSystemCheckSleepInterval` |

**End-to-end (event in DB → webhook):** often **1–3 s** in steady state; can be **minutes** during failover, backoff, or ReadyFlag waits.

No formal SLA is encoded in code.

### 3.12 Security model

| Area | Current state |
|------|----------------|
| **Broker** | On-prem Service Bus STS (`StsEndpoint`, ports 9354/9355); connection string in config (shared secret style). |
| **SQL** | SQL auth; credentials in `App.config` + appended uid/password for system DBs. |
| **Webhook outbound** | HTTPS depends on target URL; optional **HMAC** signature (`X-Rambase-Signature`) for receiver verification. No mTLS. |
| **Webhook inbound auth** | None from service bus side — receivers must validate HMAC / event id themselves. |
| **Management plane** | Same broker topic; no separate auth layer in app code. |
| **Secrets management** | Plaintext in config files (debt). |
| **PII / payload** | Event parameters in message properties and HTTP body; logged to DB/Event Log/Teams on errors and verbose POST logging. |

Treat webhook URLs and HMAC secrets as **tenant-configured trust boundaries**.

### 3.13 Observability & troubleshooting

| Signal | Where | Notes |
|--------|-------|-------|
| Windows Event Log | Source `EventLogSource` (`RB_SB`) | Primary ops surface via `SBEventLogger.AddSysLogEntry` |
| SQL `Log` table | RambaseServiceBus DB | Structured-ish audit; includes system, event id, webhook id, log status |
| MS Teams | `MSTeamsUrl` webhook | Errors and some warnings pushed async |
| Broker peek | `WebHookSubscriber.PrintCurrentMessagesInQueue` | Debug: up to 10 peeked messages at subscriber start |

**Not present:** metrics (Prometheus/etc.), distributed tracing, health check HTTP endpoint, centralized log correlation IDs.

**Troubleshooting hooks:** `DeliveryCount` on messages logged; verbose POST request/response logging; CSM state change logs (ACTIVE/PASSIVE); NGSystem sync logs.

### 3.14 Operational procedures

| Change | How it happens |
|--------|----------------|
| **Add RamBase system to this cloud** | Insert/update row in `Repository.NGSystems` with matching `SB_ID`; within ~60 s `UpdateSystemList` creates `RambaseSystemClient`, topic, publisher; subscribers if active host. |
| **Remove system** | Remove from NGSystems → client quit on next sync. |
| **Add/update/delete webhook** | RamBase writes `WHA` + emits management event → published to management topic → `ServiceBusManagementSubscriber` → `RambaseSystemClient.WebHookChangedAsync` → subscription rules updated. |
| **Add service bus host** | Row in `ServicebusHosts` for cloud + unique `ServiceBusCloudId` on host config; CSM auto-registers `CloudHostSync`. |
| **Failover** | Automatic via CSM when higher-priority host stops heartbeating (~2 s interval) and no blocking host. |
| **Deploy new version** | Stop/start Windows Service; `#if DEBUG` changes cloud id behavior. |
| **Replay / reprocess events** | **Not supported** in-app; would require manual cursor change in `PublishedEvent` (dangerous). |

### 3.15 Failure modes

| Failure | System behavior | User impact |
|---------|-----------------|-------------|
| **Service Bus down** | Publish/subscribe throws; exponential backoff on subscribers; publisher logs errors | Events backlog in SQL; webhooks stall |
| **RambaseServiceBus SQL down** | CSM `CheckShouldRun` fails → treats as `activeHosts=1` → **this host stays passive**; cursor/logging fail | Failover broken; no new cursor; logging loss |
| **System SQL down** | Publisher/subscriber errors for that system only | That system's events/webhooks stall |
| **Webhook endpoint down / slow** | POST fails → message retried indefinitely with backoff + lock renew | Duplicate deliveries possible; queue depth grows |
| **Active host dies** | ~60 s delay; next eligible host becomes active | Webhook delivery gap |
| **Split-brain risk** | Mitigated by priority + heartbeat + blocking timestamps; not a formal consensus algorithm | Brief duplicate or missed delivery possible at boundary |
| **Passive host receives message** | Abandon + subscriber self-quits | Message returned to queue for active host |
| **Event gap in EVR sequence** | Publisher stuck retrying until manual fix or skip logic | Pipeline blocked for that system |
| **Unready event (ReadyFlag)** | Skipped after timeout | **Event never delivered** |

---

## 4. Startup sequence

1. `Service1.OnStart` / console `Program.Main` → `ServiceBusManager.Startup()`
2. `SBEventLogger` + `RambaseServiceBusConfigInfo.Initialize()` (load this host + friends from DB)
3. Shared `NamespaceManager` from `Microsoft.ServiceBus.ConnectionString`
4. Management topic client + management subscriber
5. `CloudStateMonitor.Start`
6. NGSystem change-check thread (`NGSystemCheckSleepInterval`, default 60s) → create `RambaseSystemClient`s, start publishers (+ subscribers if active)
7. `RBSTcpListener` on `ThisSB.RBSNotificationPort`

Shutdown: quit all system clients → stop CSM → quit management subscriber → stop logger.

---

## 5. Active component summaries

### Host / orchestration

| Component | Summary |
|-----------|---------|
| **Service1** | Windows Service wrapper (`ServiceName=ServiceBusManager`). Starts/stops `ServiceBusManager`; reports service status via Win32 API. |
| **Program** | Entry point: runs as service if non-interactive, else console debug host calling `Startup()`. |
| **ServiceBusManager** | Root orchestrator: shared namespace manager, task schedulers (high / default / publisher priority via `QueuedTaskScheduler`), system list sync, start/stop subscribers, TCP listener, cloud id. |
| **ServiceBusService** (library class) | Designer/legacy service bits in library project; real host is `ServiceBusService` project. |

### Cloud / HA / config

| Component | Summary |
|-----------|---------|
| **CloudStateMonitor** | DB-driven leader election and host heartbeat. Starts/stops all system subscribers when active/passive flips. |
| **RambaseServiceBusConfigInfo** | Loads this host + peer hosts for the cloud from `ServicebusHosts`. |
| **ServiceBusManagementInfo** | DTO: host id, hostname, RBS port, management port, priority. |

### Publish path

| Component | Summary |
|-----------|---------|
| **RambaseSystemClient** | Per RamBase system: ensures topic exists, owns topic client, publisher, and webhook subscriber list; handles webhook CRUD from management messages. |
| **RambaseSystemPublisher** | Long-running poller: sequential event publish, ReadyFlag wait/timeout, cursor persistence, wake-on-TCP-notification, routes mgmt vs normal topics. |
| **RBSTcpListener** | TCP listener; reads 4-byte big-endian system id; calls `Publisher.RecieveNotification()` to break poll sleep. |
| **RamBaseEvent / RamBaseEvents / Parameter** | In-memory event model; `AsDictionary()` builds broker properties (`P_` params). |

### Subscribe / deliver path

| Component | Summary |
|-----------|---------|
| **BaseSubscriber** | Abstract SB subscription lifecycle: create subscription, OnMessage handler, abandon if not leader, exponential-ish retry sleep on errors, quit. |
| **WebHookSubscriber** | Concrete subscriber: MD5 subscription name, sync SQL rules to webhook defs, map message → HTTP via `Poster`, complete/abandon. |
| **Poster** | Shared `HttpClient` POST of JSON/XML with optional HMAC signature headers. |
| **SubscriptionFilters** | Builds Azure SB `SqlFilter` expressions for webhook rules. |
| **BrokeredMessageExtensions** | Helpers: `RamBaseEventId()`, `EventType()`, `SystemID()` from properties. |
| **MessageProperties** | Property name constants. |

### Management plane

| Component | Summary |
|-----------|---------|
| **ServicebusManagementTopicClient** | Ensures management topic; sends management `BrokeredMessage`s. |
| **ServiceBusManagementSubscriber** | Subscribes to management topic; reacts to webhook create/update/delete by updating the right `RambaseSystemClient`. |
| **ManagementEventTypes** | Constants for the three webhook lifecycle event types. |

### Data access / models

| Component | Summary |
|-----------|---------|
| **RambaseDBCommunicator** | Per-system SQL: read EVR events, webhooks (`WHA`), archive helpers, max EVR for bootstrap. |
| **RambaseContext / EVR / EVR_T1** | EF models for RamBase system DB event tables. |
| **RambaseServicebusContext / PublishedEvent / Log** | EF for service-bus control DB (cursor + logging). |
| **RepositoryContext / NGSystem** | EF for system registry (which systems this cloud owns). |
| **Webhook / IgnoreUsers / IgnoreUser** | Webhook configuration model from `WHA`. |

### Cross-cutting

| Component | Summary |
|-----------|---------|
| **SBEventLogger** | Async log queue → Windows Event Log + RambaseServiceBus `Log` table + optional MS Teams webhook. |
| **Helpers** | Task-active check, XML sanitize regex, extract fields vs `P_` params, management subscription naming. |
| **TaskExtensionMethods** | Cancelable await helper for TCP accept. |
| **ExceptionToStringHelper** | Short exception formatting for logs. |
| **ConnectionException / UrlPostException** | Domain exceptions. |
| **Resources** | Designer resources (noise). |

### Not “active product” (ignore for migration)

EF package tooling/docs under `packages/EntityFramework.*` — NuGet junk captured by graphify, not runtime.

---

## 6. Technical debt (real stuff)

1. **Ancient broker SDK** — Service Bus 1.1 / WindowsAzure.ServiceBus 2.2.4; on-prem Service Bus for Windows Server (connection string has `StsEndpoint`, `RuntimePort=9354`, `ManagementPort=9355`). Cloud Azure SB / new brokers won’t drop-in.
2. **Secrets in App.config** — SQL passwords, SB endpoints, Teams webhook URL committed in config (prod + commented dev).
3. **Hardcoded task concurrency = 12** — CPU detection commented out in `InitTaskSchedulers`.
4. **Duplicate `LogSourceID` appSetting** — defined twice in config.
5. **Publishers vs leader** — subscribers gated by `IsActiveHost`; publishers started for new systems even when not active → risk of double-publish across hosts unless something else prevents it (ordering/cursor may mitigate; still fragile).
6. **`MaxDeliveryCount = int.MaxValue`** — poison messages can loop forever.
7. **Message body unused** — all data in properties; property size limits matter; filters depend on property bag.
8. **MD5 subscription names** — opaque; hard to ops/debug; collisions theoretically possible.
9. **Newtonsoft 5 / EF6 / net45-era stack** — migration is a good time to modernize runtime.
10. **TCP wake path half-broken** — comments admit 2s timeout “Not working”; falls back to poll interval (`PublisherSleepInterval` = 1s).
11. **Static/global state everywhere** — `ServiceBusManager`, CSM, logger, mgmt client — hard to test, hard to multi-instance cleanly.
12. **SQL string building for filters** — injection risk if webhook filter text isn’t trusted.
13. **Typos as API** — `RecieveNotification`, `OnMessageRecieveASync` — cosmetic but shows age.

---

## 7. Caveats / gotchas

| Gotcha | Why it matters for migration |
|--------|------------------------------|
| **Strict sequential publish** | Publisher requires `eventId == last+1`. Gaps block; ReadyFlag false retries until timeout then **skips** and advances cursor. |
| **Duplicate detection window 20 min** | MessageId = `SystemID-EventId`. New broker needs similar idempotency or you’ll double-deliver on retry. |
| **SupportOrdering = true** | Assumes ordered delivery per topic; not all brokers guarantee this. |
| **SQL filters are broker-native** | If new broker has no SQL filter, recreate filtering in consumer (group by URL still OK). |
| **Non-leader abandons** | Competing consumers / shared subscriptions need equivalent leader semantics or you’ll fight over messages. |
| **Failover delay ~60s** | Built around SB message lock timeout; retune for new broker’s ack/visibility timeout. |
| **Webhook ST=4 only** | Inactive webhooks ignored. |
| **Parameter filter rewrite** | `[Field]` in DB becomes `P_Field` in filter — preserve this mapping. |
| **Cloud id DEBUG override** | `#if DEBUG` forces `ServiceBusCloudId = DEBUG_TEST`. |
| **Management vs system topics** | Same event model; different topic based on `EventType`. Don’t merge carelessly. |
| **HttpClient TaskCanceledException** | Timeout and cancel look the same on Framework — already called out in code comments. |
| **Connection string format** | Dual endpoints (`vmrbsbus01`/`02`) — on-prem SB farm HA, not Azure cloud SAS style. |

---

## 8. Migration checklist (what to preserve vs replace)

### Preserve (business contract)

- [ ] Event property schema (`EventType`, `SystemID`, `Database`, `RamBaseEventId`, `P_*`, …)
- [ ] Sequential cursor in `PublishedEvent`
- [ ] ReadyFlag wait + skip-after-timeout behavior
- [ ] Webhook HTTP shape (JSON/XML, headers, HMAC)
- [ ] Webhook filter semantics (DB, event type, ignore users, parameter filter)
- [ ] Management events for live webhook CRUD
- [ ] Multi-host active/passive subscriber model (or redesign deliberately)
- [ ] Per-system topic isolation (or explicit replacement design)
- [ ] TCP notification as optional fast-wake (or drop and rely on poll)

### Replace (broker-specific)

- [ ] `NamespaceManager` / topic & subscription provisioning
- [ ] `TopicClient.Send` / `SubscriptionClient.OnMessageAsync`
- [ ] `BrokeredMessage` + `SqlFilter` / `RuleDescription`
- [ ] Connection string / STS ports
- [ ] Lock/abandon/complete semantics → new ack/nack model
- [ ] Duplicate detection + ordered delivery equivalents

### Nice-to-fix while migrating

- [ ] Move secrets out of config
- [ ] Dead-letter / max delivery policy
- [ ] Clarify whether passive hosts should publish
- [ ] Structured logging instead of Teams + Event Log soup
- [ ] Modern .NET + current broker client

---

## 9. Config keys that matter

| Key | Role |
|-----|------|
| `Microsoft.ServiceBus.ConnectionString` | Broker connection |
| `ServiceBusManagementTopic` | Mgmt topic name |
| `ServiceBusManagerSubscription` | Legacy/unused-ish name note; actual mgmt sub is computed |
| `ServiceBusCloudId` | Which cloud this process owns |
| `PublisherSleepInterval` | Poll sleep ms |
| `ReadyFlagToleranceTimeInMilliSeconds` | Skip unreadied events after this |
| `NGSystemCheckSleepInterval` | How often to reload systems |
| `BaseSleepTimeOnErrorInSeconds` / `NumberOfTimesToSleepBeforeExponentialIncrease` | Subscriber retry backoff |
| `UidAndPasswordForSQLRICxx` | Appended to per-system SQL connection strings |
| `RambaseServicebusConnectionstring` / `RepositoryConnectionstring` / `NGSystemConnectionstring` | Control DBs |
| `MSTeamsUrl` | Alert sink |
| `EventLogSource` | Windows event log source |

---

## 10. Solution architect Q&A

Ten questions a solution architect typically asks when assessing this implementation for migration or ops. **Coverage before this pass** noted in the third column; gaps are filled in §3.8–3.15 above.

| # | Question | Was answered? | Where |
|---|----------|---------------|-------|
| 1 | What business problem does this solve, and who consumes it? | Partial | §1, §3.8 |
| 2 | What are the integration boundaries and external dependencies? | Partial | §3.6, §3.8 |
| 3 | How is high availability implemented, and what is the failover behavior? | Yes (thin on DR) | §3.5, §3.10, §3.15 |
| 4 | What delivery guarantees exist (ordering, duplicates, missed messages)? | No | §3.9 |
| 5 | How does it scale, and what are the bottlenecks? | No | §3.10 |
| 6 | What is the end-to-end latency profile? | No | §3.11 |
| 7 | What is the security and trust model? | Partial | §3.12, §6 |
| 8 | How is the system monitored and troubleshooted in production? | Partial | §3.13 |
| 9 | How are operational changes applied (systems, webhooks, hosts)? | Partial | §3.14 |
| 10 | What happens when key components fail? | Partial | §3.15, §7 |

### Q1 — What business problem does this solve, and who consumes it?

**Answer:** RamBase ERP generates domain events in each customer system database. Integrators need near-real-time notification without polling RamBase directly. This service **bridges RamBase SQL events → message broker → HTTP webhooks**. Producers: RamBase application servers (events + optional TCP poke). Consumers: third-party systems at configured `POSTURL`s. Internal consumers: the management subscriber (webhook lifecycle only). See §3.8.

### Q2 — What are the integration boundaries and dependencies?

**Answer:** Inbound: SQL (EVR/WHA per system), TCP notification (4-byte system id), Repository NG_SYSTEM registry. Outbound: Service Bus topics, HTTP POST webhooks, Teams alerts, Windows Event Log. Hard dependency on **RambaseServiceBus SQL** for leadership and cursors; **on-prem Service Bus** for fan-out; **per-system SQL** for event source. Not an API-first product — no public REST admin API in this codebase. See §3.6, §3.8.

### Q3 — How is high availability implemented, and what is the failover behavior?

**Answer:** Active/passive **per cloud** (`ServiceBusCloudId`). `CloudStateMonitor` heartbeats into `CloudHostSync` every ~500 ms. A host becomes active only if no **higher-priority** peer has heartbeat fresher than ~2 s and no peer is in **blocking** state (starting/stopping subscribers, 60 s window). On promote: wait ~60 s, start all webhook subscribers. On demote: stop subscribers; any stray messages are abandoned. **Disaster recovery** (second datacenter, broker rebuild) is not automated — manual redeploy + DB/ broker restore. See §3.5, §3.10, §3.15.

### Q4 — What delivery guarantees exist?

**Answer:** **At-least-once** to webhooks with **per-system ordering** in the happy path. Duplicates possible on retry/failover. Events can be **silently skipped** if ReadyFlag timeout fires. Not exactly-once. Webhook clients must dedupe on `RamBaseEventId`. See §3.9.

### Q5 — How does it scale, and what are the bottlenecks?

**Answer:** Scale **out** by adding RamBase systems (each gets its own topic) and webhook subscriptions, not by parallelizing delivery to the same URL. Single active host processes subscriptions. Bottlenecks: 12-thread pool, sequential publisher per system, `MaxConcurrentCalls=1`, SQL polling, legacy broker SDK. See §3.10.

### Q6 — What is the end-to-end latency profile?

**Answer:** Steady state often 1–3 s from DB write to webhook (poll interval dominates). Worst cases: ReadyFlag wait (up to 60 s), HTTP timeout (30 s), failover (~60 s gap), NGSystem discovery (up to 60 s). No SLA in code. See §3.11.

### Q7 — What is the security and trust model?

**Answer:** Perimeter/trust-on-config: SQL and broker credentials in config, HMAC optional for webhook verification by receivers. No app-level auth for management messages. Verbose logging may capture payload data. Migration should use secret stores + TLS everywhere. See §3.12.

### Q8 — How is the system monitored and troubleshooted?

**Answer:** Windows Event Log (`RB_SB`), RambaseServiceBus `Log` table, MS Teams webhook for alerts. Async queued logger. No metrics endpoint. Use delivery count in logs, CSM ACTIVE/PASSIVE transitions, and subscriber peek at startup for queue depth hints. See §3.13.

### Q9 — How are operational changes applied?

**Answer:** Systems: NG_SYSTEM registry + periodic sync. Webhooks: RamBase DB + management topic events for live updates. Hosts: `ServicebusHosts` table + config `ServiceBusCloudId`. No built-in replay. See §3.14.

### Q10 — What happens when components fail?

**Answer:** See failure matrix §3.15. Critical nuance: **RambaseServiceBus SQL failure makes hosts prefer passive**, which stops webhook delivery cluster-wide even if broker and system SQL are fine.

---

## 11. Graphify anchors (for deeper digs)

God nodes to query next when implementing: `RambaseSystemClient`, `RambaseSystemPublisher`, `BaseSubscriber`, `ServiceBusManager`, `CloudStateMonitor`, `WebHookSubscriber`, `SBEventLogger`.

```text
graphify path "RambaseSystemPublisher" "Poster"
graphify explain "CloudStateMonitor"
graphify query "subscription filter webhook publish"
```

---

*Lazy architect verdict: this is a classic “SQL CDC-ish poller + topic fan-out + HTTP webhooks” on dead-end on-prem Service Bus. Migration success = keep the event/webhook/cursor contracts; throw away the `Microsoft.ServiceBus.*` surface. Before sign-off, walk §10 Q&A with stakeholders — especially Q4 (delivery), Q6 (latency), and Q10 (SQL-as-SPOF for leadership).*

# RamBase ServiceBus — Technical Analysis & RabbitMQ Migration Assessment

## Table of Contents

1. [Architecture Overview](#architecture-overview)
2. [Event Flow](#event-flow)
3. [Special Features](#special-features)
4. [Migration Assessment: Azure Service Bus → RabbitMQ](#migration-assessment)
5. [Complexity Summary](#complexity-summary)
6. [Key Architectural Decision](#key-architectural-decision)

---

## Architecture Overview

This is a Windows Service that acts as a bridge between **RamBase ERP systems** and **external HTTP consumers** via webhooks. It uses **Azure Service Bus topics** as the internal message bus.

```
RamBase EVR table
    ↓ (TCP push notification OR polling)
RambaseSystemPublisher   ← per RamBase system instance
    ↓ publishes BrokeredMessage
Azure Service Bus Topic  ← one topic per RamBase system (e.g. "SQLRIC")
    ↓ SQL-filtered subscriptions
WebHookSubscriber        ← one per unique webhook URL
    ↓ HTTP POST (JSON/XML)
External customer endpoint
```

### Core Components

| Component | File | Responsibility |
|---|---|---|
| Central orchestrator | `ServiceBus/ServiceBusManager.cs` | System lifecycle management |
| Per-system client | `ServiceBus/RambaseSystem/RambaseSystemClient.cs` | Events, webhooks, subscribers |
| Event poller | `ServiceBus/RambaseSystem/RambaseSystemPublisher.cs` | Polls EVR table, publishes to topic |
| Webhook delivery | `ServiceBus/RambaseSystem/Subscribers/WebHookSubscriber.cs` | Receives filtered messages, HTTP POSTs |
| HA coordinator | `ServiceBus/ServiceBusCloud/CloudStateMonitor.cs` | Active/passive host election |

### Task Queues (Priority-Based)

Three concurrent queues manage scheduling:

- **HighPrioTaskQueue** (priority 0) — TCP notifications, message reception
- **DefaultPrioTaskQueue** (priority 1) — Management operations
- **PublisherTaskQueue** (priority 2) — Event polling

### Data Access Layer

- `RambaseDBCommunicator.cs` — Interfaces with RamBase via Entity Framework
- `RambaseServicebusContext.cs` — Service Bus tracking database (PublishedEvent, Log tables)
- `RepositoryContext.cs` — Central registry of managed NGSystem entries

---

## Full System Flow Diagram

```
  ┌─────────────────────────────────────────────────────────────────────────────────┐
  │                          RAMBASE ERP SYSTEM (e.g. SQLRIC)                       │
  │                                                                                  │
  │   EVR Table (events)          WHA Table (webhooks)    EVR_T1 (parameters)       │
  └────────┬──────────────────────────────┬───────────────────────────┬─────────────┘
           │                              │                           │
           │ 1. New event written         │ 2. Webhook config read    │ 3. Key/value params
           │                              │    at startup & on change │    per event
           ▼                              ▼                           │
  ┌─────────────────┐          ┌──────────────────────┐              │
  │  RBSTcpListener │          │  RambaseDBCommunicator│◄─────────────┘
  │                 │          │                       │
  │  Listens for    │          │  - PollForNewEvents() │
  │  4-byte System  │          │  - ReadWebHooks()     │
  │  ID over TCP    │          │  - ReadParameters()   │
  └────────┬────────┘          └──────────┬────────────┘
           │                              │
           │ 4. Wakeup signal             │ 5. Batch of events (≤26)
           │    (cancels sleep)           │    with READYFLAG check
           ▼                              ▼
  ┌─────────────────────────────────────────────────────┐
  │              RambaseSystemPublisher                  │
  │                                                      │
  │  - Hybrid push (TCP) + poll (configurable interval)  │
  │  - Validates sequential EVR numbers                  │
  │  - Waits up to 20s for READYFLAG = true              │
  │  - Converts event → BrokeredMessage properties       │
  │    (EventType, Database, RegisterPid, params...)     │
  │  - MessageID = {SystemID}-{RamBaseEventId}           │
  └───────────────────────────┬─────────────────────────┘
                              │
                              │ 6. Publish BrokeredMessage
                              ▼
  ┌─────────────────────────────────────────────────────┐
  │         Azure Service Bus Topic: "SQLRIC"            │
  │                                                      │
  │  - One topic per RamBase system                      │
  │  - Duplicate detection window: 20 minutes            │
  │  - SupportOrdering = true                            │
  │  - MaxDeliveryCount = int.MaxValue                   │
  └──────┬──────────────────────────┬───────────────────┘
         │                          │
         │ 7a. Management events    │ 7b. Business events
         ▼                          ▼
  ┌─────────────────┐    ┌──────────────────────────────────────────┐
  │  Management     │    │         Subscriptions (per unique URL)    │
  │  Topic          │    │                                           │
  │                 │    │  Sub A (MD5 hash of URL-1)                │
  │  WEBHOOKCREATED │    │  ┌─────────────────────────────────────┐  │
  │  WEBHOOKUPDATED │    │  │ SQL Rule 1: EventType='ORDER'       │  │
  │  WEBHOOKDELETED │    │  │            AND Database='MYDB'      │  │
  └────────┬────────┘    │  │ SQL Rule 2: EventType='INVOICE'     │  │
           │             │  │            AND ParameterFilter...   │  │
           │             │  └─────────────────────────────────────┘  │
           │             │                                           │
           │             │  Sub B (MD5 hash of URL-2)                │
           │             │  ┌─────────────────────────────────────┐  │
           │             │  │ SQL Rule 1: EventType='ALL'         │  │
           │             │  │            AND Database='ALL'       │  │
           │             │  └─────────────────────────────────────┘  │
           │             └──────────┬──────────────────────────────┬─┘
           │                        │                              │
           │                        │ 8. Filtered messages         │
           ▼                        ▼                              ▼
  ┌─────────────────────┐  ┌──────────────────────┐  ┌──────────────────────┐
  │ServiceBusManagement │  │  WebHookSubscriber A  │  │  WebHookSubscriber B │
  │   Subscriber        │  │                       │  │                      │
  │                     │  │  AutoComplete = false  │  │  AutoComplete = false│
  │  - Routes create/   │  │  Manual ack only on   │  │  Exponential backoff │
  │    update/delete    │  │  successful HTTP POST  │  │  on failure          │
  │    to correct       │  │  Exponential backoff   │  │  Lock renewal during │
  │    RambaseSystem    │  │  Lock renewal during   │  │  wait                │
  │    Client           │  │  wait                  │  └──────────┬───────────┘
  └──────────┬──────────┘  └──────────┬─────────────┘             │
             │                        │                            │
             │ 9. Hot reconfiguration │ 10. HTTP POST              │ 10. HTTP POST
             │    (no restart needed) │     JSON or XML            │     JSON or XML
             │                        │     HMAC signed            │     HMAC signed
             ▼                        ▼                            ▼
  ┌──────────────────┐    ┌───────────────────────┐  ┌───────────────────────┐
  │ RambaseSystem    │    │  Customer Endpoint A  │  │  Customer Endpoint B  │
  │ Client           │    │  https://api.foo.com  │  │  https://api.bar.com  │
  │                  │    │  /webhook             │  │  /events              │
  │ AddRule /        │    └───────────────────────┘  └───────────────────────┘
  │ RemoveRule on    │
  │ live subscription│
  └──────────────────┘


  ┌──────────────────────────────────────────────────────────────────────────────┐
  │                    HIGH AVAILABILITY — ACTIVE/PASSIVE                         │
  │                                                                               │
  │   Host 1 (ACTIVE)   ──heartbeat──►  SQL CloudHostSync table  ◄──heartbeat──  │
  │   Host 2 (PASSIVE)                  every 500ms                              │
  │                                                                               │
  │   Every 2s: check if higher-priority host is alive                           │
  │   After 60s of silence from active host → passive takes over                 │
  └──────────────────────────────────────────────────────────────────────────────┘


  ┌──────────────────────────────────────────────────────────────────────────────┐
  │                              LOGGING                                          │
  │                                                                               │
  │   SBEventLogger (async, batched queue, flush every 25ms or 799 entries)      │
  │       ├─→  Windows Event Log  (Info / Warning / Error)                        │
  │       ├─→  SQL Log table      (structured rows, EventID, SystemName, etc.)   │
  │       └─→  Microsoft Teams    (critical alerts via webhook, optional)         │
  └──────────────────────────────────────────────────────────────────────────────┘
```

---

## Event Flow

```
RamBase System (EVR Table)
    ↓ (TCP notification on port RBSNotificationPort)
RBSTcpListener
    ↓ (notifies via RecieveNotification())
RambaseSystemPublisher.FetchEventsFromSystem()
    ↓ (polls EVR via RambaseDBCommunicator.PollForNewEvents(), batch ≤26)
RamBaseEvent
    ↓ (converted to BrokeredMessage properties)
Azure Service Bus Topic (per system — e.g., "SQLRIC")
    ├─→ Management Topic (for WebHookCreated/Updated/Deleted events)
    └─→ WebHookSubscriber (filtered subscriptions per unique webhook URL)
            ↓ (transforms to JSON or XML)
HTTP POST to Remote Webhook URL
```

### Key Flow Notes

- Events are polled in batches of up to 26 (configurable `PublisherSleepInterval`)
- **READYFLAG timeout**: Events without `READYFLAG=true` are retried for up to 20 seconds (configurable `ReadyFlagToleranceTimeInMilliSeconds`)
- **Message ID**: `{SystemID}-{RamBaseEventId}` — used for duplicate detection (20-minute window)
- Events are ordered per topic to maintain consistency
- Publisher validates sequential EVR numbers and waits or skips forward on gaps

---

## Special Features

### 1. SQL Filter-Based Routing

Instead of routing logic in code, Azure Service Bus SQL filter rules dispatch messages. Each subscription (one per unique webhook URL) can have multiple SQL rules — one per webhook configured to that URL. Filters cover:

- `EventType = 'ORDER'`
- `Database = 'MYDB'` or `'ALL'`
- `RegisterPid NOT IN (...)` — IgnoreUsers exclusion
- Arbitrary custom parameter predicates (`ParameterFilter` field in WHA table)

### 2. Webhook Lifecycle Management (Hot Reconfiguration)

A dedicated **management topic** receives `WEBHOOKCREATED / WEBHOOKUPDATED / WEBHOOKDELETED` events from RamBase. `ServiceBusManagementSubscriber.cs` processes these and adds/removes subscription rules on the fly — **no restart needed**.

### 3. Active/Passive High-Availability

`CloudStateMonitor.cs` implements an HA coordinator. Multiple service bus hosts write heartbeats to a shared SQL `CloudHostSync` table every 500ms. Only one host is ACTIVE at a time, elected by priority + heartbeat freshness. On failure, the next-priority host takes over after 60 seconds of silence.

### 4. HMAC Payload Signing

Webhook HTTP POSTs can be signed with HMAC (MD5, SHA1, SHA256 etc.) using a per-webhook secret stored in the `WHA` table. This allows customers to verify message authenticity.

### 5. TCP Push + Polling Hybrid

The publisher combines event-driven (TCP notification from RamBase) with scheduled polling (configurable sleep interval). `RBSTcpListener.cs` minimizes delivery latency without relying solely on polling as a safety net.

**Protocol:**
- RamBase sends a **4-byte big-endian System ID** over TCP when a new event is written
- Listener fires `RecieveNotification()` → cancels current sleep → triggers immediate poll
- Connection is closed after handshake (one-way notification)

### 6. Event Sequence Integrity

The publisher validates that EVR sequence numbers are contiguous. If a gap is detected it waits; if it cannot be resolved it syncs forward to the minimum available EVR in the system. This prevents replaying or skipping events.

### 7. Exponential Backoff with Jitter

`BaseSubscriber.cs` implements `2^(n-N) × baseSeconds` backoff, capped at 60 minutes, with ±10s random jitter above 120s to prevent thundering herd when a remote endpoint recovers.

Parameters (configurable):
- `BaseSleepTimeOnErrorInSeconds` = 2
- `NumberOfTimesToSleepBeforeExponentialIncrease` = 3

Message locks are renewed during wait periods to prevent Azure SB timeout.

### 8. Multi-Destination Structured Logging

`SBEventLogger.cs` writes to three destinations concurrently:

- **Windows Event Log** — Info/Warning/Error
- **SQL Database** — Structured rows (EventID, Message, SystemName, WebhookID, timestamps)
- **Microsoft Teams Webhook** — Critical alerts (configurable via `MSTeamsUrl`)

Logs are queued in memory and flushed every 25ms or 799 entries (batched to reduce DB load).

### 9. Duplicate Detection

Azure Service Bus duplicate detection (20-minute window) uses message ID `{SystemID}-{RamBaseEventId}` to prevent double-delivery during failover or restarts.

### 10. Unlimited Redelivery

`MaxDeliveryCount` is set to `int.MaxValue` on all subscriptions — messages are never dead-lettered. Combined with the backoff strategy, the system will keep retrying indefinitely until a remote endpoint recovers.

### 11. Configuration Model

All significant parameters are externalised in `App.config`:

| Key | Purpose | Example |
|---|---|---|
| `Microsoft.ServiceBus.ConnectionString` | Azure Service Bus namespace | `sb://...` |
| `ServiceBusCloudId` | Logical cloud identifier | `PROD_CLOUD` |
| `RambaseServicebusConnectionstring` | Service Bus tracking DB | SQL connection string |
| `RepositoryConnectionstring` | NGSystem registry DB | SQL connection string |
| `ServiceBusManagementTopic` | Admin event topic name | `RBS_Management` |
| `ReadyFlagToleranceTimeInMilliSeconds` | READYFLAG wait timeout | `20000` |
| `PublisherSleepInterval` | Event poll interval | `5000` |
| `BaseSleepTimeOnErrorInSeconds` | Initial retry wait | `2` |
| `NGSystemCheckSleepInterval` | System list refresh | `60000` |
| `MSTeamsUrl` | Teams alert webhook (optional) | Webhook URL |

---

## Migration Assessment

### What Maps Directly (Low Effort)

| Feature | Azure Service Bus | RabbitMQ Equivalent | Effort |
|---|---|---|---|
| Topics / Subscriptions | `TopicClient` / `SubscriptionClient` | Exchange (topic type) + Queues | Low |
| Publish event | `TopicClient.SendAsync()` | `IModel.BasicPublish()` | Low |
| Consume messages | `OnMessageAsync` pump | `BasicConsume` / `AsyncEventingBasicConsumer` | Low |
| Manual ack | `msg.CompleteAsync()` | `BasicAck` / `BasicNack` | Low |
| Connection strings / config | `App.config` keys | Same pattern | Low |
| Retry / backoff logic | `BaseSubscriber.cs` | Copy as-is | Low |
| HMAC signing | `Poster.cs` | Copy as-is | Low |
| Logging | `SBEventLogger.cs` | Copy as-is | Low |
| HA coordinator | `CloudStateMonitor.cs` + SQL | Reuse as-is (no Azure SB dependency) | Low |

### What Requires New Code (Medium Effort)

| Feature | Current Approach | RabbitMQ Replacement | Effort |
|---|---|---|---|
| **Duplicate detection** | Azure SB built-in (20-min window) | Redis `SETNX` or a DB dedup table | Medium |
| **Message ordering** | `SupportOrdering = true` on topic | Single queue per system or consistent-hash exchange | Medium |
| **Dynamic subscription rules** | `NamespaceManager.AddRuleAsync()` | Re-declare queue bindings at runtime | Medium |
| **Namespace admin** (create topics/subscriptions) | Azure SB Management SDK | RabbitMQ HTTP Management API or `IModel.ExchangeDeclare()` | Low–Medium |

### What Must Be Rebuilt (High Effort)

| Feature | Notes |
|---|---|
| **SQL filter routing** | The entire filter dispatch model (EventType, Database, ParameterFilter, IgnoreUsers) is delegated to Azure SB. In RabbitMQ this must move into application code or a plugin. This is the largest single change. |
| **Unlimited redelivery** | RabbitMQ has a DLQ concept but no built-in unlimited redelivery — needs explicit `BasicNack(requeue: true)` logic with in-process backoff (the backoff code already exists). |

---

## Complexity Summary

| Area | Estimated Effort |
|---|---|
| Basic publish/subscribe plumbing | 1–2 days |
| Filter rule evaluation engine (replaces Azure SB SQL predicates) | 3–5 days |
| Dynamic webhook add/update/delete at runtime | 2–3 days |
| HA coordinator (reuse existing SQL logic) | 0.5 days |
| Duplicate detection (add Redis or DB check) | 1–2 days |
| Retry/backoff (copy from `BaseSubscriber`) | 0.5 days |
| HMAC signing (copy from `Poster.cs`) | 0.5 days |
| Logging (copy `SBEventLogger`) | 0.5 days |
| Testing & integration | 5–7 days |
| **Total estimate** | **~2–4 weeks** |

---

## Key Architectural Decision

The **biggest design choice** when migrating to RabbitMQ is how to replace the Azure Service Bus SQL filter rules.

### Option A — Fan-out + In-Process Filtering

Publish every event to all consumer queues. Each `WebHookSubscriber` evaluates filters locally and discards non-matching messages.

- **Pro**: Simple, no RabbitMQ plugin required, supports arbitrary `ParameterFilter` predicates
- **Con**: All subscribers receive all messages; wastes bandwidth with many subscribers

### Option B — Headers Exchange Routing *(Recommended)*

Use a RabbitMQ **headers exchange** with `EventType` and `Database` as headers, binding queues with matching header arguments.

- **Pro**: Server-side routing for the common cases (EventType + Database); covers ~90% of filtering
- **Con**: Cannot evaluate arbitrary SQL `ParameterFilter` predicates server-side — needs in-process fallback for those

### Option C — MassTransit or Similar Framework

Use MassTransit (which supports RabbitMQ) as a .NET message bus abstraction providing filter/routing middleware.

- **Pro**: Reduces boilerplate, provides consumer lifecycle management
- **Con**: Adds a framework dependency; existing retry/backoff/HA logic may need to be adapted to fit the framework model

### Recommendation

**Option B** (headers exchange) for primary routing, with in-process evaluation as fallback for `ParameterFilter` webhook rules. This closely mirrors the current architecture with manageable implementation complexity and no large framework dependency.

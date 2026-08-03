---
type: process
id: OPS-002
name: Activating a System on Service Bus
status: active
---

# Activating a System on Service Bus

## Purpose

Assign a RamBase **system** (e.g. tenant SQL instance code) to a **Service Bus instance** so publishers and subscribers start for that system.

## Minimal operator action

```text
1. Locate system row in NGSystem (Repository database)
2. Set SB_ID to target ServiceBusId (bus instance id)
3. Wait for NGSystem poll cycle (~1 minute)
4. Verify auto-provisioned SB_CLIENTID / SB_CLIENTSECRET if absent
5. Confirm publisher started and events flow in test
```

Setting `SB_ID` is the **primary activation lever**—no separate bus redeploy required for standard onboarding.

## Automatic follow-up (bus host)

On NGSystem change detection:

```text
if credentials missing:
    generate ClientId + ClientSecret
    persist to NGSystem
start Publisher for system
ensure Subscriptions exist for registered webhooks
```

## Deactivation

Clearing or reassigning `SB_ID` stops bus responsibility for that system—confirm with ops before disabling production integrations.

## SQL system specifics

SQL-backed systems may require additional RIC/SQLRIC handling flags in legacy configs—validate environment-specific notes before activation.

## Verification

| Check | Expected |
|-------|----------|
| Publisher logs | System name in publish loop |
| Test event | Message on MainTopic |
| Webhook | HTTP delivery in partner logs |
| NGSystem | Credentials populated |

## Relationships

- [[activating-a-system]] --activated_on--> [[system-context-and-boundaries]]
- [[activating-a-system]] --uses--> [[data-architecture]]
- [[activating-a-system]] --requires--> [[rambase-api-integration]]
- [[activating-a-system]] --part_of--> [[deployment-architecture]]

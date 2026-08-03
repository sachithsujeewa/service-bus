---
type: design
id: DES-005
name: Deployment Event Design
status: active
---

# Deployment Event Design

## Purpose

**DeployTopic** carries **control messages** for rolling out new Service Bus software versions or coordinated configuration changes without manual per-machine surgery.

## Separation from MainTopic

Deploy messages never compete with webhook filters on MainTopic. Deploy subscribers run dedicated handlers that understand package version, role (publisher/subscriber/manager), and activation steps.

## Typical deploy sequence (conceptual)

```text
1. Publish "prepare" deploy message
2. Target components quiesce publishers (stop cursor advance)
3. Drain or timeout in-flight HTTP
4. Apply binaries / config transform
5. Publish "activate" deploy message
6. Restart publishers with new version
7. Health check via NGSystem poll + test publish
```

Exact message schema is version-specific—operators follow [[release-and-deployment]].

## Interaction with NGSystem

Deploy does not replace NGSystem for **which systems are active**—it coordinates **how code behaves** on an instance already assigned systems.

## Failure modes

| Risk | Mitigation |
|------|------------|
| Half-upgraded instance | Deploy messages must be idempotent; version handshake |
| Stuck publisher | Manual cursor reset after ops approval |
| Wrong DeployTopic config | Environment isolation in appSettings |

## Relationships

- [[deployment-event-design]] --part_of--> [[deployment-architecture]]
- [[deployment-event-design]] --deployed_via--> [[release-and-deployment]]
- [[deployment-event-design]] --publishes_to--> [[service-bus-topics-and-subscriptions]]
- [[deployment-event-design]] --compared_with--> [[event-delivery-flow]]

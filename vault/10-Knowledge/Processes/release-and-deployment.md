---
type: process
id: OPS-003
name: Release and Deployment
status: active
---

# Release and Deployment

## Purpose

Ship a new **version** of the RamBase Service Bus Windows service safely across bus hosts using DeployTopic coordination and standard release discipline.

## Release types

| Type | Scope |
|------|-------|
| Hotfix | Single defect; minimal quiesce |
| Minor | Features; full deploy sequence |
| Major | Breaking message or config changes; partner comms required |

## Standard rollout (conceptual)

```text
1. Build and sign release package
2. Deploy to test environment; run integration suite
3. Publish prepare message on DeployTopic (test)
4. Update binaries on bus host
5. Publish activate message; verify publishers/subscribers
6. Repeat per production host with maintenance window
7. Record version in ops log
```

## Pre-deploy checklist

- [ ] Connection strings unchanged or migrated
- [ ] ServiceBusId unchanged on host
- [ ] Topic names unchanged
- [ ] NGSystem poll interval acceptable post-deploy
- [ ] Rollback package available

## Post-deploy validation

- Cursor advance on publishers (no stuck loop)
- Sample webhook delivery end-to-end
- DLQ depth nominal
- Event log free of repeated exceptions

## Knowledge transfer

Major releases should update:

- [[architecture-overview]] if components change
- [[event-payload-and-formats]] if contracts change
- Operator runbooks in vault Processes

## Relationships

- [[release-and-deployment]] --deployed_via--> [[deployment-event-design]]
- [[release-and-deployment]] --part_of--> [[deployment-architecture]]
- [[release-and-deployment]] --uses--> [[service-bus-technology]]

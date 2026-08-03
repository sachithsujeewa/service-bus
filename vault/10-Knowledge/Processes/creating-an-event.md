---
type: process
id: OPS-001
name: Creating an Event
status: active
approval_required: true
---

# Creating an Event (Registration Process)

## Purpose

Register a new **event type** in RamBase so it can be archived (EVR/VET), exposed via Event Resources, subscribed by webhooks, and published by the bus Publisher.

## Prerequisites

- Business trigger defined (what ERP action fires the event)
- COF parameter schema drafted
- Event type name chosen (stable, unique, filter-safe)
- Approval from platform/event owners

## Procedure (conceptual)

```text
1. Define event type in VET dimension (name, version policy)
2. Configure COFs — parameters appearing in payload
3. Wire ERP trigger to emit EVR on business commit
4. Register Event Resource for API/history access if required
5. Document payload format for integrators
6. Smoke test: trigger in test system → EVR row → optional manual publish verify
7. Announce event type to integration partners
```

## Publisher visibility

Until EVR rows exist and publisher cursor includes the type, **no bus message** appears—webhook registration alone is insufficient.

## Webhook binding

Partners create webhooks selecting the new event type → manager/subscriber path creates subscription filters per [[routing-and-filter-design]].

## Validation checklist

- [ ] EventType string matches filter property exactly
- [ ] COFs documented in [[event-payload-and-formats]]
- [ ] Ready flag behavior defined
- [ ] No duplicate type name in VET
- [ ] Security review for sensitive COFs in outbound payloads

## Relationships

- [[creating-an-event]] --produces--> [[events-concept]]
- [[creating-an-event]] --requires--> [[event-type-registration-rules]]
- [[creating-an-event]] --uses--> [[data-architecture]]
- [[creating-an-event]] --triggers--> [[event-delivery-flow]]

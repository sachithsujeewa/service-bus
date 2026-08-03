---
type: relationship-vocabulary
name: Service Bus Relationship Vocabulary
status: active
---

# Service Bus Relationship Vocabulary

Controlled vocabulary for typed links in this vault. Prefer these relations over generic wiki links.

## Generic (from base framework)

```text
requires          Prerequisite or dependency
uses              Component or artifact consumption
includes          Containment
belongs_to        Parent category
part_of           Structural membership
depends_on        Runtime or design dependency
supports          Enables a capability
explains          Clarifies another note
validates         Confirms or tests
affects           Side effect or impact
produces          Output or artifact creation
consumes          Input consumption
derived_from      Synthesis or lineage source
compared_with     Alternative or contrast
replaces          Supersedes
deprecated_by     Successor
related_to        Weak association (use sparingly)
```

## Chapter isolation (10-Knowledge vs 11-New-System)

```text
contrasts_with           Different era or model — do not merge notes
targets_replacement_of   New chapter supersedes legacy (future state)
preserves_contract_from  Stable integrator contract lineage
feeds                    Concern register → ASUC catalog
blocks                   Decision gap blocks target work
candidate_for            Exploratory option, not approved decision
```

## Service Bus–specific

```text
publishes_to      Event or message sent to broker target
subscribes_to     Subscription binding to topic or filter
delivers_to       Final HTTP or handler destination
routes_through    Intermediate routing hop
stores_in         Persistence location (archive table concept)
triggers          Business activity causes event
registered_as     Event type registration
filtered_by       Subscription rule application
retried_by        Retry policy ownership
managed_by        Manager component responsibility
activated_on      System-to-bus assignment
deployed_via      Deployment channel
documents         Traceability to external archive stub
```

## Format

```markdown
- [[Source Note]] --relation--> [[Target Note]]
```

## Examples

```markdown
- [[event-delivery-flow]] --routes_through--> [[service-bus-topics-and-subscriptions]]
- [[webhooks-concept]] --delivers_to--> [[subscriber-and-manager-design]]
- [[push-events-business-goal]] --triggers--> [[events-concept]]
```

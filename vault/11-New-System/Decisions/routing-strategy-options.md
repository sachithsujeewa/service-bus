---
type: decision
name: Routing Strategy Options
status: needs_review
chapter: 11-New-System
---

# Routing Strategy Options (Pending)

Applies once broker is known; RabbitMQ options documented in [[rabbitmq-routing-options]].

## Decision question

Where does webhook filter evaluation live?

```text
A) Broker-native (SqlFilter today; headers exchange tomorrow)
B) Application filter engine (full WHA semantics)
C) Hybrid (broker pre-filter + app ParameterFilter)
```

## Evaluation criteria

| Criterion | Broker-native | App engine | Hybrid |
|-----------|---------------|------------|--------|
| ParameterFilter support | Poor unless SQL | Full | Full |
| Testability in CI | Hard | Easy | Medium |
| Bandwidth at scale | Best | Worst | Good |
| Ops transparency | Opaque MD5 subs | Clear logs | Medium |
| Migration effort | Broker-specific | Rebuild filters | Recommended in POC |

## Recommendation state

RabbitMQ POC doc recommends **Option B headers exchange + in-process fallback** — not program-approved.

## Relationships

- [[routing-strategy-options]] --compared_with--> [[rabbitmq-routing-options]]
- [[routing-strategy-options]] --depends_on--> [[broker-decision-pending]]
- [[routing-strategy-options]] --targets_replacement_of--> [[10-Knowledge/Design/routing-and-filter-design]]

# New System — Broker Evaluation Landscape

**Status:** decision pending — not final architecture

```text
                    ┌──────────────────────────────────────┐
                    │     RamBase webhook workload         │
                    │  (queues + topics + ordering)        │
                    └──────────────────┬───────────────────┘
                                       │
     ┌─────────────┬─────────────┬────┴────┬─────────────┬─────────────┐
     ▼             ▼             ▼         ▼             ▼             ▼
  RabbitMQ      NATS+JS      Kafka    Redpanda    Azure SB      Artemis
  (charter      (scorecard   (stream  (Kafka API  (current      (JMS)
   candidate)    #1)          log)     simpler)    baseline)

MVP prototype uses RabbitMQ for local validation — not program decision.
```

## Internal scorecard ranking (program artifact)

```text
1. NATS + JetStream  (85)
2. Redpanda          (82)
3. Kafka             (79)
4. Azure SB current  (78)
5. RabbitMQ          (76)
6. Artemis           (70)
```

## Related

- `vault/11-New-System/Migration/broker-evaluation-summary.md`
- `vault/11-New-System/Decisions/broker-decision-pending.md`

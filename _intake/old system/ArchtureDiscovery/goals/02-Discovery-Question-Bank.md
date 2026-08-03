# Discovery Question Bank

These questions classify and clarify **existing source content**. They must not be used to invent requirements. For every question, first record whether the source answers it and cite the page and section. If the source does not answer it, write **Not specified in source documentation** and add a gap. Any proposed answer belongs to a separate, explicitly approved future-work document.

## A. Why — purpose and outcomes

1. Which business processes require events or webhooks, and what happens if delivery is late, duplicated, reordered, or lost?
2. Who are the internal and external consumers, and what outcomes do they achieve?
3. What problems cannot be solved adequately with polling, synchronous APIs, workflows, or batch integration?
4. What business value and measurable outcomes justify operating this capability?
5. Which use cases are mission-critical, legally sensitive, or financially material?
6. Are deployment events, internal asynchronous events, synchronous code hooks, and customer webhooks one product or separate bounded contexts?
7. What does success look like for consumers, operators, product owners, and security?
8. What is explicitly not a goal?

## B. What — functional requirements

1. What creates an event, and at what transaction boundary is it considered committed?
2. What uniquely identifies an event, event type, tenant, producer, subject, and delivery attempt?
3. Is event delivery at-most-once, at-least-once, or effectively-once from each stakeholder’s viewpoint?
4. What ordering is guaranteed: global, per tenant, per aggregate, per subscription, or none?
5. Can consumers filter events? Which fields and operators are supported?
6. How are subscriptions created, validated, activated, paused, updated, tested, rotated, and deleted?
7. Can one endpoint own multiple subscriptions? Must events to that endpoint be ordered?
8. Are endpoint ownership and domain control verified before activation?
9. What payload modes exist: notification-only, embedded snapshot, API resource link, or change set?
10. What are the canonical JSON/media types, timestamps, identifiers, correlation fields, and error formats?
11. How are large payloads, binary data, pagination, and unavailable enrichment data handled?
12. What acknowledgement constitutes successful delivery? Which HTTP status codes are retryable?
13. What are retry schedules, maximum attempts, expiry, dead-letter, quarantine, and replay rules?
14. How does a consumer distinguish redelivery from a new event and implement idempotency?
15. Can consumers replay by time range, event ID, type, or subscription? Who may authorize it?
16. What audit history must be available for events, subscriptions, administration, and deliveries?
17. What user-facing diagnostics and delivery history are exposed?
18. How are test events and sandbox endpoints supported?
19. What happens when a referenced resource has changed or been deleted before delivery?
20. Which behaviours are contractual versus implementation details?

## C. Requirements quality and governance

1. Who owns each capability and requirement?
2. What acceptance test proves each requirement?
3. What priority method is used, and what is the minimum viable scope?
4. Which requirements conflict, and who is empowered to resolve them?
5. Which laws, contractual terms, retention rules, and data residency constraints apply?
6. What terminology is canonical, and which legacy terms must be retired?
7. How are requirements changed, reviewed, versioned, and communicated?

## D. Architecture questions

1. What is the verified current context diagram, component diagram, and deployment topology?
2. Which systems are producers, brokers, enrichers, control-plane services, delivery workers, state stores, and consumers?
3. Where are transactional boundaries, consistency boundaries, and failure boundaries?
4. How is event publication made consistent with the originating business transaction (for example, transactional outbox)?
5. What prevents missed events during crashes, deployment, cloning, or network partitions?
6. Where is durable state held, for how long, and who owns it?
7. How are tenants isolated in processing, storage, credentials, rate limits, and diagnostics?
8. How does backpressure propagate when an endpoint or enrichment API is slow?
9. How are poison messages isolated without blocking unrelated consumers?
10. What scaling unit is used: tenant, endpoint, subscription, partition, event type, or worker pool?
11. What capacity assumptions exist for event rate, subscription count, fan-out, payload size, latency, bursts, and retention?
12. What active/active or active/passive topology is required, and how is split-brain avoided?
13. What are RTO, RPO, regional failure, backup, restore, and disaster-recovery requirements?
14. Which platform/vendor constraints are still valid? What is the lifecycle status of the current technology?
15. What migration path preserves compatibility and permits rollback?
16. Which components should be consolidated, separated, replaced, or retired—and why?
17. Are deployment-data synchronization and customer webhook delivery sufficiently different to require separate platforms?

## E. Detailed design questions

1. What state machine governs an event from creation through terminal delivery?
2. What state machine governs a subscription and its credential lifecycle?
3. What concurrency model preserves required ordering while maximizing throughput?
4. What idempotency key and deduplication window are used?
5. How are message locks/leases renewed, timed out, and recovered?
6. How are retry categories separated: endpoint, authorization, throttling, platform, payload, and permanent validation failures?
7. How is exponential backoff bounded and jittered?
8. How are routing/filter rules compiled, validated, updated atomically, and rolled back?
9. How are caches invalidated and made safe after configuration changes?
10. How are event schemas registered, validated, generated, and tested?
11. What compatibility rules govern optional/required fields, renames, removals, type changes, and enum expansion?
12. What happens when producer and consumer schema versions differ?
13. What API resources are fetched for enrichment, with which identity, timeout, consistency, and fallback?
14. How are duplicate requests made harmless at the consumer contract level?
15. What data is encrypted in transit and at rest, and where are keys managed?

## F. Security, privacy, and abuse

1. How is webhook authenticity proven—signature, timestamp, nonce, certificate, or another scheme?
2. How are signing keys created, stored, rotated, revoked, and scoped?
3. How are API/service identities authenticated and authorized without long-lived embedded secrets?
4. How are SSRF, DNS rebinding, private-address targets, redirect abuse, and malicious endpoints prevented?
5. Are HTTPS and modern TLS mandatory? Is certificate validation configurable or bypassable?
6. How are replay attacks prevented?
7. Which event fields may contain personal, confidential, export-controlled, or tenant-sensitive data?
8. Can payloads leak data across tenants through filtering, enrichment, logs, or replay?
9. What redaction and retention rules apply to payloads, logs, traces, and dead letters?
10. Who can create, inspect, replay, or modify subscriptions, and how is that audited?
11. What rate limiting, quotas, anomaly detection, and abuse response are required?
12. Have all credentials exposed in historical documentation been identified, revoked, and rotated?

## G. Reliability, operations, and support

1. What availability, successful-delivery, and latency SLOs apply by service tier?
2. Which SLIs measure publish lag, queue age, delivery latency, success rate, retries, dead letters, and replay?
3. What alert thresholds are actionable, and who owns each alert?
4. What correlation ID connects business transaction, event, broker message, enrichment call, and delivery attempt?
5. Can operators answer: “Where is event X?” without database access?
6. How are endpoint outages prevented from affecting other tenants or endpoints?
7. What runbooks exist for backlog, poison messages, auth failure, platform outage, corruption, replay, and disaster recovery?
8. How are backups and restores tested? When was the last successful recovery exercise?
9. How are capacity, cost, saturation, and noisy-neighbour effects monitored?
10. What on-call, escalation, incident communication, and post-incident practices apply?
11. How are changes deployed progressively, verified, and rolled back?
12. What support information may safely be exposed to customers?

## H. Testing and verification

1. Which contract, component, integration, end-to-end, performance, resilience, security, and recovery tests are mandatory?
2. How are retries, duplicates, ordering, expired messages, clock skew, and network partitions tested?
3. Is there a consumer contract test kit and signed-payload verifier?
4. What production-like test environment and synthetic consumer are available?
5. What load profile and failure injection represent expected and worst-case conditions?
6. What evidence is required before an event type or platform release is approved?

## I. Documentation and developer experience

1. What audience-specific journeys must documentation support?
2. Can a consumer create, secure, test, observe, troubleshoot, and retire a webhook using documentation alone?
3. Are event schemas machine-readable and examples validated automatically?
4. Are diagrams current-state or target-state, dated, owned, and traceable?
5. Are generated reference docs separated from explanatory guides and operational procedures?
6. What freshness checks and review cadence prevent documentation drift?

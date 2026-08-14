# Speaker Notes — Agentic Knowledge Vault (RamBase Service Bus)

Delivery: concise technical pitch for 6–8 minutes including live demo.

Timing:
- 0:00–0:40 — Slide 1
- 0:40–2:00 — Slides 2–3
- 2:00–3:30 — Slide 4
- 3:30–6:30 — Slide 5 live demo
- 6:30–8:00 — Slide 6 + questions

## Slide 1: Agentic Knowledge Vault

I want to share a short story about moving from vibe coding to durable execution — and we proved it on RamBase Service Bus. The Agentic Knowledge Vault is the pattern; Service Bus is the working example.

## Slide 2: Why vibe coding hit a wall

We were productive with vibe coding. The wall appeared when every new task still needed a fresh dump of context. There was no durable understanding. Context lived in scattered Confluence and people’s heads. Dependencies were hidden. Work stayed partially manual. Rediscovery burned time — especially when reconstructing how RamBase events, webhooks, and the Service Bus actually worked.

## Slide 3: Knowledge alone is not enough

A knowledge base alone is content — like a book. What we needed was Skills plus Micro-workflows plus Knowledge: a matured architect that can execute. Before: chat-driven rediscovery. After: Inbox, skills, knowledge graph, then execute.

## Slide 4: How we applied it to RamBase Service Bus

The reusable framework in `base/` runs Inbox through an orchestrator and a twelve-skill pipeline into a vault and graph. For RamBase, scattered history in `_intake/` became the structured Service Bus vault, and that understanding let us execute into a runnable `prototype/` MVP. Same pattern: understand the product, then execute on it.

## Slide 5: Demo

Two live asks. First, a domain question about RamBase Service Bus from the vault — can it answer from structured knowledge. Second, ask for an improvement to the vault — can it propose a governed structured update. Watch for understanding plus action.

## Slide 6: What to take away

Three takeaways. Durable context beats re-prompting. Skills plus a knowledge graph give executable architecture literacy. The pattern transfers — RamBase Service Bus is the working proof. Happy to take questions.

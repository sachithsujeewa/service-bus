---
type: technology
id: TEC-002
name: RamBase API Integration
status: active
---

# RamBase API Integration

## Integration surfaces

The bus host interacts with RamBase through **API clients**, not direct ERP SQL (except repository/NGSystem SQL on ops side).

### Publisher API usage

- Read event streams / archives (EVR-oriented)
- Respect API pagination and rate limits
- Use per-system credentials from NGSystem

### Subscriber / Manager API usage

- Fetch supplemental webhook or event detail when message alone insufficient
- Apply webhook CRUD side effects through API where required

### Management client

Separate **management credentials** in configuration for operator-level Service Bus Manager tasks—distinct from per-system publisher credentials.

## Credential lifecycle

```text
NGSystem row without SB_CLIENTID/SECRET
    → bus detects on poll
    → auto-generate client pair
    → write back to NGSystem
    → use for API calls
```

Auto-generation reduces manual onboarding friction but requires securing NGSystem write path.

## Source system identifier

Configuration includes a **SourceSystemID** (conceptually `BASE_DATA`) anchoring API calls in the correct ERP context—should remain stable across environments unless multi-tenant model changes.

## API vs. archives

| Need | Prefer |
|------|--------|
| Push notification | Event via bus |
| Historical replay | Event Resources / archive APIs |
| Command / mutation | Standard RamBase API (not bus) |

## Error handling

API throttling and auth failures in publisher loop should **not** crash entire service—per-system isolation logs and retries.

## Relationships

- [[rambase-api-integration]] --used_by--> [[publisher-design]]
- [[rambase-api-integration]] --used_by--> [[subscriber-and-manager-design]]
- [[rambase-api-integration]] --part_of--> [[system-context-and-boundaries]]
- [[rambase-api-integration]] --requires--> [[activating-a-system]]

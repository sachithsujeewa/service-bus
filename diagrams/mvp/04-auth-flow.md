# MVP — Authentication & Authorization Flow

## API key gate (ingress)

```text
Client request
     │
     ▼
┌─────────────────┐
│ ApiKeyMiddleware│
│ reads X-Api-Key │
└────────┬────────┘
         │
    ┌────┴────┐
    ▼         ▼
 valid      invalid / missing
    │         │
    ▼         ▼
 role map   401 Unauthorized
 (source |
  partner |
  admin)
    │
    ▼
 endpoint policy
 (events vs webhooks vs dlq)
```

## Webhook outbound security

```text
Dispatcher builds POST body
     │
     ▼
HMAC-SHA256(secret, body) → header X-Signature
     │
     ▼
PartnerApp WebhookController
     │
     ├── recompute HMAC with shared secret
     ├── mismatch → 401
     └── match → process event
```

## MVP vs production target

```text
MVP (prototype)          Production target (program)
─────────────────────────────────────────────────────
Static API keys in env   OIDC / mTLS / rotating keys
Optional HMAC            Mandatory signing + replay protection
No tenant isolation      Per-integrator isolation
```

## Related

- `vault/11-New-System/MVP/mvp-tech-stack.md`

---
type: concern-analysis
name: Security Debt Register
status: active
chapter: 11-New-System
---

# Security Debt Register

## Current weaknesses

| Item | Risk | Target mitigation |
|------|------|-------------------|
| SQL/broker/Teams credentials in App.config | Leak from repo or backup | K8s secrets, vault CSI |
| Management plane — broker conn only | Unauthorized topology change if conn leaks | Separate creds, mTLS, network policy |
| HTTPS not enforced on webhook URL | MITM to partner endpoint | Document requirement; optional enforce |
| HMAC optional | Forged payloads if partner skips verify | Encourage HMAC; document algorithms |
| `ParameterFilter` in broker SQL | Injection if WHA untrusted | Validate/sandbox filters in app engine |
| Verbose POST logging | PII in logs | Redaction, log levels, retention policy |
| No threat model completed | Unknown attack surface | Program placeholder [[CONC-017]] |

## Trust boundaries (preserve)

- RamBase ERP → bus: API/SQL credentials per system
- Bus → broker: namespace credentials
- Bus → partner: URL + optional HMAC

## Modernization opportunity

Greenfield integration layer should meet **current security review** expectations without carrying plaintext debt forward.

## ASUC seeds

- Credential rotation without service restart
- Compromised webhook URL — operator disables registration
- Audit trail for management binding changes

## Relationships

- [[security-debt-register]] --validates--> [[CONC-007]], [[CONC-014]]
- [[security-debt-register]] --requires--> [[target-platform-direction]]

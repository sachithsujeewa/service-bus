# Session: 2026-09-14 — atlassian-plugin-vs-cloud-agents

Full user log: [USER-REQUEST-LOG.md](USER-REQUEST-LOG.md) (U-040).

## User requests (this session)

| ID | Category | You asked | Outcome |
|----|----------|-----------|---------|
| U-040 | troubleshooting | Why can’t I create Cloud Agents against Atlassian repos after adding the Atlassian plugin? | Plugin is Jira/Confluence MCP, not SCM. Cloud Agents need GitHub/GitLab/Bitbucket Cloud/Azure DevOps under Dashboard → Integrations. This run is bound to `github.com/sachithsujeewa/service-bus`. |

## Goal

Explain why the Atlassian marketplace plugin does not make Atlassian-hosted git (or Jira projects) appear as Cloud Agent repository targets.

## Decisions

| Decision | Rationale |
|----------|-----------|
| Treat this as a product-capability question, not an env-setup bug | Atlassian MCP is authenticated and working (`rambase.atlassian.net` with Jira + Confluence scopes). The gap is repo eligibility, not plugin auth. |
| Do not add Atlassian git to this environment | This Cloud Agent environment’s only clone target is `github.com/sachithsujeewa/service-bus`. Bitbucket (if any) must be connected separately in Cursor Integrations. |

## Artifacts created / updated

| Path | Change |
|------|--------|
| [USER-REQUEST-LOG.md](USER-REQUEST-LOG.md) | U-040 |
| [INDEX.md](INDEX.md) | Timeline row |
| [../README.md](../README.md) | Chat-history table row |

## Diagrams added/updated

- None (product explanation; no architecture diagram for the Service Bus vault)

## Open items

- [ ] If the intended git host is **Bitbucket Cloud**, connect it under Cursor Dashboard → Integrations (personal Bitbucket OAuth + workspace Cursor app).
- [ ] If the intended git host is **Bitbucket Data Center**, managed Cloud Agents are not supported (Bugbot only).
- [ ] If “repos” meant Jira/Confluence, keep using the Atlassian plugin from an agent already running on a GitHub/GitLab/Bitbucket Cloud repo.

## Related

- [Cursor Cloud Agents](https://cursor.com/docs/cloud-agent.md)
- [Bitbucket integration](https://cursor.com/docs/integrations/bitbucket.md)
- [Jira integration](https://cursor.com/docs/integrations/jira.md)

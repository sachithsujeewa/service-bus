---
title: "Service Bus Error handling"
source: "https://rambase.atlassian.net/wiki/spaces/API/pages/38010947/Service+Bus+Error+handling"
confluence_page_id: "38010947"
author: "By Tobias Nilsson (Deactivated)"
last_updated: "Feb 12, 2014"
exported: "2026-07-17"
---

# Service Bus Error handling
## Manager

| Error | Reaction |
| --- | --- |
| Could not read (and then generate) ClientID / ClientSecret for system | Ignoring system |
| Could not initiate system connection to Rambase | Publisher for system is never started |
| Could not retrieve web hooks for system | Publisher for system is never started |
|  |  |
| Cannot find WebHookId in message | Message is ignored |
| Cannot parse WebHookId from message | Message is ignored |

## Publisher

| Error | Reaction |
| --- | --- |
| No connection to Rambase | Wait 60 seconds, and then try again |
| On startup: Cannot read "Latest Published Message" | Defaulting to read all events from Rambase |
| Got Event that has ReadyFlag != true | Wait 60 seconds, and then try again |
| Got Event out of sequence (should be latestEventID + 1) | Wait 60 seconds, and then try again |
|  |  |

## Subscriber

| Error | Reaction |
| --- | --- |
| No connection to Rambase | Wait 60 seconds before retry (x5), then double the time for each retry up to 24h |
| Remote Url POST failed | Wait 60 seconds before retry (x5), then double the time for each retry up to 24h |
|  |  |
| Cannot build Api Resource string (missing parameters) | Ignore data from API Resource and complete message without extra data |
| Error from Api Resource | Ignore data from API Resource and complete message without extra data |
| Requested Format is not set | Defaulting to JSON |
| Requested Format is in incorrect format | Defaulting to JSON |
| When Building the "Content" in a message, if the different WebHooks have different output format (JSON/XML) | Wait 60 seconds before retry (x5), then double the time for each retry up to 24h |
|  |  |
|  |  |
|  |  |
|  |  |

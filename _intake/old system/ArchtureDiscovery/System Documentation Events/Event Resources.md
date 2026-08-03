---
title: "Event Resources"
source: "https://rambase.atlassian.net/wiki/spaces/API/pages/21954604/Event+Resources"
confluence_page_id: "21954604"
confluence_parent_id: "21954585"
author: "By Tobias Nilsson (Deactivated)"
last_updated: "Jul 24, 2013"
exported: "2026-07-17"
---

# Event Resources

| RES | Verb | Path | Description |
| --- | --- | --- | --- |
| 259 | GET | system/events | Get list of events that has occured. (Read from EVR archive) |
| 255 | POST | system/eventtypes | Create new Event Type (without parameters) |
| 277 | GET | system/eventtypes | Get a list of Valid Event Types |
| 254 | GET | system/eventtypes/{eventtypeid} | Get information about a specific VET |
| 256 | DELETE | system/eventtypes/{eventtypeid} | Deletes a specific VET |
| 269 | POST | system/eventtypes/{eventtypeid} | Creates a new parameter for a specific VET |
| 257 | PUT | system/eventtypes/{eventtypeid} | Updates status, description and Event Type Name for a specific VET |
| 279 | GET | system/EventTypes/{eventtypeid}/parameters | Get a list of parameters for a specific VET |
| 270 | PUT | system/eventtypes/{eventtypeid}/parameters/{parameterid} | Updates a specific parameter for a specific VET |

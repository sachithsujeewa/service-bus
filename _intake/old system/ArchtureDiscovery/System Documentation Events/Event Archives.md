---
title: "Event Archives"
source: "https://rambase.atlassian.net/wiki/spaces/API/pages/21954600/Event+Archives"
confluence_page_id: "21954600"
confluence_parent_id: "21954585"
author: "By Tobias Nilsson (Deactivated)"
last_updated: "Jul 24, 2013"
exported: "2026-07-17"
---

# Event Archives

- 1 [EVR (Event Register)](https://rambase.atlassian.net/wiki/spaces/API/pages/21954600/Event+Archives#EVR-(Event-Register))
- 2 [VET (Valid Event Types)](https://rambase.atlassian.net/wiki/spaces/API/pages/21954600/Event+Archives#VET-(Valid-Event-Types))

## EVR (Event Register)

EVR (Event Register) is an archive and an app that displays all events that occured in Rambase.

Events can only be created with the COF EvrCreate. It is not possible to create an event with the API.

All events that are created must be based on a Valid Event Type (VET). I.e a Valid Event Type must exists before an Event of that type can exist.

This is an archive where all events are registered. **This is a global Archive.**

| FieldName | DataType | TNO | DESCRIPTION |
| --- | --- | --- | --- |
| CS | CS | 00 |  |
| NO | Key#NO | 00 |  |
| ST | STRING | 00 | (Field exists but is not used, and should not be used) |
| DB | STRING | 00 |  |
| EVENTTYPE | STRING | 00 | eg. ITMSHIPPED / ARTCREATED. This value has to be registered in the VET archive to be valid. |
| REGTIME | DATETIME | 00 | Contains date and time |
| KEY | STRING | 08 | A field name. eg. DOCID |
| VALUE | STRING | 08 |  |

### Example data

| NO | DB | EVENTTYPE | REGTIME |
| --- | --- | --- | --- |
| 10001 | JHC-NO | ItmShipped | 2013.01.01 23:10:14 |
|  | KEY OrderNo ItmNo | VALUE 34343 4 |  |
| 10002 | SKA-NO | ArticleCreated | 2013.01.01 23.10:19 |
|  | KEY SKU | VALUE 10023 |  |

## VET (Valid Event Types)

VET is an archive containing the valid event types in the system.

VETs can be created and modified in the VET app.

List all valid event types with a description of each type. **This is a Global Archive.**

| FieldName | DataType | TNO | Description |
| --- | --- | --- | --- |
| CS | CS | 00 |  |
| NO | KEY#NO | 00 |  |
| ST | STRING | 00 | Status of the event type |
| EVENTTYPE | STRING | 00 | Name of the event type |
| KEY | STRING | 08 | Parameter Field Name |
| DATATYPE | STRING | 08 | Parameter Datatype |
| ISREQUIRED | BOOL | 08 | Is the parameter required? |
| DESCR | STRING | 09 | Long description of event type |

### Example Data

| EVENTTYPE | DESCR |
| --- | --- |
| ItmShipped | This event will occur when an order item is shipped to customer. It will have to parameters: OrderNo and ItmNo |
| ProductCreated | This event will occur when a new article is created. It will have one parameter: SKU |
| ProductUpdated | This event will occur when an article is updated. It will have one parameter: SKU |
| ProductClassChanged | This event will occur when the CLASS field is changed on an article. It will have three parameters: SKU, OldClass, NewClass |

### parameters

Each event type can be associated with parameters. Each parameter can either be required or optional. If it is required, the parameter key and value must be set when creating an event of the event type.

---
title: "How to create a new event / webhook in RamBase"
source: "https://rambase.atlassian.net/wiki/spaces/API/pages/2380989177/How+to+create+a+new+event+webhook+in+RamBase"
confluence_page_id: "2380989177"
confluence_parent_id: "21954585"
author: "By Roger Gullhaug"
last_updated: ""
exported: "2026-07-17"
---

# How to create a new event / webhook in RamBase

An event in RamBase could be anything that can be interesting for external systems or other parts of RamBase to listen for. Then can then choose to be notified when the event occurs. Example of events can be

- CustomerCreated
- ProductionOperationPaused
- ProductionOperationStarted
- SalesInvoiceRegistered
- +++

This is how you register a new event in RamBase:

1. Open the EVENT application in RamBase and register a new event type. First check that the event type does not exist from before.
2. When creating an event type you have to first give the event a name. See the list of example events at the top of this page for an idea about how to name the events.
3. When the event is registered you have to give it a good description, so that everyone can understand when this event occurs.
4. There is a checkbox for “Allow webhook”. This should normally be checked. It means that it will be possible for external systems to listen for this event. Unless you are creating an event that is important that should only be used internally, this should always be checked.
5. When an event like “SalesInvoiceRegistered” occurs, it does not help anyone to get notified about it, unless they also get some information about what invoice we are talking about. This is where parameters comes into play. Add a new parameter and add the data needed for someone to identify the record. In this case we should add a “SalesInvoiceId” parameter. When adding parameters, make sure you use the same name and datatype that is used in the API (name it SalesInvoiceId and NOT CINNO)
6. You can add additional parameters if needed.

When the event is registered you need to modify COS to make sure that the event is triggered.

In COS you trigger an event by calling a shared method:

```
EVT(0).KEY = "SalesInvoiceId";

EVT(0).VALUE = ARG.NO;

RamBaseEvent::Create( "SalesInvoiceRegistered":EVENTTYPE, EVT );
```

Take a look at COS/51430 if you want an example where there is more than 1 parameter.

Make sure you also deploy the Event Type (VET) before or together with your COS where the event is being triggered.

If you want some more background information about events and webhooks, take a look at:

- [Getting started (rambase.net)](https://api.rambase.net/gettingstarted/webhooks)
- [Event-driven development and code hooks - API framework - Confluence (atlassian.net)](https://rambase.atlassian.net/wiki/spaces/API/pages/1855258681)
- [Webhooks & event logs - RamBase Base - Confluence (atlassian.net)](https://rambase.atlassian.net/wiki/spaces/RBBASE/pages/155655990/Webhooks+event+logs)

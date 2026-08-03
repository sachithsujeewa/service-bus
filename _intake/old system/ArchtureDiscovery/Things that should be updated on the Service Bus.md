---
title: "Things that should be updated on the Service Bus"
source: "https://rambase.atlassian.net/wiki/spaces/API/pages/36045085/Things+that+should+be+updated+on+the+Service+Bus"
confluence_page_id: "36045085"
author: "By Tobias Nilsson (Deactivated)"
last_updated: "Jan 23, 2014"
exported: "2026-07-17"
---

# Things that should be updated on the Service Bus
# Upgrade to Service Bus 1.1

1.1 Seems backward compatible with 1.0

TODO:

- Upgrade projekt (via NuGet)
- Install Service Bus 1.1 on the machine

## New features that we could use in Service Bus 1.1

- Event driven message handling
- (Enable / disable send/receive, "This feature gives you the ability to suspend and resume sending and receiving messages to and from queues and topics. You can enable this feature by using theÂ Microsoft.ServiceBus.Messaging.EntityStatus enumeration and setting the Microsoft.ServiceBus.Messaging.QueueDescription.Status property."Â )

# Set up Publish mechanism for the Service Bus

Today, publishing works like this:

- Files are manually copied from the development machine in to the server where Service Bus is running.

This could be handled much better, by using some kind of publishing script.

# Change the Receive-loop to event-driven design

Today, the receiver process works like this:

Receive loop (that locks the process until next message is received or the process quits).

Service bus 1.1 supports Event-driven message handling.

In event-driven, a callback method is called each time a new message arrives. This can be limited to only handle one message at a time (otherwise the order of the messages can become scrambled)

I think that Event-driven message handling sounds like a more robust way to go. We need less code, and it is possible to limit it to only handle one message at a time (to guarantee correct order)

# Clean up the log management

Some of the logging on the service bus is directly to the console. Some of the logging is too much and some is too little, so this can be better organized. We need to make sure we have the relevant logs, and that they are collected into one place.

Maybe we should send an email to someone if something goes very wrong?

# Update the authentication to Rambase

How is it done today?:

**Manager**Â (Gets WHA-information from Rambase)

 Uses the SDK:
 ClientID: zFzAqbpjG0GHOW-MdjW--Q2, 
 ClientSecret: GY6wRNoP3E-8yvekidNuRg2, 
 UserName: 1167, 
 Password: !1445osyr

**Publisher**Â (GetsÂ Events from Rambase (EVR) )

Uses the SDK:
 ClientID: zFzAqbpjG0GHOW-MdjW--Q2, 
 ClientSecret: GY6wRNoP3E-8yvekidNuRg2, 
 UserName: 1167, 
 Password: !1445osyr

**Subscriber **(Gets extra Event-information from Rambase)

Uses the SDK with long lived access token
 ClientID: zFzAqbpjG0GHOW-MdjW--Q2, 
 ClientSecret: GY6wRNoP3E-8yvekidNuRg2, 
 Long lived Access Token (in SETTINGS i Rambase, Software Parameters => Category=WEB, Category = WEB, Archive = WEBHOOKS, Field = APIACCESSTOKEN )

If we could avoid long lived access tokens, this could be prettier, but how to solve it? We need to have separate logins for the different subscribers (different databases).

How to solve it?

**Alternative 1**: Resources to get username / password
Â  Â  Feels unsafe to get passwords with resources
 **Alternative 2**: Read the login information directly from database
Â  Â  Can we do this with Azure?
**Alternative 3**: Hardcode / local database on the Service Bus, with manual updates each time we need a new database access

# Major Refactor / Subscriber concept Change

Problem that exists today:

**One subscriber processes handles all WHA for a specific database. All these WHA exist in the same Subscription in the Service bus. All filters are merged so that all relevant events for the database will come to the Subscription. This means that we have our own filtering on WHA when we are handling a message (to be able to know if it is relevant for the WHA or not).**

This can be solved by separating the WHA into their own subscriptions (One Subscription per WHA in best case), where each subscription has its own unique filter, but we need to take the POST URLs into consideration. **All WHA (webhooks) with the same URL must be ordered.Â **

Possible solutions:

1. One subscriber-process per subscription / WHA
2. One subscriber-process per database (as it is today)
3. One subscriber-process for all subscriptions / WHA

I think alternative 3 is the best to go for if we implement the Event-driven message handling. This will cause us to have one active thread per subscription (wha) at any time.

**If we have an event that is sent to multiple URLs (multiple webhooks that wants the same types of events but sends to different urls), the event is sent one by one by HTTP POST to the different URLs. If one of these POSTs fail, the message will be returned to the service bus, and will be processed again. This causes the message to be sent again to the same URLs even if they succeded the first time. It only takes one of the HTTP POSTs to fail for this to happen.**

When there are multiple URLs, we should separate them into separate subscriptions (to make message handling easier). It requires more subscriptions on the service bus, and possible more memory.

Pros:

- Reduces the risk that messages are sent multiple times
- Better utilization of the Service Bus
- Simpler code

Cons:

- Requires more subscriptions (and therefore more processes, or more memory usage if we use Event-driven message handling)
- All in one process => less scalable?
- More subscriptions => More memory usage? (more overhead)

# Move to Azure

Pros

- Updates to Service Bus comes earlier to Azure than to Service Bus for Windows Server
- Code seems to be exactly the same. Only the connection to the Service Bus is a bit different.
- ??

Cons:

- Cost
- ??

# Move data to SQL

We store some data in txt files on the Service Bus Server. These may be move to an SQL database

**Publisher:Â **

TXT file: Latest Published Message is saved to txt-file. This is one (1) value, maybe a database is overkill for this?

**Manager:**

TXT file: List of processes that are active right now (Subscribers / Publisher / Manager)

This might not be needed if we change to event-driven message handling AND do everything in the same process.

# Add 'ready flag' to Events

See mail history with Roald / Roger / Ruben. The flag is needed to make sure that all parameters are written before the event can be read.

---
title: "Deployment through Service Bus"
source: "https://rambase.atlassian.net/wiki/spaces/API/pages/38011207/Deployment+through+Service+Bus"
confluence_page_id: "38011207"
author: "By Tobias Nilsson (Deactivated)"
last_updated: "Feb 21, 2014"
exported: "2026-07-17"
---

# Deployment through Service Bus
# Goal:

Produce a design for deployment of updated data in systems, using the Service Bus

# How To Get Deploy Events / Messages into the Service bus

## Alternative 1: Create RamBaseEvent

On an update, a RamBaseEvent "DeployEvent" is created.

This seems like more stable, since it will create an event like any other event, and then the service bus will pick it up.

Cons:

## Alternative 2: Create BrokeredMessage

On an update, a direct connection to the Service Bus is used to create an event in the topic.

Cons:

- This requires the Service Bus to be online to be able to update
- May create inconsistency if the update is performed on the system, then power outage, the message will never get to the Service Bus

# Subscriptions

# We need one deploy-subscription per system that will receive all deployment-events that occurs. This subscription will get all events of type "DeployEvent", no matter what database or system, because we want to be able to get all possible changes from all other systems.

# Cache

If event messages **does not** contain the SQLXML, we need to have a good caching system. If the event messages **does** contain the SQLXML, we do not need to think about cache (this section can be skipped).

Caching of data from the changed system is important, so that we do not end up with 1000 requests (one (1) per system) for each change. Caching must be done in a smart way, because on a deploy, we will have multiple subscribers that try to access the data.

# Subscriber: 2 alternatives in Service Bus

## Alternative 1: Expand on the current Service Bus Setup

- This will require an integration server
- This requires that we setup WHA for each system, so that the Service Bus knows where to perform the POST (could probably be automated)

**Flow**

1. Trigger Event Deploy
2. WebHookSubscriber gets event
3. WebHookSubscriber requests the necessary data from Rambase
4. WebHookSubscriber POSTs data to Remote Url Service
5. Remote Url Service receives data
6. Remote Url Service is responsible for Deploy

## Alternative 2: Special Case of Subscriber

- This requires a special written "DeploySubscriber" (one per system) that performs the deployment directly when the message is arrived, instead of POSTing it to an external web server.

No need to specify webhooks in WHA. Deploy-Subscriptions with a standard filter can be automatically setup for each system

**Flow**

1. Trigger Event Deploy
2. DeploySubscriber gets event
3. if(it the SystemID of the Subscriber match with the SystemID of the message)
  1. ignore the message
4. DeploySubscriber requests the necessary data from Rambase
5. DeploySubscriber performs the deploy to the system by performing SQL query

One "DeploySubscriber" per system
 DeploySubscriber know about connectionstring to SQL Server to which it can perform the update

# Deploy Code

When the DeploySubscriber (or Deploy Web Service) has gotten the message that contains the XML, we will utilizeÂ XML => SQL. Jens has the code for this.

# Three possible ways to do this

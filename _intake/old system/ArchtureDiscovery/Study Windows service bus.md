---
title: "Study Windows service bus"
source: "https://rambase.atlassian.net/wiki/spaces/API/pages/16089135/Study+Windows+service+bus"
confluence_page_id: "16089135"
author: "By Jon Terje Aksland (Deactivated)"
last_updated: "Apr 24, 2013"
exported: "2026-07-17"
---

# Study Windows service bus
Service Bus 1.0 is available today from the download center and is a free piece of technology for use with a properly licensed version of Windows Server.

What is possible with Service Bus for Windows Server:

- Service Bus Queues
  - "load leveling by allowing the message receiver to process messages at its own pace."
- Service Bus Topics
  - "publish-subscribe capabilities"
- **? HTTP POST ?**

Getting started: [http://msdn.microsoft.com/en-us/library/jj193021](http://msdn.microsoft.com/en-us/library/jj193021)

Cost: Free when run under a valid Windows Server licence

The service bus contains topics. Each topic consists of subscriptions.

When a message is sent to the Service Bus, it is sent to a topic. It is possible to set subscription filters so that a subscription only receives messages of a certain type.

Subscribers subscribe to a topic subscription.

Subscribers have a persistent connection to the service bus. It is however possible for the subscriber to connect and disconnect at any time. The subscriber polls for messages in the topic subscription. Messages that are sent to a subscription stays there until it is read.

### Problems with Windows Service Bus:

**Problem? 1:** The subscriber polls for data (calling a receiveMessage method) and also locks the subscription as long as no message is received. This also leads to the next problem:

**Problem? 2:** One subscription can only handle one subscriber at a time. If multiple subscribers subscribe to the same subscription, only one of the subscribers will receive the message. In other words one message in a subscription can only be read once. This can be useful for load balancing.

Using the service bus internally only, and adding the REST communication on the side of this, the first problem should not be relevant. Considering the other problem, we may handle this by having one subscriber per "customer POST address".

It is possible to create this system, if we create one subscription / subscriber for each POST address. What needs to be done to make it work is to write a "subscriber layer" that handles the polling from the service bus and POST the messages to the appropriate address.

A message is removed from the service bus when the method "Complete" is run on the message. If an error occurs during the POST, we do not run the "Complete" method on that message. This message is at a later stage sent to the subscriber automatically (every 1 minute seems to be the default) from the service bus as long as the Complete method has not been run.

Problem 3: A message that is not "completed" seems to disappear after ~10 minutes (or after around 10 sends to the subscriber). Â Set MaxDeliveryCount to > 10 when creating SubscriptionDescription to solve this.

Problem 4: When a message has entered the subscriber, the HTTP POST is run, but if this takes too long time, the lock of the message will be lost. This makes it not possible to 'Complete' the message, which causes the message to stay in the service bus subscription. This also causes the message to be delivered again to the subscriber.

### REST Communication

To perform a HTTP POST to the end pointÂ should not be a problem using C# libraries.

"Windows Azure Service Bus eliminates one large complexity: you do not have to manage the challenges of push notifications. Instead, you can use aÂ **Service Bus Notification Hub**. The feature is only available in the Service Bus as a preview feature in January 2013. It is expected to transition to General Availability (GA) in midyear 2013." ([http://msdn.microsoft.com/en-us/library/jj927170.aspx](http://msdn.microsoft.com/en-us/library/jj927170.aspx))

In my understanding, it is possible to make use of the REST API in Service Bus for Server even though it it says 
Windows Azure Service Bus (see below)

Service Bus REST API Reference: "The Windows Azure Service Bus offers a REST API for runtime and management (AtomPub) 
operations. By using REST, you can write applications in any language that supports HTTP requests, without the need 
for a client SDK. The Service Bus is a multi-protocol service. You can send and receive messages to or from the 
service using REST or .NET managed API, mixing and matching clients using different protocols in a given scenario. 
For example, you can send a message to a queue using one protocol and consume it using a different protocol." 
[http://msdn.microsoft.com/en-us/library/windowsazure/hh780717.aspx](http://msdn.microsoft.com/en-us/library/windowsazure/hh780717.aspx)

### Problem 1:

From what I can understand, to enable push, Notification Hubs can be used, however it is only possible to use this 
for Windows Store apps and iOS apps at the moment.
"As of April 2013, Notification Hubs are able to send push notifications to Windows Store apps and iOS apps, from 
.NET backends and Windows Azure Mobile Services. Support for Android and Windows Phone as well as additional back-end 
technologies will be added soon." [http://msdn.microsoft.com/en-us/library/windowsazure/jj927170.aspx](http://msdn.microsoft.com/en-us/library/windowsazure/jj927170.aspx)

"solicited push"
[http://brentdacodemonkey.wordpress.com/2012/07/18/service-bus-and-pushing-notifications/](http://brentdacodemonkey.wordpress.com/2012/07/18/service-bus-and-pushing-notifications/)

The following is an overview of Windows Azure ServiceBus messagingÂ [http://imgrouponline.wordpress.com/2012/05/03/overview-of-service-bus-messaging/](http://imgrouponline.wordpress.com/2012/05/03/overview-of-service-bus-messaging/)

[http://imgrouponline.wordpress.com/2012/07/03/windows-azure-servicebus-topic-and-subscription-part-1/](http://imgrouponline.wordpress.com/2012/07/03/windows-azure-servicebus-topic-and-subscription-part-1/)

[http://imgrouponline.wordpress.com/2012/07/03/windows-azure-servicebus-topic-and-subscription-part-2/](http://imgrouponline.wordpress.com/2012/07/03/windows-azure-servicebus-topic-and-subscription-part-2/)

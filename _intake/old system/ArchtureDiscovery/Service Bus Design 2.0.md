---
title: "Service Bus Design 2.0"
source: "https://rambase.atlassian.net/wiki/spaces/API/pages/36045144/Service+Bus+Design+2.0"
confluence_page_id: "36045144"
author: "By Tobias Nilsson (Deactivated)"
last_updated: "Jan 21, 2015"
exported: "2026-07-17"
---

# Service Bus Design 2.0
# Service Bus Server

Event flow:

EVR => Publisher => Topic => Subscription => Subscriber => Target Url

## App Config

`Â Â Â `

`<appSettings>Â Â Â Â <!--Â ServiceÂ BusÂ specificÂ appÂ setingsÂ forÂ messagingÂ connectionsÂ (connectionÂ stringÂ toÂ ServiceÂ Bus)-->Â Â Â Â <addÂ key="Microsoft.ServiceBus.ConnectionString"Â value="Endpoint=sb://jhc-servicebus/ServiceBusDefaultNamespace;StsEndpoint=https://jhc-servicebus:9355/ServiceBusDefaultNamespace;RuntimePort=9354;ManagementPort=9355"Â />Â Â Â Â Â Â Â Â <!--Â IDÂ ofÂ thisÂ serviceÂ bus.Â ThisÂ MustÂ beÂ changedÂ whenÂ startingÂ upÂ anÂ additionalÂ serviceÂ busÂ -->Â Â Â Â <addÂ key="ServiceBusId"Â value="1"Â />Â Â Â Â Â Â Â Â Â Â Â Â Â Â Â Â Â Â <!--Â TopicÂ inÂ ServiceÂ BusÂ (WebÂ Hooks).Â ShouldÂ notÂ beÂ changedÂ -->Â Â Â Â <addÂ key="ServiceBusTopic"Â value="MainTopic"Â />Â Â Â Â Â Â Â Â <!--Â TopicÂ toÂ useÂ forÂ DeployÂ messages.Â ShouldÂ notÂ beÂ changedÂ -->Â Â Â Â <addÂ key="DeployTopic"Â value="DeployTopic"/>Â Â Â Â Â Â Â Â <!--Â SubscriptionÂ nameÂ ofÂ theÂ ServiceÂ BusÂ Manager.Â ShouldÂ notÂ beÂ changed.Â -->Â Â Â Â <addÂ key="ServiceBusManagerSubscription"Â value="ManagerSubscription"Â />Â Â Â Â Â Â Â Â <!--Â RetryÂ settingsÂ -->Â Â Â Â <addÂ key="BaseSleepTimeOnErrorInSeconds"Â value="60"/>Â Â Â Â <addÂ key="NumberOfTimesToSleepBeforeExponentialIncrease"Â value="5"Â />Â Â Â Â <addÂ key="ReadyFlagToleranceTimeInMilliSeconds"Â value="10000"/>Â Â Â Â <!--Â NumberÂ ofÂ millisecondsÂ toÂ sleepÂ betweenÂ eachÂ pollingÂ forÂ newÂ eventsÂ -->Â Â Â Â <addÂ key="PublisherSleepInterval"Â value="1000"Â />Â Â Â Â Â Â Â Â <!--Â NumberÂ ofÂ millisecondsÂ toÂ sleepÂ betweenÂ eachÂ checkÂ inÂ NGSystemÂ -->Â Â Â Â <addÂ key="NGSystemCheckSleepInterval"Â value="60000"Â />Â Â Â Â <!--Â RamBaseÂ ApiÂ ClientÂ informationÂ forÂ ServiceÂ BusÂ ManagerÂ -->Â Â Â Â <addÂ key="ServiceBusManagementClientID"Â value="zFzAqbpjG0GHOW-MdjW--Q2"Â />Â Â Â Â <addÂ key="ServiceBusManagementClientSecret"Â value="GY6wRNoP3E-8yvekidNuRg2"Â />Â Â Â Â <addÂ key="ServiceBusManagementUserName"Â value="1167"Â />Â Â Â Â <addÂ key="ServiceBusManagementPassword"Â value="!1445osyr"Â />Â Â Â Â <!--Â SourceÂ SystemÂ (BASE_DATA).Â ShouldÂ notÂ beÂ changedÂ -->Â Â Â Â <addÂ key="SourceSystemID"Â value="BASE_DATA"/>Â Â Â Â <!--Â NameÂ ofÂ EventÂ ViewerÂ logÂ -->Â Â Â Â <addÂ key="EventLogSource"Â value="RambaseÂ ServiceÂ Bus"Â />Â Â Â Â Â Â Â Â <!--Â SetÂ toÂ trueÂ toÂ handleÂ RICÂ (ThisÂ shouldÂ beÂ obsoleteÂ byÂ now,Â sinceÂ RICÂ existsÂ inÂ NGSystemÂ table)Â -->Â Â Â Â <addÂ key="HandleRIC"Â value="false"/>Â Â Â Â <!--Â CredentialsÂ forÂ SQLRICÂ -->Â Â Â Â <addÂ key="UidAndPasswordForSQLRICxx"Â value="uid=ServiceBus;password=Serv1ceBus88;"/>Â Â Â Â <addÂ key="ClientSettingsProvider.ServiceUri"Â value=""Â />Â Â Â Â Â Â </appSettings>`

`<connectionStrings>Â Â Â Â <addÂ name="RepositoryEntities"Â connectionString="metadata=res://*/Repository.csdl|res://*/Repository.ssdl|res://*/Repository.msl;provider=System.Data.SqlClient;providerÂ connectionÂ string=&quot;dataÂ source=rbsqlric.rambase.local\sqlric;initialÂ catalog=Repository;userÂ id=ServiceBus;password=Serv1ceBus88;MultipleActiveResultSets=True;App=EntityFramework&quot;"Â providerName="System.Data.EntityClient"Â />Â Â Â Â <addÂ name="RambaseServiceBusEntities"Â connectionString="metadata=res://*/PublishedEvent.csdl|res://*/PublishedEvent.ssdl|res://*/PublishedEvent.msl;provider=System.Data.SqlClient;providerÂ connectionÂ string=&quot;dataÂ source=jhcvmsql01.netsentral.no;initialÂ catalog=RambaseServiceBus;persistÂ securityÂ info=True;userÂ id=rbservicebus;password=DxjG2W6x;MultipleActiveResultSets=True;App=EntityFramework&quot;"Â providerName="System.Data.EntityClient"Â />Â Â </connectionStrings>`

`Â `

`Â `

## Service Bus Subscription

A subscriptionÂ on the Service Bus is set up with filters. All messages that are sent to the topic will be autmatically splitted / filtered by the rules of these filters. Â A subscription will consist of all WebHooks (for a specific system) that has the same target Url. From a webhook eventtype we generate part of the subscription filter. For example, a WebHook with EventType "HldCreated" will produce the filter EventtType="HldCreated". This filter is added to the Subscription and the Service Bus will make sure that the appropriate messages are delivered to the subscription.

## ServiceBusService

Each instance of the ServiceBusService (at the time of writing, only one exists: 172.21.13.121, ID:1) has a unique identifier. Each instance can handle multiple systems. This mapping is setup in theÂ NGSystem tableÂ in the Repository database on the serverÂ 192.168.10.239\STDINST.

The application has a list of Systems (example: RIC, SQLRIC). Each of these systems has its own Api Server Credentials, which are mapped in the NGSystem table in the Repository database on the serverÂ 192.168.10.239\STDINST. The credentials are used by the Publisher and Subscriber to get data from Rambase. Each system has one (1) publisher, which gets events from Rambase and publishes them into the Service Bus. Each system has one Subscriber per Subscription (grouped by Remote Url).

### Subscriber

Handling of events / messages in the Subscriber is event-driven. This means that a method (OnMessageReceive) will be called for each new message that comes from the service bus subscription. If the message event type is of one of the special types (WebHookCreated, WebHookUpdated, WebHookDeleted), it will be handled by the manager, else it will be handled as a normal message by the Subscriber. The Subscriber (as well as the Manager) will have access to the Client Database, where login credentials (such as ClientID and ClientSecret) for the different systems is stored.

## Setup of a system in a service bus

You have a system. You want it to be handled by the Service bus. Here is how you solve it:

In NGSystemÂ tableÂ in the Repository database on the serverÂ 192.168.10.239\STDINST, you find your system, then you set SB_ID to the service bus id that should handle the system (at the time of writing, the only service bus is running on 172.21.13.121, and has the **SB_ID = 1**). This is all you need to do. The following is what happens:

### How it works

The NGSystem table is polled by the Service Bus Servcie each minute to check for changes. When a change is detected, the service bus will check if ClientId and ClientSecret is set in the same Table. If they are set, the ClientId and ClientSecret are used for that system to communicate with the API. Otherwise a ClientID and ClientSecret is automatically generated and the generated ClientID and ClientSecret is written to NGSystem. The ClientID and ClientSecret are then used to communicate with the Rambase System through the API.

Table**Â NGSystemÂ **

| Table Field | Datatype | Description |
| --- | --- | --- |
| Name | String | Unique identifier for the system. (example: RIC) |
| SB_ID | String | Service Bus ID |
| SB_CLIENTID | String |  |
| SB_CLIENTSECRET | String |  |
| SB_CLONESYNCSTATUS | int | Set to 1 to start sychronization of old messages. When this is set, on startup of Service Bus Service, or on startup of system management, will start to synchronize messages from EventId specified in field below. |
| SB_DEPLOYSYNCHRONIZATIONSTARTEVENTID | bigint | Event to start "catchup" synchronization from |

The messages are stored in the internal databases of the Service bus. These are **SbGatewayDatabase**, **SbManagementDB**, **SbMessageContainer01**, **SbMessageContainer02**, **SbMessageContainer03 **and exist on the serverÂ JHCVMSQL01.netsentral.no. In addition to these databases, we have one database **RamBaseServiceBus** on the same server

TableÂ **PublishedEvent (**RambaseServiceBusÂ @ [JHCVMSQL01.netsentral.no](http://jhcvmsql01.netsentral.no/)**)**

Holds the latest value from the EVR archive for each system that has been published to the service bus. So when the service bus starts up (or starts monitoring a system), it will start to get events with EventId = (LatestPublishedEvent + 1) and higher.

| Table Field | Datatype | Description |
| --- | --- | --- |
| SystemID | varchar(100) | System identifier (RIC, BASE_DATA, JHCDEVSYS ...) |
| ServiceBusId | varchar(50) | Identifier of the Service bus (At the time of this writing, we only have one server with ServiceBusId="1" |
| LatestPublishedEvent | bigint | Latest event that has been returned from the resource GET system/rambase-events and published to the service bus. It is important to note that all subscribers may not have processed the message yet |

# Data Structure

## WebHookSubscriber Class

| Type | Name | Description |
| --- | --- | --- |
| method | OnMessageReceive | Code for handling a message |
| property | Url | Url to post to |
| property | List of WebHooks | List of WebHook related to the Subscription |

**OnMessageReceived**

1. For each WebHook that match the EventType of the message
  1. If Api resource is set for the webhook.
    1. Get additional data from Api resource
2. Combine the additional data into one list of data (XML / JSON).
3. Store the additional data in "Content" property of the message
4. Generate output (XML / JSON)
5. Send Message to [URL]

## ManagerSubscriber Class

| Type | Name | Description |
| --- | --- | --- |
| method | OnManagementMessageReceive | Handling of management message (for example: WebHookCreated) |
| property | Subscriptions | List of subscription objects |

**OnManagementMessageReceive**

1. switch(EventType)
  1. case WebHookCreated:
    1. Read System/DB from Message
    2. if there exists a ClientID / ClientSecret / Username / Password
      1. use the already existing clientID
    3. else Create ClientID / ClientSecret / Username / Password
      1. POST system/api/clients
      2. Save the generated Credentials to NGSystem
      3. Use the newly created ClientID
    4. Get information about Web Hook (from API)
      1. Get Filter of web hook
      2. Add new filter to Subscription with the Url (using WebHookID as identifier for filter)
  2. case WebHookUpdated:
    1. Get information about Web Hook (from API)
      1. Get Filter of web hook
      2. Remove old filter from Subscription (Using WebHookID as identifier for filter)
      3. Add new filter to Subscription with the Url (using WebHookID as identifier for filter)
  3. case WebHookDeleted:
    1. Delete WebHook from Subscriber
    2. Delete the Filter from the Service bus subscription

## Publisher Class

| Type | Name | Description |
| --- | --- | --- |
| method | PollForNewEvents | Code to handle new events |
|  |  |  |

**PollForNewEvents**

1. for each System
  1. if system is base system (BASE_DATA)
    1. GetÂ system/rambase-events/globalsynchronization using the system RambaseCommunicator
  2. else
    1. Get system/events using the system RambaseCommunicator
  3. for each event
    1. Create BrokeredMessage
    2. Store Event Parameters in Message
    3. Add System / DB to Message
    4. Send message to Service Bus Topic

## DeploySubscriber Class

Only subscribes to messages with EventType "CommonArchiveUpdated", and only events with matching archive

| Type | Name | Description |
| --- | --- | --- |
| method | OnMessageReceive | Code for handling a message |
| property | Archive | RamBase archive that this deploy subscriber handles |

**OnMessageReceived**

1. If the message system is the same as this system
  1. ignore message
2. Get deploy query from message
3. Perform sql deploy

# Rambase API

EVR - All local events for a system.

Events that occur on a system are added as an entry to the EVR archive of the system / database. The EVR archive is local for each system / database.

**Archive EVR**

| Table Field | Datatype |  |
| --- | --- | --- |
| NO |  |  |
| READYFLAG |  | Set to True when the event is written completely to the archive |
| DB |  |  |
| EVENTTYPE |  | The event type that the Service Bus is using for filtering to the subscriptions |
| REGTIME |  |  |

**Table EVRParameters** - Same as before, does not need to be updated

**WHA** - Unique list for each system - same structure as current

# Limitations

If you want message order guarantees, you must set up the WHA to the same target URL. All messages sent to a target URL have a guaranteed chronological order. Two events that happened after one another are not guaranteed to arrive in the same order if they are sent to different target URLs.

# Flows

### Event Created Flow

Publisher is polling each second each system for new events

1. An event is created in a local system.
2. The event will be picked up by the Publisher
3. Publisher sends the event to the Service Bus Topic
4. Subscriber receives new Message
5. Subscriber checks Message Properties to see if it needs additional data.
6. If additional data is needed, a request to the API Server is made.
7. Subscriber updates the message with additional data.
8. Subscriber sends the message to the target URL.

### WHA Created (Similar for WHAUpdated and WHADeleted)

1. Event WebHookCreated is created in local system (EVR)
2. The Event will be picked up by the Publisher
3. Publisher adds system / DB to Message
4. Publisher Sends message to Service Bus
5. ManagerÂ **OnManagementMessageReceive** gets the Message
6. Manager gets system from Event
7. If system (ClientID) does not exist, it will be created
8. Manager gets information about WebHook (using ClientID)
9. Subscription is created if it does not already exist (check system + url)
10. WebHook Filter is added to the Subscription

## Child pages

- [[Service Bus Design 2.0/Releasing new version of Service Bus]]
- [[Service Bus Design 2.0/Service Bus- Activating or Deactivating an SQL system]]

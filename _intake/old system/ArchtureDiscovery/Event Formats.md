---
title: "Event Formats"
source: "https://rambase.atlassian.net/wiki/spaces/API/pages/21430297/Event+Formats"
confluence_page_id: "21430297"
author: "By Tobias Nilsson (Deactivated)"
last_updated: "Jul 17, 2013"
exported: "2026-07-17"
---

# Event Formats
- 1 [Event in Rambase](./Event Formats.md#Event-in-Rambase)
- 2 [Event internally in Service Bus (from Publisher -> Service Bus -> Subscriber)](./Event Formats.md#Event-internally-in-Service-Bus-(from-Publisher--%3E-Service-Bus--%3E-Subscriber))
- 3 [Event Out from Service Bus](./Event Formats.md#Event-Out-from-Service-Bus)
  - 3.1 [Example of (JSON)](./Event Formats.md#Example-of-(JSON))
  - 3.2 [Event out from Service Bus (XML)](./Event Formats.md#Event-out-from-Service-Bus-(XML))

# Event in Rambase

The rambase archive EVR contains the triggered events. Note Table 1 containing parameters for the event.

| FieldName | DataType | TNO | DESCRIPTION |
| --- | --- | --- | --- |
| CS | CS | 00 |  |
| NO | Key#NO | 00 |  |
| DB | STRING | 00 |  |
| EVENTTYPE | STRING | 00 | eg. ITMSHIPPED / ARTCREATED. This value has to be registered in the VET archive to be valid. |
| REGTIME | DATETIME | 00 | Contains date and time |
| KEY | STRING | 08 | A field name. eg. DOCID |
| VALUE | STRING | 08 |  |

# Event internally in Service Bus (from Publisher -> Service Bus -> Subscriber)

Each Event is read from the EVR into the Publisher. In the Publisher, the event is converted into a BrokeredMessage. The BrokeredMessage contains a property with the name Properties, which contains the contents of the event including the parameters.

Easy way: Use the Events class. This class has a constructor that takes an entire IDictionary from a BrokeredMessage as parameter. This will convert the contents of the IDictionary into an Event Object. Example usage:

> Event myEvent = new Event(brokeredMessage.Properties);
> 
> myEvent.EventID now contains the same information as brokeredMessage.Properties["EventID"]

Details: The following are examples of how the event information can be retrieved from a BrokeredMessage. (Note that the usage of Events.cs is preferred instead of accessing the properties directly, the examples below are just for reference):

| Rambase FieldName | BrokeredMessage equivalent | Comment |
| --- | --- | --- |
| NO | brokeredMessage.Properties["EventID"] |  |
| EVENTTYPE | brokeredMessage.Properties["EventType"] |  |
| DB | brokeredMessage.Properties["Database"] |  |
| REGTIME | brokeredMessage.Properties["RegisterTime"] |  |
|  | brokeredMessage.Properties["Parameters"] | The parameters are stored as an XElement string which needs to be converted into a Dictionary. This can be done by calling DictionaryMethods.xelementToDictionary |

Parameters (Rambase KEY / VALUE) is a special case. They can be retrieved by accessingÂ brokeredMessage.Properties["Parameters"] which will result in a string which needs to be converted in to a Dictionary. This is done automatically when creating a new Event object, if passing the "brokeredMessage.Properties" as argument.

# Event Out from Service Bus

The events are converted to either JSON or XML before POSTing to the remote url. Before the POST is performed the API resource is requested from the Rambase API and the result is stored under "Content" in the outputted JSON/XML. If the Api Resource is not set for the web hook, "Content" does not exist.

In the examples below, the API resource "sandbox/auctions/lots/{LotId}" has been used.

## Example of (JSON)

{

"EventID": 100311,

"EventType": "BidPlaced",

"Database": "FMH-NO",

"RegisterTime": "2013.07.16 15:50:56",

"Parameters:": [

{

"ParameterName": "LotId",

"ParameterValue": "792934"

},

{

"ParameterName": "BidPrice",

"ParameterValue": "2300"

},

{

"ParameterName": "Account",

"ParameterValue": "FMH-NO:CUS/272663"

},

{

"ParameterName": "IpAddress",

"ParameterValue": "172.22.32.213"

}

],

"Content":Â {

"auctionItem": {

"auctionItemId": 792934,

"currentPrice": 2300,

"imageId": "FIL/100506.100",

"timeleft": "14d 17h 9m",

"title": "",

"auctionName": "Test auksjon"

}

}

}

## Event out from Service Bus (XML)

<Event>

<EventID>100311</EventID>

<Database>FMH-NO</Database>

<EventType>BidPlaced</EventType>

<RegisterTime>2013.07.16 15:50:56</RegisterTime>

<Parameters>

<LotID>792937</LotID>

<BidPrice>1300</BidPrice>

<Account>FMH-NO:CUS/272663</Account>

<IpAddress>172.22.32.213</IpAddress>

</Parameters>

<Content>

<AuctionItem>

<AuctionItemId>792934</AuctionItemId>

<CurrentPrice>2300</CurrentPrice>

<ImageId>FIL/100506.100</ImageId>

<Timeleft>14d 17h 9m</Timeleft>

<Title></Title>

<AuctionName>Test auksjon</AuctionName>

</AuctionItem>

</Content>

</Event>

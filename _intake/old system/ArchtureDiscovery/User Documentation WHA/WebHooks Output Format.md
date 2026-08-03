---
title: "WebHooks Output Format"
source: "https://rambase.atlassian.net/wiki/spaces/API/pages/38011170/WebHooks+Output+Format"
confluence_page_id: "38011170"
confluence_parent_id: "21954576"
author: "By Tobias Nilsson (Deactivated)"
last_updated: "Feb 20, 2014"
exported: "2026-07-17"
---

# WebHooks Output Format

The output will be on the following format, (JSON / XML) depending on the setup in the WHA. The "Content" element contains the result from the Rambase Api Resource(s) that is setup in the WHA. This is constructed as a list / array and can therefore handle multiple Rambase Api resource data in the same output. An event will only be sent once to one remote url. This means that if the WHA is setup to receive multiple Rambase Api Resource Data on the same EventType to the same Remote Url, the Rambase Api Data will be combined into one list, and only one request to the remote url will be performed, and not one per web hook.

Json Output Format{
 "SystemID": "RIC",
 "RamBaseEventId": 104888,
 "EventType": "HldCommentCreated",
 "Database": "JHC-NO",
 "RegisterTime": "2014.02.20 08:00:40",
 "Parameters": [
 {
 "ParameterName": "HldId",
 "ParameterValue": "230274"
 },
 {
 "ParameterName": "JiraIssue",
 "ParameterValue": ""
 },
 {
 "ParameterName": "CommentId",
 "ParameterValue": "160221"
 },
 {
 "ParameterName": "UserId",
 "ParameterValue": "4580"
 }
 ],
 "Content": [
 {
 "helpDeskRequestComment": {
 "commentId": 160221,
 "subject": "Tilgang til HLD uten å ha tilgang til UserDb",
 "date": "2014-02-20",
 "permission": "ALL",
 "exernalCommentId": "",
 "comment": "Yada yada"
 }
 }
 ]
}

XML Output Format<?xml version="1.0" ?> 
<Event> 
 <SystemID>RIC</SystemID> 
 <RamBaseEventId>104894</RamBaseEventId> 
 <EventType>TestEvent</EventType> 
 <Database>TST-NO</Database> 
 <RegisterTime>2014.02.20 08:18:41</RegisterTime> 
 <Parameters /> 
 <Content> 
 <DomainValues> 
 <DomainValue> 
 <DomainValueId>100018</DomainValueId> 
 <ObjectStatus>4</ObjectStatus> 
 <Object>CNT</Object> 
 <Field>COMMDESCR</Field> 
 <Value>FAX</Value> 
 <Scope>GLOBAL</Scope> 
 <Priority>25</Priority> 
 </DomainValue> 
 <DomainValue> 
 <DomainValueId>100019</DomainValueId> 
 <ObjectStatus>4</ObjectStatus> 
 <Object>CNT</Object> 
 <Field>COMMDESCR</Field> 
 <Value>BUSINESS</Value> 
 <Scope>GLOBAL</Scope> 
 <Priority>25</Priority> 
 </DomainValue> 
 <DomainValue> 
 <DomainValueId>100080</DomainValueId> 
 <ObjectStatus>4</ObjectStatus> 
 <Object>FRM</Object> 
 <Field>SCOPE</Field> 
 <Value>GLOBAL</Value> 
 <Scope>GLOBAL</Scope> 
 <Priority>25</Priority> 
 </DomainValue> 
 <DomainValue> 
 <DomainValueId>100081</DomainValueId> 
 <ObjectStatus>4</ObjectStatus> 
 <Object>FRM</Object> 
 <Field>SCOPE</Field> 
 <Value>LOCAL</Value> 
 <Scope>GLOBAL</Scope> 
 <Priority>25</Priority> 
 </DomainValue> 
 <Paging> <NextPage>/system/domain-values?%24format=XML&amp;%24handle=1_2CzCNTpRLFIGILT_2CzGRPpRLSJBAMK_2C1_2C57.000000&amp;%24offset=1</NextPage> <Position>1</Position> <Size>57</Size> </Paging> 
 </DomainValues> 
 <FieldDescription> 
 <DescriptionKey>HLD.PRIO</DescriptionKey> 
 <TechnicalDescription>Internal priority of request</TechnicalDescription> 
 <Description>Priority of development set by Helpdesk</Description> 
 </FieldDescription> 
 </Content> 
</Event>

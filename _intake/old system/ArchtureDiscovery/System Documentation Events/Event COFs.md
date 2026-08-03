---
title: "Event COFs"
source: "https://rambase.atlassian.net/wiki/spaces/API/pages/21954602/Event+COFs"
confluence_page_id: "21954602"
confluence_parent_id: "21954585"
author: "By Tobias Nilsson (Deactivated)"
last_updated: "Jul 24, 2013"
exported: "2026-07-17"
---

# Event COFs

- 1 [Overview](https://rambase.atlassian.net/wiki/spaces/API/pages/21954602/Event+COFs#Overview)

# Overview

The COFs that are used for the Event system are shown below in the table below and each COF is described further down.

| COF Number | Name | Description |
| --- | --- | --- |
| 39007 | EvrCreate | Create an event in Rambase |
| 38999 | EvrRead | Read Rambase events |
| 39082 | VetParamDelete | Delete a parameter from a VET |
| 39035 | VetListRead | Read list of VETs |
| 39024 | VetParamCreate | Create Parameter for a VET |
| 39022 | VetParamupdate | Update Parameter for a VET |
| 39000 | VetCreate | Create a VET |
| 38993 | VetRead | Read specific VET |
| 38992 | VetUpdate | Update VET |

## EvrCreate (Creating Event)

EvrCreate is one of the most important COF regarding Events. This is used to create an event and this should be called directly before or after the desired change in Rambase has been made.

Input arguments are EVENTTYPE and DB, to describe what type of the event is created and what database it belongs to, as well as ATB(80) that contains the parameters for the event. ATB is a list of KEY and VALUE. The KEY must match one parameter of the Valid Event Type. All required parameters in Valid Event Type must be set in ATB.

Return value is the new DOCID for the created event.

Below is a COS example of how to use EvrCreate. The example is from the Proof Of Concept with Bids as an example. When a bid is made, and event of type "BidPlaced" is created.

BidCreated ExampleEVENTPARAMS(0).KEY = "LotId";
   EVENTPARAMS(0).VALUE = ARG.DOCID.NO;
   
   EVENTPARAMS(1).KEY = "BidPrice";
   EVENTPARAMS(1).VALUE = ARG.BIDMAX;
   
   EVENTPARAMS(2).KEY = "Account";
   EVENTPARAMS(2).VALUE = VAR.ACCOUNT;
   
   EVENTPARAMS(3).KEY = "IpAddress";
   EVENTPARAMS(3).VALUE = VAR.SOURCEIP;
   
   Z=EvrCreate("BidPlaced":EVENTTYPE, "FMH-NO":DB, EVENTPARAMS);

## EvrRead

EvrRead reads a list of events from the EVR. Supports Paging, Sorting, Filtering

No required input arguments, but has HANDLE, MFILTER, OFFSET, SORTBY

Return values are a list of events (RTB) having values ST, NO, DOCID, DB, EVENTTYPE, REGTIME and a list of parameters (PARAMS), having values KEY, PARAMKEY and VALUE. KEY and PARAMKEY is a mapping between events and parameters

Example of EvrReadres RES .( HANDLE, POS, SIZE );

res RTB(80) .( NO, DB, EVENTTYPE, REGTIME, KEY );

res PARAMS(500).( KEY, PARAMKEY, VALUE );

(

( RES, RTB.NO, RTB.KEY:NO, RTB.DB, RTB.EVENTTYPE, RTB.REGTIME, PARAMS ) = EvrRead( ARG, ARG.FILTER:MFILTER);

)

## EvrParamRead

EvrParamRead reads the parameters for a specific Event.

Input argument is DOCID of an event.

Return values are a list of parameters (KEY and VALUE) that are associated with the event. Both Required and optional parameters are returned.

Example of using EvrParamReadPARAMS = EvrParamRead("EVR/100123":DOCID);

## VetRead

Reads a specifc Valid Event Type.

Input Argument: DOCID

Returns information about the Valid Event Type (RES): DOCID, ST, EVENTTYPE, DESCR, HANDLE, POS, SIZE, as well as a list of parameters for the Valid Event Type (RTB): PARAMID, KEY, DATATYPE, ISREQUIRED.

Example of VetRead( RES, RTB ) = VetRead( "VET/100004":DOCID, ARG );

## VetListRead

Reads a list of Valid Event Types. Supports Paging, Sorting, Filtering

Input Arguments:

Return values: List of Valid Event Types (RES): DOCID, ST, EVENTTYPE, DESCR

Example of VetListReadres RES .( HANDLE, POS, SIZE );

var TMP(10).(DOCID, ST, EVENTTYPE, DESCR);

(

(TMP, RES) = VetListRead();

)

## VetCreate

Creates a Valid Event Type. VetCreate only creates the document, with no description or parameters. This should be used together with VetUpdate to add the description and also VetParamCreate to add the parameters.

Input Arguments: ST, EVENTTYPE

Return value: DOCID of the created Valid Event Type

Example of VetCreateVAR.DOCID = VetCreate( "4", "BidPlaced");

## VetUpdate

Updates the status (ST), eventtype (EVENTTYPE) and description (DESCR) of a specific Valid Event Type.

Input Arguments: DOCID, ST, EVENTTYPE, DESCR

Return value is DOCID

If ST is set to "D", it will be considered as deleted.

Example of VetUpdateres RES. (DOCID);

(

RES = VetUpdate("VET/100004":DOCID, "D":ST);

)

## VetParamCreate

Creates a parameter for a specific Valid Event Type

Input Arguments: DOCID, DATATYPE, ISREQUIRED, KEY. The DOCID must be a Valid Event Type that already exists. The datatype must be a valid datatype, i.e. one of the following: "INTEGER,STRING,DATETIME,DATE,TIME,LONG,BOOLEAN,DECIMAL". IsRequired must be set to either "Y" or "N"

Return value: PARAMID, the id of the created parameter

Example of VetParamCreateres RES.(PARAMID);

(

RES.PARAMID = VetParamCreate("VET/123456":DOCID, "integer":DATATYPE, "Y":ISREQUIRED, "Price":KEY);

)

## VetParamUpdate

Updates a parameter for a specific Valid Event Type

Input Arguments: DOCID, PARAMID, DATATYPE, ISREQUIRED, KEY. The datatype must be a valid datatype, i.e. one of the following: "INTEGER,STRING,DATETIME,DATE,TIME,LONG,BOOLEAN,DECIMAL". IsRequired must be set to either "Y" or "N". PARAMID must be a parameter that already exists for the Valid Event Type

Return value: PARAMID, the id of the created parameter

Example of VetParamUpdateres RES.(PARAMID);

(

RES.PARAMID = VetParamUpdate("VET/123456":DOCID, "1":PARAMID, "integer":DATATYPE, "Y":ISREQUIRED, "Price":KEY);

)

## VetParamDelete

Deletes a parameter from a specific Valid Event Type

Input Arguments DOCID, PARAMID,

Return value: PARAMID, the id of the parameter that was deleted

Example of VetParamDeleteres RES.(PARAMID);

(

RES.PARAMID = VetParamDelete("VET/123456":DOCID, "1":PARAMID);

)

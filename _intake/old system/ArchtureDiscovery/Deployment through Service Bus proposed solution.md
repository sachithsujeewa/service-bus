---
title: "Deployment through Service Bus proposed solution"
source: "https://rambase.atlassian.net/wiki/spaces/API/pages/38600741/Deployment+through+Service+Bus+proposed+solution"
confluence_page_id: "38600741"
author: "By Jens Hittenkofer (Deactivated)"
last_updated: "Feb 26, 2014"
exported: "2026-07-17"
---

# Deployment through Service Bus proposed solution
# Solution

The solution is based on the alternative "Proposed Deploy,usingÂ RamBaseEventsandÂ DeploySubscriber" (found here:Â [Deployment through Service Bus](https://rambase.atlassian.net/wiki/spaces/API/pages/38011207)).
When data in a globally synchronized archive is added or updated an Rambase Event must be created (either by COF: EVRCREATE or possible external solution) telling the Service Bus that something has happened (i.e when the published finds it).
The event contains information about the source archive and a primary key. When the publisher finds the message it executes an SQL-query serializing the document into XML, this XML will instantly be used to generate an SQL-query which is in turn put inside a brokered message. The brokered message is then sent to each subscriber (fetched every minute from the NGSystems table on the Repository) and the subcriber executes the SQL-query.

The solution is based on sending GLOBALREVID for an included archive into the Rambase event.
Each Rambase system will need to have the event id for it's latest written synchronization data saved.

## Data source

To guarantee that no data will be lost, the system that acts as the source system (where the changes to the data later deployed out occur) always need to have the latest dictionary version of the archives being synchronized. Because the events are tagged with each archives version, the deployment of the synced change can be halted until the target system is at the same version as the message. This source cannot be the THG-system because that means THG always has to be updated to the most recent version before all other rbsystems. This is ofcourse something we will strive for but it would make the deployment to all other rbsystems dependet on THG which feels like anÂ unnecessary limitation. Thus we need a new rbsystem or at least the database-part which we will use as the source, this system will also always recieve new versions before all of the other systems (can be forced in code or specified in a deployment-protocol so the persons responsible make sure this happens).

Special cases

## Case: A target-rbsystem is down for a longer period in which fields have been added and then removed from the source system.

## During that period a field has been added in a version, events for that version has been sent out, and then in a later version the

A target-rbsystem is down for a longer period. During that period a field has been added to an archive, events has been created for that version of the archive and sent out. Afterwards that same field has been removed from the archive in a later version. When the target-rbsystem comes online it gets updated to the latest version instantly. When the subscriber tries to deploy the document some, which were created when the source had the field will fail because the archive-version for the document is lower than the target-rbsystems version of that archive.

#### State: Target-rbsystems XXX-archive has a higher revision id than the revision id for XXX-archive inside the event.

#### Solution: The SQL-query checks System B's revision id and sees that it's higher, then it cross-matches the fields on each table and removes the update for the removed field(s). Loss of data here isnt important because the field isn't used anyhow. Although there is a scenario where for example a RAW-sql was run during the deployment, and it copied data from the field that was created and removed. This data would be that fields default value in the target-rbsystems case because the service bus didn't get a chance to deploy it.Â This is something thats very hard to deal with when we have the 'regular deployment solution' which always follows the same path, and then we have a separate system doing things outside of it, the final solution is to make sure no one creates this scenario!

## Case: A field is added on an event source and the event is deployed onto a system without it

System A has a field more than System B on the XXX-archive. Events originating from System A are created and the subscriber deploys them onto System B.

#### State: System B's XXX-archive has a lower revision id than the revision id for XXX-archive inside the event.

#### Solution: The SQL-query detects System B's lower revision id and throws an exception making the subscriber wait and retry. When System B's dictionary becomes updated (through a Repository deploy) SQL-query will execute and write the data.

## Case: When cloning the ModelDB onto a new Nextgen system there might be a gap in the synchronization to the new Nextgen system

The Service Bus polls the NGSystems table in the Repository with a 1 minute interval. When cloning the ModelDB onto a new NextGen system the Service Bus won't know of the new systems existence immediately and thus the events created in that time window wont be queued for that system until a subscriber is created for it (when the Service Bus notices it).

#### Solution: Each Rambase System knows the event id for the latest global archive data it recieved. When the system is halted for cloning that id will be copied to the clone. When the Service Bus detects a NEW system it check the difference between the new system's latest event id and latest published event. Then deploys the "event difference" onto the new system.

# Problems / Questions to be answered

### If the name of a field is changed (supported at all in dictionary?)

It's not possible to detect a name change on a field in this solution. To do this a lookup into the revision history in repository is necessary

### Service Bus Topic special case for Deploy events.

To avoid rehandling events for all other systems that are not new, we may need a topic for each system, so that the synchronization of events only are performed on the newly detected system.

### Deploying to development systems with altered dictionaries for affected archives might create problems (due to RGLOBSUGREVID)?

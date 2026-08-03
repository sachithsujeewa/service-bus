---
title: "Service Bus: Activating or Deactivating an SQL system"
source: "https://rambase.atlassian.net/wiki/spaces/API/pages/47612287/Service+Bus+Activating+or+Deactivating+an+SQL+system"
confluence_page_id: "47612287"
confluence_parent_id: "36045144"
author: "By Tobias Nilsson (Deactivated)"
last_updated: "Jun 13, 2014"
exported: "2026-07-17"
---

# Service Bus: Activating or Deactivating an SQL system

RIC is a special case. see bottom for how to handle the RIC system.

# Deactivate the Service Bus for a system

To deactivate the Service Bus for the entire system

- In the NGSystem table in the Repository database (192.168.10.239\STDINST):

When this value is removed, it can take up to one minute before the Service Bus stops monitoring the system.

# Activate the Service Bus for a system

- In the PublishedEvent table in the RambaseSeviceBus database ([JHCVMSQL01.netsentral.no](http://jhcvmsql01.netsentral.no/)):

- In the WHA archive (For testing):

- In the NGSystem table in the Repository database (192.168.10.239\STDINST):

After the SB_ID has been set, the following will happen on the Service Bus:

- ** It can take up to one minute before the Service Bus starts monitoring the system.**

- All ST:4 WHA entries are now handled by the Service Bus.

- All events in EVR, starting from LastPublishedEvent, will be handled.

# Activating / Deactivating for system RIC

All directions regarding activating and deactivating explained above (except for the SB_ID column) also applies for the RIC system.

RIC is treated separately from the other systems. In the Service Bus config file, there is an entry called HandleRIC. This is set to true for the Service Bus with ServiceBusID = 1. Setting this to false and restarting the Service Bus Service will cause it not to hande the RIC system anymore.

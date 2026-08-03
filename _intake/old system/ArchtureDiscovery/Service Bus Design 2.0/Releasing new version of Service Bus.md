---
title: "Releasing new version of Service Bus"
source: "https://rambase.atlassian.net/wiki/spaces/API/pages/48922730/Releasing+new+version+of+Service+Bus"
confluence_page_id: "48922730"
confluence_parent_id: "36045144"
author: "By Tobias Nilsson (Deactivated)"
last_updated: "Jul 01, 2014"
exported: "2026-07-17"
---

# Releasing new version of Service Bus

The Service Bus Rambase Service is running as a Windows Service with the name "ServiceBusRambaseService" at 172.21.13.121.

The code can be found on SVN in the path: rbsystem\RBPush\trunk\ServiceBus\ServiceBus.sln

Steps to perform to release a new version of the Service Bus:

1. Compile the project in "Release" Configuration
2. Stop the Service Bus Rambase Service (Prepare all steps up to step 4 as much as you can before doing this to minimize downtime)
3. Copy the contents of rbsystem\RBPush\trunk\ServiceBus\ServiceBusService\bin\Release (except for the file ServiceBusService.exe.config)
4. Start the Service Bus Rambase Service

---
title: "Event-driven development and code hooks"
source: "https://rambase.atlassian.net/wiki/spaces/API/pages/1855258681/Event-driven+development+and+code+hooks"
confluence_page_id: "1855258681"
author: "By Roger Gullhaug"
last_updated: ""
exported: "2026-07-17"
---

# Event-driven development and code hooks
We already have an event concept in RamBase today, but it is so far only used for webhooks. Already when it was developed, many years ago, we had some ideas for how to extend it to something more.

It is always good to structure code and modules so that it is loosely coupled. The standard way of doing it is to publish events and then create a way of subscribing to those events. An example could be that when you register an order, you publish an â€œOrderRegisteredâ€ event. This is often referred to as â€œfire-and-forgetâ€ events. You fire the event, and donâ€™t care who is listening for that event to happen. Everyone that is interested in the â€œOrderRegisterâ€ event can subscribe to the event and react to it when it happens. An example could be that every time an order is register you would like to listen for that event and inform the customer when it happened (send an e-mail). The developer responsible for the code for registering an order, just publish the event and doesnâ€™t know, and donâ€™t need to know how many is subscribing to that event.

So far the only way to subscribe to events in RamBase has been to subscribe to webhooks. Webhooks is a very nice way for external systems to listen for events in RamBase, but is not so well suited for internal events inside a RamBase system. For that we need something new, and it is natural to think that it should be our batch system that should handle these event.

## Event triggered forms

It shouldnâ€™t be so hard to allow forms to be triggered by events. All we need is a way to specify on the form which event this form should be triggered by. Today we have a shared method named System.RamBaseEvent::Create that is responsible for creating events (EVR). This COS could check if it finds forms that should be triggered by this particular event, and if it does it triggers the job.

This type of events is still a fire-and-forget approach, just like webhooks. You fire an event and you donâ€™t care if any forms will be triggered by this event. In some cases this is not good enough, because you need the events to be synchronous, meaning that you should not continue the execution of your code before all the subscribers of the event has finished the execution of their code. These types of events are typically referred to as synchronous events, or code hooks.

## Code hooks

Fire-and-forget/asynchronous events can be very valuable in many cases, and for internal development (development done by RamBase developers) it may be all you really need. If you have a need for some code to be executed synchronous  when an event happens, it will always be a possibility for internal developers to just insert that piece of code directly, instead of relying on an event. Event if it is possible to insert more and more shared functions in existing COSâ€™s to further extend the application, it can be a much better approach to use fire an event (code hook), and donâ€™t care about who is listening and reacting to the events.

A good example of where a code hooks/synchronous event could be used is the Polish Whitelist check that ITVision has developed for us. It is a requirement from the Polish government that you are not allowed to pay any of your suppliers before you have checked that the bank account number the supplier has given you is whitelisted. If it is not whitelisted you should stop the payment.

To handle this with code hooks we can add an event â€œBeforePaymentStartedâ€ and fire that event when the payment process is started, but before the money is transferred to the bank. The developers of the payment process donâ€™t care who is listening to the event and just write their code as if there was no ones listening, but they need to keep in mind that the COS execution could stop after this event if any of the listening code hooks prevents further execution.

The developers of the whitelist check could then hook into this code by registering a COS that should be run on this event - â€œBeforePaymentStartedâ€. This COS has a standard interface so that it can get the information it needs from the event and can return information on a standard format. The return value is typically OK or NOT OK, and if not ok, there will also be a message that will be the reason for stopping the program execution. In this way the COS that has been registered to run as a code hook when the event happens can check the Polish Whitelist registry and if it is NOT OK, then return a message â€œBank account not in the whitelistâ€ and the payment process is then aborted.

Code hooks can be vary valuable for our internal developers to write more decoupled code, but after RACR, when external developers will be allowed to write COS it can be a very powerful tool for them to be able to extend and tweak RamBase to their needs.

These concepts should be seen together with [Process automation - Workflow](https://rambase.atlassian.net/wiki/spaces/ID/pages/297893952)

[Jira automation template library | Atlassian](https://www.atlassian.com/software/jira/automation-template-library)

Inspiration [https://docs.microsoft.com/en-us/powerapps/developer/common-data-service/apply-business-logic-with-code](https://docs.microsoft.com/en-us/powerapps/developer/common-data-service/apply-business-logic-with-code)

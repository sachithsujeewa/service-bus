---
title: "What is web hooks?"
source: "https://rambase.atlassian.net/wiki/spaces/API/pages/16089137/What+is+web+hooks"
confluence_page_id: "16089137"
author: "By Jon Terje Aksland (Deactivated)"
last_updated: "Apr 18, 2013"
exported: "2026-07-17"
---

# What is web hooks?
What is the difference between HTTP streaming and web hooks? Is HTTP streaming for web browsers only?Â [http://ajaxpatterns.org/HTTP_Streaming](http://ajaxpatterns.org/HTTP_Streaming)

The way I understand this is that HTTP streaming keeps the connection open between messages, whereas web hooks establishes one connection for each message.

"The trouble with Webhooks is that you need a publicly visible URL to handle them. Unlike client-side redirects, webhooks originate directly from the server. This means that you can't use localhost as an endpoint in your testing environment as the API server would effectively be calling itself.Â Fortunately, there are a couple of tools that make working with webhooks during development much easier, such as PostCatcher and LocalTunnel"Â [http://www.shopify.com/technology/3931292-webhook-testing-made-easy#axzz2QnYqcsfG](http://www.shopify.com/technology/3931292-webhook-testing-made-easy#axzz2QnYqcsfG)

### Systems:

Windows Service Bus

Amazon SNS - works with other Amazon AWS services

[https://code.google.com/p/pubsubhubbub/](https://code.google.com/p/pubsubhubbub/) - A simple, open, web-hook-based pubsub protocol & open source reference implementation.

[https://code.google.com/p/ops/](https://code.google.com/p/ops/)

### Examples:

[http://www.lightstreamer.com/](http://www.lightstreamer.com/)Â (HTTP streaming?)

[http://tweetping.net/](http://tweetping.net/)

# Service Bus Relay and Queue/Topic sample
This sample shows a simple "e-commerce" application that uses [Service Bus Relay](https://azure.microsoft.com/en-gb/documentation/articles/service-bus-relay-overview/) to communicate to an on-premises service for checking stock of items. In addition, it uses [Service Bus Topics](https://azure.microsoft.com/en-gb/documentation/articles/service-bus-queues-topics-subscriptions/) to place an order. The message (order) is then consumed in the same on-premises service. 


## Getting Started

1. Create a Service Bus Namespace through http://manage.windowsazure.com. Detailed instructions are available [here](https://azure.microsoft.com/en-gb/documentation/articles/service-bus-dotnet-how-to-use-relay/).

2. Create, or use the default management key and copy the key's name and value to the app.config and web.config files


## Why topics?

Topics are used in this sample, because they are useful in a similar scenario, where you have potentially multiple subscribers listening to a single message. For example, you could have the StockService and an InvoicingService listening to the same "Order" message. When one is received the invoice is generated, stored and prepared for sending, while the StockService processes the order.  

## More information

Patterns used:

- [Cache Aside](https://msdn.microsoft.com/en-gb/library/dn589799.aspx)
- [Service Gateway](https://msdn.microsoft.com/en-us/library/ff650101.aspx)

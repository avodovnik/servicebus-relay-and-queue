using Microsoft.ServiceBus;
using Microsoft.ServiceBus.Messaging;
using StoreSample.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StoreSample.Web.Gateway
{
    /// <summary>
    /// While this is called an OrderServiceGateway, there is no actual service in the back, but 
    /// the same pattern has been used as in <see cref="StockServiceGateway"/> to provide consistency.
    /// The only difference is, it has been simplified a bit, and made static, versus exposing a 
    /// singleton instance. 
    /// </summary>
    public class OrderServiceGateway
    {
        /// <summary>
        /// Sends out the order for the certain product, with the specified quantity.
        /// </summary>
        /// <param name="productId"></param>
        /// <param name="quantity"></param>
        public static void SendOrder(int productId, int quantity)
        {
            var serviceNamespace = System.Configuration.ConfigurationManager.AppSettings.Get("sb:namespace");
            var keyName = System.Configuration.ConfigurationManager.AppSettings.Get("sb:keyName");
            var sharedAccessKey = System.Configuration.ConfigurationManager.AppSettings.Get("sb:sharedAccessKey");
            var topicName = System.Configuration.ConfigurationManager.AppSettings.Get("sb:topicName");
            var tokenProvider = TokenProvider.CreateSharedAccessSignatureTokenProvider(keyName, sharedAccessKey);

            // create the client that will listen to the subscription
            var messagingFactory = MessagingFactory.Create(ServiceBusEnvironment.CreateServiceUri("sb", serviceNamespace, String.Empty), tokenProvider);

            var client = messagingFactory.CreateTopicClient(topicName);

            // and now we just send the message into the queue
            client.Send(new BrokeredMessage(new Order() { ProductId = productId, Quantity = quantity }));

            // clean-up after ourselves
            client.Close();
        }
    }
}
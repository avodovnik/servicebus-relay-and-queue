using Microsoft.ServiceBus;
using Microsoft.ServiceBus.Messaging;
using StoreSample.Models;
using System;
using System.ServiceModel;

namespace StoreSample.StockService
{
    /// <summary>
    /// This part of the sample handles the display of stock. It also represents our "storage" for the products.
    /// It exposes two basic operations: getting all the items available, and getting stock for a specific item.
    /// </summary>
    class Program
    {
        private static string _keyName;
        private static string _serviceNamespace;
        private static string _servicePath;
        private static string _sharedAccessKey;
        private static TokenProvider _tokenProvider;

        private static string SubscriptionName = "StockService";

        static void Main(string[] args)
        {
            // we initialize the service host programatically
            var host = new ServiceHost(typeof(StockService));

            // this requires the System.Configuraiton reference to be added

            _serviceNamespace = System.Configuration.ConfigurationManager.AppSettings.Get("sb:namespace");
            _servicePath = System.Configuration.ConfigurationManager.AppSettings.Get("sb:servicePath");
            _keyName = System.Configuration.ConfigurationManager.AppSettings.Get("sb:keyName");
            _sharedAccessKey = System.Configuration.ConfigurationManager.AppSettings.Get("sb:sharedAccessKey");
            _tokenProvider = TokenProvider.CreateSharedAccessSignatureTokenProvider(_keyName, _sharedAccessKey);

            // and add the service endpoint
            host.AddServiceEndpoint(typeof(Models.Contracts.IStockService), new NetTcpRelayBinding(),
                // we need to build the service URI, which is composed of the sb prefix, the namespace and service path
                ServiceBusEnvironment.CreateServiceUri("sb", _serviceNamespace, _servicePath))
                // next, we need to describe how our service is authenticated to our namespace 
                .EndpointBehaviors.Add(new TransportClientEndpointBehavior
                { TokenProvider = _tokenProvider });

            // note: you could configure this in the app.config file as well, but we won't, for simplicity's sake

            // Step 2, initiate the listening to our Topic
            StartTopicListener();

            // open the host
            host.Open();

            Console.WriteLine("Stock service is ready and listening...");
            Console.WriteLine("Press [Enter] to exit.");
            Console.ReadLine();

            // close the host
            host.Close();
        }

        /// <summary>
        /// This will not only start listening to the topic, and handle the orders coming in, but will also 
        /// create the topic, if one does not exist. Normally, you'd have these concerns separated, but for
        /// our sample, this is good enough.
        /// </summary>
        static void StartTopicListener()
        {
            var namespaceManager = new NamespaceManager(ServiceBusEnvironment.CreateServiceUri("sb", _serviceNamespace, String.Empty),
                _tokenProvider);

            var topicName = System.Configuration.ConfigurationManager.AppSettings.Get("sb:topicName");

            // we can easily create a topic, if it doesn't exist yet
            if (!namespaceManager.TopicExists(topicName))
            {
                var td = namespaceManager.CreateTopic(topicName);

                // since we're creating the topic, chances are, we also need to create a subscription
                namespaceManager.CreateSubscription(topicName, SubscriptionName);
            }

            // create the client that will listen to the subscription
            var messagingFactory = MessagingFactory.Create(namespaceManager.Address, _tokenProvider);
            var client = messagingFactory.CreateSubscriptionClient(topicName, SubscriptionName);

            // setup the handler for when a message is recieved
            client.OnMessage((message) =>
            {
                Console.ForegroundColor = ConsoleColor.Gray;
                Console.WriteLine("Order message recieved...");
                Console.ResetColor();

                var order = message.GetBody<Order>();

                // call into our little repository
                StockRepositoryDummy.Instance.DecreaseStock(order.ProductId, order.Quantity);

                Console.WriteLine("Order for product {0}, Qty {1} processed.", order.ProductId, order.Quantity);

                // to remove the message completely, we need to mark it as complete
                message.Complete();
            });

            Console.WriteLine("Listening to topic...");
        }
    }
}

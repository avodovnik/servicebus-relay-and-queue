using Microsoft.ServiceBus;
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
        static void Main(string[] args)
        {
            // we initialize the service host programatically
            var host = new ServiceHost(typeof(StockService));

            // this requires the System.Configuraiton reference to be added

            var serviceNamespace = System.Configuration.ConfigurationManager.AppSettings.Get("sb:namespace");
            var servicePath = System.Configuration.ConfigurationManager.AppSettings.Get("sb:servicePath");
            var keyName = System.Configuration.ConfigurationManager.AppSettings.Get("sb:keyName");
            var sharedAccessKey = System.Configuration.ConfigurationManager.AppSettings.Get("sb:sharedAccessKey");

            // and add the service endpoint
            host.AddServiceEndpoint(typeof(Models.Contracts.IStockService), new NetTcpRelayBinding(),
                // we need to build the service URI, which is composed of the sb prefix, the namespace and service path
                ServiceBusEnvironment.CreateServiceUri("sb", serviceNamespace, servicePath))
                // next, we need to describe how our service is authenticated to our namespace 
                .EndpointBehaviors.Add(new TransportClientEndpointBehavior
                { TokenProvider = TokenProvider.CreateSharedAccessSignatureTokenProvider(keyName, sharedAccessKey) });

            // note: you could configure this in the app.config file as well, but we won't, for simplicity's sake

            // open the host
            host.Open();

            Console.WriteLine("Stock service is ready and listening...");
            Console.WriteLine("Press [Enter] to exit.");
            Console.ReadLine();

            // close the host
            host.Close();
        }
    }
}

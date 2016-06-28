using Microsoft.ServiceBus;
using StoreSample.Models;
using StoreSample.Models.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Web;

namespace StoreSample.Web.Gateway
{
    /// <summary>
    /// The gateway provides us an abstration for communicating with our on-premises service. Using
    /// the gateway pattern itself helps us contain the complexity involved in establishing the relay
    /// channel, etc. contained in a single place. Other approaches, such as static caching of products
    /// and so on, are *bad* practice, especially for web applications. If you do want to do something
    /// like this, using something like Redis cache is strongly recommended.
    /// </summary>
    public class StockServiceGateway
    {
        private static StockServiceGateway _instance;
        public static StockServiceGateway Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new StockServiceGateway();
                }
                return _instance;
            }
        }

        private StockServiceGateway() { }

        private IStockServiceChannel CreateChannel()
        {
            var serviceNamespace = System.Configuration.ConfigurationManager.AppSettings.Get("sb:namespace");
            var servicePath = System.Configuration.ConfigurationManager.AppSettings.Get("sb:servicePath");
            var keyName = System.Configuration.ConfigurationManager.AppSettings.Get("sb:keyName");
            var sharedAccessKey = System.Configuration.ConfigurationManager.AppSettings.Get("sb:sharedAccessKey");

            var cf = new ChannelFactory<IStockServiceChannel>(
                new NetTcpRelayBinding(),
                new EndpointAddress(ServiceBusEnvironment.CreateServiceUri("sb", serviceNamespace, servicePath)));

            cf.Endpoint.Behaviors.Add(new TransportClientEndpointBehavior
            { TokenProvider = TokenProvider.CreateSharedAccessSignatureTokenProvider(keyName, sharedAccessKey) });

            return cf.CreateChannel();
        }

        /// <summary>
        /// Gets all available products, by calling into the relay service.
        /// </summary>
        /// <returns></returns>
        public List<Product> GetAllProducts()
        {
            // this uses a cache-aside pattern, but keep in mind, the cache is fake

            // 1. check if items are in cache
            if (_fakeProductsCache == null)
            {
                // 2. if not, add them
                using (var c = CreateChannel())
                {
                    _fakeProductsCache = c.GetAvailableProducts();

                    // we'll add this, to show the user how "stale" the cache is
                    LastUpdateTime = DateTime.Now;
                }
            }

            // 3. return from the "cache"
            return _fakeProductsCache;
        }

        internal Product GetProduct(int id)
        {
            return GetAllProducts().Where(x => x.Id == id).Single();
        }

        internal void InvalidateCache()
        {
            this._fakeProductsCache = null;
        }

        private List<Product> _fakeProductsCache = null;
        public DateTime LastUpdateTime;
    }
}
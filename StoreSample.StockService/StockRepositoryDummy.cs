using StoreSample.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoreSample.StockService
{
    /// <summary>
    /// This is a dummy repository, holding information about the products and their available stock.
    /// </summary>
    internal class StockRepositoryDummy
    {
        private static StockRepositoryDummy _instance = null;
        public static StockRepositoryDummy Instance
        {
            // a simple implementation of the singleton pattern, without locking, etc.
            get
            {
                if (_instance == null)
                {
                    _instance = new StockRepositoryDummy();
                }

                return _instance;
            }
        }

        internal List<Product> GetAllProducts()
        {
            return _products;
        }

        private StockRepositoryDummy()
        {
            // we'll populate our dummy r Id = 1,epository with the list of dummy products and their stock
            _products.Add(new Product() { Id = 1, Name = "Wheel Spokes, 18\"", Price = 178.5M, Stock = 4 });
            _products.Add(new Product() { Id = 2, Name = "Dashboard", Price = 898.8M, Stock = 35 });
            _products.Add(new Product() { Id = 3, Name = "Engine V10", Price = 19298.1M, Stock = 60 });
            _products.Add(new Product() { Id = 4, Name = "Extra leather seats", Price = 609.4M, Stock = 1000});
        }

        private List<Product> _products = new List<Product>();

        internal int DecreaseStock(int productId, int quantity)
        {
            var p = _products.Single(x => x.Id == productId);

            // this is obviously wrong; in the real world, we'd need to check the stock is there,
            // and if not, reject the order, potentially moving it into a different queue, or a 
            // backlog, and making sure there's a restocking process that started
            p.Stock = p.Stock - quantity;

            return p.Stock;
        }
    }
}

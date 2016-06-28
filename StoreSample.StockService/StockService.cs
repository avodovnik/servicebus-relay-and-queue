using System;
using System.Collections.Generic;
using StoreSample.Models;
using StoreSample.Models.Contracts;

namespace StoreSample.StockService
{
    /// <summary>
    /// This is the implementation of the stock service.
    /// </summary>
    class StockService : IStockService
    {
        public List<Product> GetAvailableProducts()
        {
            System.Diagnostics.Debug.WriteLine("Available products queried...");
            
            return StockRepositoryDummy.Instance.GetAllProducts();
        }

        public int GetAvailableQuantity(int productId)
        {
            throw new NotImplementedException();
        }
    }
}

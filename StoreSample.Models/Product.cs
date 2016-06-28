using System;

namespace StoreSample.Models
{
    /// <summary>
    /// The product class describes the product available through the
    /// stock service. 
    /// </summary>
    public class Product
    {
        public int Id { get; set; }

        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the price in local currency.
        /// </summary>
        public decimal Price { get; set; }

        public int Stock { get; set; }
    }
}

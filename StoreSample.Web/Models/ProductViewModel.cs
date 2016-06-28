using StoreSample.Models;
using System;

namespace StoreSample.Web.Models
{
    /// <summary>
    /// We've created a separate view model for the product page, to also display the 
    /// time of the last cache refresh.
    /// </summary>
    public class ProductViewModel
    {
        public Product Product { get; set; }
        public DateTime LastCacheRefresh { get; set; }
    }
}
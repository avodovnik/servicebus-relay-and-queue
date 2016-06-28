using System.Collections.Generic;
using System.ServiceModel;

namespace StoreSample.Models.Contracts
{
    [ServiceContract(Namespace="urn:ps")]
    public interface IStockService
    {
        /// <summary>
        /// Gets a list of all available products, along with a rough stock estimate.
        /// </summary>
        /// <returns>Returns a list of products, including their stock quantity at the time of the call.</returns>
        [OperationContract]
        List<Product> GetAvailableProducts();

        /// <summary>
        /// Queries the available stock of a single product.
        /// </summary>
        /// <param name="productId">The id of the product.</param>
        /// <returns>Returns the number of units of the product still available.</returns>
        [OperationContract]
        int GetAvailableQuantity(int productId);
    }
}

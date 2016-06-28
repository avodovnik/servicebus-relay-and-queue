using System.ServiceModel;

namespace StoreSample.Models.Contracts
{
    /// <summary>
    /// This interface enables the client to more easily manage the proxy lifetime. Creating such an interface is considered a best practice.
    /// </summary>
    public interface IStockServiceChannel : IStockService, IClientChannel
    {
    }
}

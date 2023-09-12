using Shopping.Aggregator.Models;

namespace Shopping.Aggregator.Services
{
    public interface IOrderingService
    {
        Task<IEnumerable<OrderResponseModel>?> GetOrderByUserName(string userName);
    }
}

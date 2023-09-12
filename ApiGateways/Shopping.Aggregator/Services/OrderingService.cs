using Shopping.Aggregator.Extensions;
using Shopping.Aggregator.Models;

namespace Shopping.Aggregator.Services
{
    public class OrderingService : IOrderingService
    {
        private readonly HttpClient _client;

        public OrderingService(HttpClient client) => this._client = client;


        public async Task<IEnumerable<OrderResponseModel>?> GetOrderByUserName(string userName)
        {
            var response = await this._client.GetAsync($"/api/v1/Order/get-orders/{userName}");
            return await response.ReadContentAs<List<OrderResponseModel>>();
        }
    }
}

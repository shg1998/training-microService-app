using Basket.Api.Entities;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;

namespace Basket.Api.Repositories
{
    public class BasketRepository : IBasketRepository
    {
        private readonly IDistributedCache _cache;

        public BasketRepository(IDistributedCache cache) => _cache = cache;

        public async Task DeleteBasket(string username) => await this._cache.RemoveAsync(username);

        public async Task<ShoppingCart?> GetUserBasket(string username)
        {
            var basket = await this._cache.GetStringAsync(username);
            if (string.IsNullOrEmpty(basket))
                return null;
            return JsonConvert.DeserializeObject<ShoppingCart?>(basket);
        }

        public async Task<ShoppingCart?> UpdateBasket(ShoppingCart basket)
        {
            await this._cache.SetStringAsync(basket.Username,JsonConvert.SerializeObject(basket));
            return await this.GetUserBasket(basket.Username);
        }
    }
}

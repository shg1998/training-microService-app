using System.Net;
using Microsoft.AspNetCore.Mvc;
using Shopping.Aggregator.Models;
using Shopping.Aggregator.Services;

namespace Shopping.Aggregator.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class ShoppingController : ControllerBase
    {
        private readonly ICatalogService _catalogService;
        private readonly IBasketService _basketService;
        private readonly IOrderingService _orderingService;

        public ShoppingController(ICatalogService catalogService, IBasketService basketService, IOrderingService orderingService)
        {
            this._catalogService = catalogService;
            this._basketService = basketService;
            this._orderingService = orderingService;
        }

        [HttpGet("{username}",Name = "get-shopping")]
        [ProducesResponseType(typeof(ShoppingModel),(int)HttpStatusCode.OK)]
        public async Task<ActionResult<ShoppingModel>> GetShopping(string username)
        {
            var basket = await this._basketService.GetBasket(username);
            if (basket?.Items != null)
                foreach (var item in basket.Items)
                {
                    var product = await this._catalogService.GetCatalog(item.ProductId);
                    if (product == null) continue;
                    item.ProductName = product.Name;
                    item.Category = product.Category;
                    item.Summary = product.Summary;
                    item.Description = product.Description;
                    item.ImageFile = product.ImageFile;
                }

            var orders = await this._orderingService.GetOrderByUserName(username);
            var shoppingModel = new ShoppingModel
            {
                UserName = username,
                Basket = basket!,
                Orders = orders!
            };

            return Ok(shoppingModel);
        }
    }
}

using AutoMapper;
using Basket.Api.Entities;
using Basket.Api.GrpcServices;
using Basket.Api.Repositories;
using EventBus.Messages.Events;
using MassTransit;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Basket.Api.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class BasketController : ControllerBase
    {
        private readonly IBasketRepository _repository;
        private readonly DiscountGrpcService _discountGrpcService;
        private readonly IPublishEndpoint _publishEndpoint;
        private readonly IMapper _mapper;
        public BasketController(IBasketRepository repository, DiscountGrpcService discountGrpcService, IMapper mapper, IPublishEndpoint publishEndpoint)
        {
            this._repository = repository;
            this._discountGrpcService = discountGrpcService;
            this._mapper = mapper;
            this._publishEndpoint = publishEndpoint;
        }

        [HttpGet("{username}", Name = "GetBasketByUsername")]
        [ProducesResponseType(typeof(ShoppingCart), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<ShoppingCart>> GetBasket(string username)
        {
            var basket = await this._repository.GetUserBasket(username);
            return Ok(basket ?? new ShoppingCart(username));
        }

        [HttpPost]
        [ProducesResponseType(typeof(ShoppingCart), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<ShoppingCart>> UpdateBasket([FromBody] ShoppingCart basket)
        {
            foreach (var item in basket.Items)
            {
                var coupon = await this._discountGrpcService.GetDiscount(item.ProductName);
                item.Price -= coupon.Amount;
            }
            return Ok(await this._repository.UpdateBasket(basket));
        }

        [HttpDelete("{username}", Name = "DeleteBasketByUsername")]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> DeleteBasket(string username)
        {
            await this._repository.DeleteBasket(username);
            return Ok();
        }

        #region checkout

        [HttpPost("[action]")]
        [ProducesResponseType((int)HttpStatusCode.Accepted)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> Checkout([FromBody] BasketCheckout basketCheckout)
        {
            // get existing basket with total price
            var basket = await _repository.GetUserBasket(basketCheckout.UserName);
            if (basket == null)
            {
                return BadRequest();
            }


            // create BasketCheckoutEvent -- set total price on basketCheckout event message
            var eventMessage = _mapper.Map<BasketCheckoutEvent>(basketCheckout);
            eventMessage.TotalPrice = basket.TotalPrice;

            // send checkout event to rabbitmq
            await _publishEndpoint.Publish(eventMessage);

            // remove basket
            await _repository.DeleteBasket(basketCheckout.UserName);

            return Accepted();
        }

        #endregion
    }
}

using MediatR;

namespace Ordering.Application.Features.Orders.Queries.GetOrdersList
{
    public class GetOrdersListQuery : IRequest<List<OrdersViewModel>>
    {
        public string UserName { get; set; }
        public GetOrdersListQuery(string userName)
        {
            this.UserName = userName;
        }
    }
}

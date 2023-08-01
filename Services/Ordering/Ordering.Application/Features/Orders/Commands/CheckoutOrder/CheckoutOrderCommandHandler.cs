using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Ordering.Application.Contracts.Infrastructure;
using Ordering.Application.Contracts.Persistence;
using Ordering.Domain.Entities;

namespace Ordering.Application.Features.Orders.Commands.CheckoutOrder
{
    public class CheckoutOrderCommandHandler : IRequestHandler<CheckoutOrderCommand, int>
    {
        private readonly IOrderRepository _repository;
        private readonly IMapper _mapper;
        private readonly IEmailService _emailService;
        private readonly ILogger<CheckoutOrderCommandHandler> _logger;

        public CheckoutOrderCommandHandler(IOrderRepository repository, IMapper mapper, IEmailService service, ILogger<CheckoutOrderCommandHandler> logger)
        {
            this._repository = repository;
            this._mapper = mapper;
            this._emailService = service;
            this._logger = logger;
        }

        public async Task<int> Handle(CheckoutOrderCommand request, CancellationToken cancellationToken)
        {
            var orderEntity = this._mapper.Map<Order>(request);
            var newOrder = await this._repository.AddAsync(orderEntity);
            this._logger.LogInformation("Order is Successfully Created!");
            await this.SendEmail(newOrder);
            return newOrder.Id;
        }        

        private async Task SendEmail(Order order)
        {
            try
            {
                // send Email
                await this._emailService.SendEmail(new Models.Email
                {
                    Body = "dhgkdhgdkghdkgkgd ldhdjkghdjhgdhgdjgdjdgjdgjdgjdgjgdjgd",
                    Subject = " kdhgkdgdjkgjdfghfdv djtgdjgd",
                    To = "dkhkdhd@khkhd.hdkhdk"
                });
            }
            catch (Exception e)
            {
                this._logger.LogError("There is problem with sending mail");
            }
        }
    }
}

using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Ordering.Application.Contracts.Infrastructure;
using Ordering.Application.Contracts.Persistence;
using Ordering.Application.Features.Orders.Commands.CheckoutOrder;
using Ordering.Domain.Entities;

namespace Ordering.Application.Features.Orders.Commands.UpdateOrder
{
    public class UpdateOrderCommandHandler : IRequestHandler<UpdateOrderCommand>
    {
        private readonly IOrderRepository _repository;
        private readonly IMapper _mapper;
        private readonly IEmailService _emailService;
        private readonly ILogger<UpdateOrderCommandHandler> _logger;

        public UpdateOrderCommandHandler(IOrderRepository repository, IMapper mapper, IEmailService service, ILogger<UpdateOrderCommandHandler> logger)
        {
            this._repository = repository;
            this._mapper = mapper;
            this._emailService = service;
            this._logger = logger;
        }

        public async Task<Unit> Handle(UpdateOrderCommand request, CancellationToken cancellationToken)
        {
            var order4Update = await this._repository.GetByIdAsync(request.Id);
            if(order4Update == null)
            {
                this._logger.LogError("order wa not exists!");
            }
            this._mapper.Map(request, order4Update, typeof(UpdateOrderCommand), typeof(Order));
            await this._repository.UpdateAsync(order4Update);
            this._logger.LogInformation("order updated!");
            return Unit.Value;
        }
    }
}

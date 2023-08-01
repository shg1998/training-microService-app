using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Ordering.Application.Contracts.Infrastructure;
using Ordering.Application.Contracts.Persistence;
using Ordering.Application.Exceptions;
using Ordering.Domain.Entities;

namespace Ordering.Application.Features.Orders.Commands.DeleteOrder
{
    public class DeleteOrderCommandHandler : IRequestHandler<DeleteOrderCommand>
    {

        private readonly IOrderRepository _repository;
        private readonly IMapper _mapper;
        private readonly ILogger<DeleteOrderCommandHandler> _logger;

        public DeleteOrderCommandHandler(IOrderRepository repository, IMapper mapper, IEmailService service, ILogger<DeleteOrderCommandHandler> logger)
        {
            this._repository = repository;
            this._mapper = mapper;
            this._logger = logger;
        }

        public async Task<Unit> Handle(DeleteOrderCommand request, CancellationToken cancellationToken)
        {
            var order4Delete = await this._repository.GetByIdAsync(request.Id);
            if (order4Delete == null)
            {
                this._logger.LogError("Order Not Found!");
                throw new NotFoundException(nameof(Order), request.Id);
            }
            else
            {
                await this._repository.DeleteAsync(order4Delete);
                this._logger.LogInformation("Order Deleted!");
            }
            return Unit.Value;
        }
    }
}

using AutoMapper;
using Discount.Grpc.Entities;
using Discount.Grpc.Protos;
using Discount.Grpc.Repositories;
using Grpc.Core;

namespace Discount.Grpc.Services
{
    public class DiscountService : DiscountProtoService.DiscountProtoServiceBase
    {
        private readonly IDiscountRepository _repository;
        private readonly ILogger<DiscountService> _logger;
        private readonly IMapper _mapper;
        public DiscountService(IDiscountRepository discountRepository, ILogger<DiscountService> logger, IMapper mapper)
        {
            this._repository = discountRepository;
            this._logger = logger;
            this._mapper = mapper;
        }

        public override async Task<CouponModel> GetDiscount(GetDiscountRequest request, ServerCallContext context)
        {
            var coupon = await this._repository.GetDiscount(request.ProductName);
            if (coupon == null)
            {
                this._logger.LogError("Error in Getting Discount !!!!");
                throw new RpcException(new Status(StatusCode.NotFound, $"Not Found!"));
            }
            return this._mapper.Map<CouponModel>(coupon);
        }

        public override async Task<CouponModel> CreateDiscount(CreateDiscountRequest request, ServerCallContext context)
        {
            var stat = await this._repository.CreateDiscount(this._mapper.Map<Coupon>(request.Coupon));
            if (!stat)
            {
                this._logger.LogError("Error In Creating Discount !!!!");
                throw new RpcException(new Status(StatusCode.Internal, $"Error Occured!"));
            }
            this._logger.LogInformation("discount is Successfully Created!");
            return request.Coupon;
        }

        public override async Task<CouponModel> UpdateDiscount(UpdateDiscountRequest request, ServerCallContext context)
        {
            var stat = await this._repository.UpdateDiscount(this._mapper.Map<Coupon>(request.Coupon));
            if (!stat)
            {
                this._logger.LogError("Error In Updating Discount !!!!");
                throw new RpcException(new Status(StatusCode.Internal, $"Error Occured!"));
            }
            this._logger.LogInformation("discount is Successfully Updated!");
            return request.Coupon;
        }

        public override async Task<DeleteDiscountResponse> DeleteDiscount(DeleteDiscountRequest request, ServerCallContext context)
        {
            var stat = await this._repository.DeleteDiscount(request.ProductName);
            if (!stat)
            {
                this._logger.LogError("Error In Deleting Discount !!!!");
                throw new RpcException(new Status(StatusCode.Internal, $"Error Occured!"));
            }
            this._logger.LogInformation("discount is Successfully Deleted!");
            return new DeleteDiscountResponse
            {
                Success = stat
            };
        }
    }
}

using FluentValidation;

namespace Ordering.Application.Features.Orders.Commands.CheckoutOrder
{
    public class CheckoutOrderCommandValidation: AbstractValidator<CheckoutOrderCommand>
    {
        public CheckoutOrderCommandValidation()
        {
            RuleFor(a => a.UserName).NotEmpty().WithMessage("{UserName} is Required!")
                .NotNull().MaximumLength(50).WithMessage("{UserName} Must not Exceed 50 Chars!");

            RuleFor(a => a.EmailAddress).NotEmpty().WithMessage("{EmailAddress} is required");

            RuleFor(a => a.TotalPrice).NotEmpty().WithMessage("{TotalPrice} is required")
                .GreaterThan(0).WithMessage("{TotalPrice} should be greater than 0");
        }
    }
}

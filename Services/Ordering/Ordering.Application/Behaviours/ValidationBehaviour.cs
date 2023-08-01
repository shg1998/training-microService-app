using FluentValidation;
using MediatR;

namespace Ordering.Application.Behaviours
{
    // this is preprocessor behaviours :)
    public class ValidationBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    {
        private readonly IEnumerable<IValidator<TRequest>> _validators;

        public ValidationBehaviour(IEnumerable<IValidator<TRequest>> validators)
        {
            this._validators = validators;
        }

        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            if (this._validators.Any())
            {
                var context = new ValidationContext<TRequest>(request);
                var validationResults = await Task.WhenAll(this._validators.Select(x => x.ValidateAsync(context, cancellationToken)));
                var failures = validationResults.SelectMany(s => s.Errors).Where(a => a != null).ToList();
                if (failures.Count > 0)
                    throw new Exceptions.ValidationException(failures);
            }
            return await next();
        }
    }
}

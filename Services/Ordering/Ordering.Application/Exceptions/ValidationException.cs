using FluentValidation.Results;

namespace Ordering.Application.Exceptions
{
    public class ValidationException : ApplicationException
    {
        public IDictionary<string, string[]> Errors { get; set; }

        public ValidationException() : base("One or more validation failures have been occured !")
        {
            this.Errors = new Dictionary<string, string[]>();
        }

        public ValidationException(IEnumerable<ValidationFailure> failures) : this()
        {
            this.Errors = failures
                .GroupBy(c => c.PropertyName, e => e.ErrorMessage)
                .ToDictionary(failuresG => failuresG.Key, failuresG => failuresG.ToArray());
        }
    }
}

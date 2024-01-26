using FluentValidation;
using MediatR;

namespace eHandbook.modules.ManualManagement.CoreDomain.Validations.PipeLineBehaviorValidation
{
    public class ValidatorPipelineBehaviorBase<TRequest, TResponse>
    {
        private readonly IEnumerable<IValidator<TRequest>> _validators;

        public Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            // Invoke the validators
            var failures = _validators
                .Select(validator => validator.Validate(request))
                .SelectMany(result => result.Errors)
                .ToArray();

            if (failures.Length > 0)
            {
                // Map the validation failures and throw an error,
                // this stops the execution of the request
                var errors = failures
                    .GroupBy(x => x.PropertyName)
                    .ToDictionary(k => k.Key, v => v.Select(x => x.ErrorMessage).ToArray());
                throw new InputValidationException(errors);
            }

            // Invoke the next handler
            // (can be another pipeline behavior or the request handler)
            return next();
        }
    }
}
using eHandbook.Infrastructure.Utilities.Validations;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;

namespace eHandbook.Infrastructure.Utilities.Behaviours
{
    /// <summary>
    /// Adding Validation behavior for automatic validation of incoming commands and queries request before they reach the corresponding handlers.
    /// The pipeline behavior is a wrapper around a request instance and gives you a lot of flexibility with how you can implement it. Pipeline behaviors are a good fit for cross-cutting concerns in our application. 
    /// Good examples of cross-cutting concerns are logging, caching, and  validation.
    /// </summary>
    /// <typeparam name="TRequest"></typeparam>
    /// <typeparam name="TResponse"></typeparam>
    public class ValidatorMediatRPipelineBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
    {
        private readonly IEnumerable<IValidator<TRequest>> _validators;
        private readonly ILogger<ValidatorMediatRPipelineBehavior<TRequest, TResponse>> _logger;

        public ValidatorMediatRPipelineBehavior(IEnumerable<IValidator<TRequest>> validators, ILogger<ValidatorMediatRPipelineBehavior<TRequest, TResponse>> logger)
        {
            _validators = validators;
            _logger = logger;
        }

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"[ VALIDATORPIPELINEBEHAVIOUR -> BEFORE INVOKE NEXT() DELEGATE ] : Handling Request: {typeof(TRequest).Name}\".");

            if (_validators.Any())
            {

                // Invoke the validators
                var context = new ValidationContext<TRequest>(request);
                var validationFailuresResult = await Task.WhenAll(
                    _validators.Select(v => v.ValidateAsync(context, cancellationToken)));

                var Errors = validationFailuresResult
                    .Where(validationResult => !validationResult.IsValid)
                    .SelectMany(validationResult => validationResult.Errors)
                    .ToList();
                if (Errors != null && Errors.Any())
                {

                    //var errors = Errors.GroupBy(x => x.PropertyName)
                    //    .ToDictionary(k => k.Key, v => v.Select(x => x.ErrorMessage).ToList());
                    _logger.LogInformation($"{Errors}");
                    throw new MyCustomInputValidationException(Errors);

                }

            }

            // If the validation succeeds Invoke/calls the next handler in the pipeline tru next() delegate.
            // (can be another pipeline behavior or the request handler)
            _logger.LogInformation("[ VALIDATORPIPELINEBEHAVIOUR ] : Invoke next deledate in the PipeLine.");

            return await next();



        }
    }
}
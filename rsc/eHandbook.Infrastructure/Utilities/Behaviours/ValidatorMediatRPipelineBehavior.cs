using FluentValidation;
using MediatR;


namespace eHandbook.Infrastructure.Utilities.Behaviours
{
    public class ValidatorMediatRPipelineBehavior<TRequest, TResponse> : ValidatorMediatRPipelineBehaviorBase<TRequest, TResponse>, IPipelineBehavior<TRequest, TResponse>
    {
        private readonly IEnumerable<IValidator<TRequest>> _validators;

        public ValidatorMediatRPipelineBehavior(IEnumerable<IValidator<TRequest>> validators)
          => _validators = validators;
    }
}

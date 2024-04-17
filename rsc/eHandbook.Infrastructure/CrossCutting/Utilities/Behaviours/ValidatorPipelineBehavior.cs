using FluentValidation;
using MediatR;


namespace eHandbook.Infrastructure.CrossCutting.Utilities.Behaviours
{
    public class ValidatorPipelineBehavior<TRequest, TResponse> : ValidatorPipelineBehaviorBase<TRequest, TResponse>, IPipelineBehavior<TRequest, TResponse>
    {
        private readonly IEnumerable<IValidator<TRequest>> _validators;

        public ValidatorPipelineBehavior(IEnumerable<IValidator<TRequest>> validators)
          => _validators = validators;
    }
}

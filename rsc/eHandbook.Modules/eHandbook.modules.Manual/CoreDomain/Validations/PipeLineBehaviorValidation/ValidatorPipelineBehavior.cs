using FluentValidation;
using MediatR;


namespace eHandbook.modules.ManualManagement.CoreDomain.Validations.PipeLineBehaviorValidation
{
    public class ValidatorPipelineBehavior<TRequest, TResponse> : ValidatorPipelineBehaviorBase<TRequest, TResponse>, IPipelineBehavior<TRequest, TResponse>
    {
        private readonly IEnumerable<IValidator<TRequest>> _validators;

        public ValidatorPipelineBehavior(IEnumerable<IValidator<TRequest>> validators)
          => _validators = validators;
    }
}

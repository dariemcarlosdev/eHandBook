using eHandbook.modules.ManualManagement.Application.CQRS.Queries.GetManual;
using FluentValidation;
using Microsoft.Extensions.Logging;

namespace eHandbook.modules.ManualManagement.CoreDomain.Validations.FluentValidation
{

    /// <summary>
    /// Each validator can contain a whole lot of Strongly Typed validation logic around your models.
    /// As the name suggest the validation style is fluent, meaning you can chain all the validation rules together.
    /// </summary>
    public sealed class GetManualByIdReqQueryValidator : AbstractValidator<GetManualByIdQueryRec>
    {
        public GetManualByIdReqQueryValidator(ILogger<GetManualByIdReqQueryValidator> logger)
        {
            RuleFor(request => request.Id)
            .NotNull()
            .NotEmpty()
            .WithMessage("Validarion Message: It can not be null or empty");
            _logger = logger;
            logger.LogInformation("RequestManualValidator REGISTETED");
        }

        private readonly ILogger<GetManualByIdReqQueryValidator> _logger;



    }

}

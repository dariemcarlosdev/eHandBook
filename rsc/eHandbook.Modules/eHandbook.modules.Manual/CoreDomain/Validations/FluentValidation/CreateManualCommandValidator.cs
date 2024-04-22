using eHandbook.modules.ManualManagement.Application.CQRS.Commands.CreateManual;
using eHandbook.modules.ManualManagement.Application.CQRS.Queries.GetManual;
using FluentValidation;
using Microsoft.Extensions.Logging;

namespace eHandbook.modules.ManualManagement.CoreDomain.Validations.FluentValidation
{

    /// <summary>
    /// Validator for CreateManualCommand request.Each validator can contain a whole lot of Strongly Typed validation logic around your models.
    /// As the name suggest the validation style is fluent, meaning you can chain all the validation rules together.
    /// </summary>
    internal sealed class CreateManualCommandValidator : AbstractValidator<CreateManualCommand>
    {
        private readonly ILogger<CreateManualCommandValidator> _logger;

        public CreateManualCommandValidator(ILogger<CreateManualCommandValidator> logger)
        {

            _logger = logger;
            RuleFor(request => request.manualToCreate.Description)
            .NotNull()
            .NotEmpty()
            .WithMessage("A Descriptions is required.");
            RuleFor(request => request.manualToCreate.Path)
            .NotNull()
            .NotEmpty()
            .WithMessage("A Path is required.");
            // Add more rules as needed


            _logger.LogInformation("REQUEST CREATEMANUALVALIDATOR REGISTETED");
        }

     



    }

}

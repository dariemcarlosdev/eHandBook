using eHandbook.modules.ManualManagement.Application.CQRS.Commands;
using FluentValidation;
using Microsoft.Extensions.Logging;

namespace eHandbook.modules.ManualManagement.CoreDomain.Validations.FluentValidation
{

    /// <summary>
    /// Validator for CreateManualCommand request.Each validator can contain a whole lot of Strongly Typed validation logic around your models.
    /// As the name suggest the validation style is fluent, meaning you can chain all the validation rules together.
    /// </summary>
    public sealed class CreateManualCommandValidator : AbstractValidator<CreateManualCommand>
    {
        private readonly ILogger<CreateManualCommandValidator> _logger;

        public CreateManualCommandValidator(ILogger<CreateManualCommandValidator> logger)
        {
            _logger = logger;


            _logger.LogInformation("[ CREATEMANUALCOMMANDVALIDATOR ] : CALLING FLUENT VALUDATOR CREATEMANUALCOMMANDVALIDATOR.");

            RuleFor(request => request.ManualToCreate.Description)
            .NotNull()
            .WithMessage("A Descriptions is required.")
            .NotEmpty()
            .WithName("Description")
            .OverridePropertyName("Description");

            RuleFor(request => request.ManualToCreate.Path)
            .NotNull()
            .WithMessage("A Path is required.")
            .NotEmpty()
            .WithName("Path")
            .OverridePropertyName("Path");
            // Add more rules as needed



        }





    }

}

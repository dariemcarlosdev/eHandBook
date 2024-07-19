using eHandbook.Infrastructure.Services.ServiceResponder;
using eHandbook.modules.ManualManagement.Application.Abstractions;
using eHandbook.modules.ManualManagement.Application.CQRS.Commands.CreateManual;
using eHandbook.modules.ManualManagement.Application.CQRS.EventPublishNotifications;
using eHandbook.modules.ManualManagement.CoreDomain.DTOs.Manual;
using FluentValidation;
using MediatR;

namespace eHandbook.modules.ManualManagement.Application.CQRS.Handlers
{
    /// <summary>
    /// This Command Handler, receives command CreateManualCommand request, and contains the logic to handle-process the request
    /// and perform the necessary actions(create, validations) or data retrieval.
    /// </summary>
    internal sealed class CreateManualCommandHandler : IRequestHandler<CreateManualCommand, ApiResponseService<ManualDto>>
    {
        //Validation inside Handler.
        // inject the corresponding validator via the constructor using the IValidator<T> interface.
        // This allows us to access the validator instance and validate the command or query by calling the ValidateAsync method.
        // If the validation fails, we throw a ValidationException and provide the validation errors.
        private readonly IValidator<CreateManualCommand> _validator;
        private readonly IManualService _manualServices;
        private readonly IMediator _mediator;


        public CreateManualCommandHandler(IManualService manualServices, IMediator mediator, IValidator<CreateManualCommand> validator)
        {
            _manualServices = manualServices;
            _mediator = mediator;
            _validator = validator;
        }

        public async Task<ApiResponseService<ManualDto>> Handle(CreateManualCommand request, CancellationToken cancellationToken)
        {
            var varlidatorResult = await _validator.ValidateAsync(request);

            if (!varlidatorResult.IsValid)
            {
                throw new ValidationException(varlidatorResult.Errors);
            }

            // Perform Manual creation logic

            var newManual = new ManualToCreateDto(request.ManualToCreate.Description, request.ManualToCreate.Path);

            var result = await _manualServices.AddNewManualAsync(newManual, cancellationToken);

            //Triggering Notifications, pushing manual once saved in db. 
            await _mediator.Publish(new ManualCreatedNotification() { manual = result.Data! });

            return result;
        }
    }
}

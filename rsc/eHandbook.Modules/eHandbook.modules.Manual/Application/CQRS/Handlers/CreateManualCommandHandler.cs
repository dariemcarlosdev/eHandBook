using eHandbook.Infrastructure.Services.ServiceResponder;
using eHandbook.modules.ManualManagement.Application.Abstractions;
using eHandbook.modules.ManualManagement.Application.CQRS.Commands.CreateManual;
using eHandbook.modules.ManualManagement.Application.CQRS.EventPublishNotifications;
using eHandbook.modules.ManualManagement.CoreDomain.DTOs.Manual;
using MediatR;

namespace eHandbook.modules.ManualManagement.Application.CQRS.Handlers
{
    /// <summary>
    /// This Command Handler, receives command CreateManualCommand, and contains the logic to handle the Command and create Manual entity accordingly.
    /// </summary>
    internal sealed class CreateManualCommandHandler : IRequestHandler<CreateManualCommand, ResponderService<ManualDto>>
    {
        private readonly IManualService _manualServices;
        private readonly IMediator _mediator;

        public CreateManualCommandHandler(IManualService manualServices, IMediator mediator)
        {
            _manualServices = manualServices;
            _mediator = mediator;
        }

        public async Task<ResponderService<ManualDto>> Handle(CreateManualCommand request, CancellationToken cancellationToken)
        {
            var newManual = new ManualToCreateDto
            {
                Description = request.manualToCreate.Description,
                Path = request.manualToCreate.Path,
            };

            var result = await _manualServices.AddNewManualAsync(newManual, cancellationToken);

            //Triggering Notifications, pushing manual once saved in db. 
            await _mediator.Publish(new ManualCreatedNotification() { manual = result.Data! });

            return result;
        }
    }
}

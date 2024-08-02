using eHandbook.Infrastructure.Services.ServiceResponder;
using eHandbook.modules.ManualManagement.Application.Abstractions;
using eHandbook.modules.ManualManagement.Application.CQRS.Commands;
using eHandbook.modules.ManualManagement.Application.CQRS.EventPublishNotifications;
using eHandbook.modules.ManualManagement.CoreDomain.DTOs.Manual;
using MediatR;

namespace eHandbook.modules.ManualManagement.Application.CQRS.Handlers
{
    internal sealed class UpdateManualCommandHandler : IRequestHandler<UpdateManualCommand, ApiResponseService<ManualDto>>
    {
        private readonly IMediator _mediator;
        private readonly IManualService _manualService;

        public UpdateManualCommandHandler(IMediator mediator, IManualService manualService)
        {
            _mediator = mediator;
            _manualService = manualService;
        }

        public async Task<ApiResponseService<ManualDto>> Handle(UpdateManualCommand request, CancellationToken cancellationToken)
        {
            ManualToUpdateDto dto = new ManualToUpdateDto(request.manualToUpdate.Id, request.manualToUpdate.Description, request.manualToUpdate.Path);

            var result = await _manualService.UpdateManualAsyn(dto, cancellationToken);

            //Triggering Notifications, pushing manual once saved in db. 
            await _mediator.Publish(new ManualUpdateNotification() { updateResponse = result.MetaData.Message });

            return result;
        }
    }
}

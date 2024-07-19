using eHandbook.Infrastructure.Services.ServiceResponder;
using eHandbook.modules.ManualManagement.Application.Abstractions;
using eHandbook.modules.ManualManagement.Application.CQRS.Commands.DeleteManual;
using eHandbook.modules.ManualManagement.Application.CQRS.EventPublishNotifications;
using eHandbook.modules.ManualManagement.CoreDomain.DTOs.Manual;
using MediatR;

namespace eHandbook.modules.ManualManagement.Application.CQRS.Handlers
{
    internal sealed class DeleteManualCommandHandler : IRequestHandler<DeleteManualCommand, ApiResponseService<string>>
    {
        private readonly IMediator _mediator;
        private readonly IManualService _manualService;

        public DeleteManualCommandHandler(IManualService manualService, IMediator mediator)
        {
            _manualService = manualService;
            _mediator = mediator;
        }


        public async Task<ApiResponseService<string>> Handle(DeleteManualCommand request, CancellationToken cancellationToken)
        {
            ManualToDeleteDto dto = new ManualToDeleteDto(request.manualToDelete.Id, request.manualToDelete.Description, request.manualToDelete.Path);

            var result = await _manualService.DeleteManualAsync(dto, cancellationToken);

            //Triggering Notifications, pushing manual once saved in db. 
            await _mediator.Publish(new ManualDeletedNotification() { deleteResponse = result.MetaData.Message });

            return result;
        }
    }
}

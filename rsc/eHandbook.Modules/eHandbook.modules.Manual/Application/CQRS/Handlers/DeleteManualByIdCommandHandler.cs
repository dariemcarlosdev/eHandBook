using eHandbook.Infrastructure.Services.ServiceResponder;
using eHandbook.modules.ManualManagement.Application.Abstractions;
using eHandbook.modules.ManualManagement.Application.CQRS.Commands.DeleteManualById;
using eHandbook.modules.ManualManagement.Application.CQRS.EventPublishNotifications;
using eHandbook.modules.ManualManagement.CoreDomain.DTOs.Manual;
using MediatR;

namespace eHandbook.modules.ManualManagement.Application.CQRS.Handlers
{
    internal sealed class DeleteManualByIdCommandHandler : IRequestHandler<DeleteManualByIdCommand, ApiResponseService<ManualDto>>
    {
        private readonly IManualService _manualService;
        private readonly IMediator _mediator;

        public DeleteManualByIdCommandHandler(IMediator mediator, IManualService manualService)
        {
            _manualService = manualService;
            _mediator = mediator;
        }
        public async Task<ApiResponseService<ManualDto>> Handle(DeleteManualByIdCommand request, CancellationToken cancellationToken)
        {
            var result = await _manualService.DeleteManualByIdAsync(request.ManualGuid, cancellationToken);

            //Triggering Notifications, pushing manual once saved in db. 
            await _mediator.Publish(new ManualDeletedNotification() { deleteResponse = result.Message }, cancellationToken);

            return result;
        }
    }
}

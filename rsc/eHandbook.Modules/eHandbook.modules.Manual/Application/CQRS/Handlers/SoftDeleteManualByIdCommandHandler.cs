using eHandbook.Infrastructure.Services.ServiceResponder;
using eHandbook.modules.ManualManagement.Application.Abstractions;
using eHandbook.modules.ManualManagement.Application.CQRS.Commands;
using eHandbook.modules.ManualManagement.Application.CQRS.EventPublishNotifications;
using eHandbook.modules.ManualManagement.CoreDomain.DTOs.Manual;
using MediatR;

namespace eHandbook.modules.ManualManagement.Application.CQRS.Handlers
{
    internal class SoftDeleteManualByIdCommandHandler : IRequestHandler<SoftDeleteManualByIdCommand, ApiResponseService<ManualDto>>
    {

        private readonly IMediator _mediator;
        private readonly IManualService _manualService;

        public SoftDeleteManualByIdCommandHandler(IMediator mediator, IManualService manualService)
        {
            _mediator = mediator;
            _manualService = manualService;
        }

        public async Task<ApiResponseService<ManualDto>> Handle(SoftDeleteManualByIdCommand request, CancellationToken cancellationToken)
        {
            var result = await _manualService.SoftDeleteManualByIdAsync(request.ManualGuid, cancellationToken);

            //Triggering Notifications, pushing manual once saved in db. 
            await _mediator.Publish(new ManualDeletedNotification() { deleteResponse = result.MetaData.Message! });

            return result;
        }
    }
}



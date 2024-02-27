using eHandbook.Core.Services.Common.ServiceResponder;
using eHandbook.modules.ManualManagement.Application.Contracts;
using eHandbook.modules.ManualManagement.Application.CQRS.Commands.DeleteManualById;
using eHandbook.modules.ManualManagement.Application.CQRS.EventPublishNotifications;
using eHandbook.modules.ManualManagement.CoreDomain.DTOs.Manual;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eHandbook.modules.ManualManagement.Application.CQRS.Handlers
{
    internal sealed class DeleteManualByIdCommandHandler : IRequestHandler<DeleteManualByIdCommand, ResponderService<ManualDto>>
    {
        private readonly IManualService _manualService;
        private readonly IMediator _mediator;

        public DeleteManualByIdCommandHandler(IMediator mediator, IManualService manualService)
        {
            _manualService = manualService;
            _mediator = mediator;
        }
        public async Task<ResponderService<ManualDto>> Handle(DeleteManualByIdCommand request, CancellationToken cancellationToken)
        {
            var result = await _manualService.DeleteManualByIdAsync(request.ManualGuid);

            //Triggering Notifications, pushing manual once saved in db. 
            await _mediator.Publish(new ManualDeletedNotification() { deleteResponse = result.Message });

            return result;
        }
    }
}

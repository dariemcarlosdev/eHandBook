using eHandbook.Core.Services.Common.ServiceResponder;
using eHandbook.modules.ManualManagement.CoreDomain.DTOs.Manual;
using MediatR;

namespace eHandbook.modules.ManualManagement.Application.CQRS.Commands.UpdateManual
{
    internal sealed record UpdateManualCommand (ManualToUpdateDto manualToUpdate) : IRequest<ResponderService<ManualDto>>; 
    
}

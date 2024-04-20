using eHandbook.Infrastructure.Services.ServiceResponder;
using eHandbook.modules.ManualManagement.CoreDomain.DTOs.Manual;
using MediatR;

namespace eHandbook.modules.ManualManagement.Application.CQRS.Commands.DeleteManualById
{
    internal sealed record DeleteManualByIdCommand(Guid ManualGuid) : IRequest<ResponderService<ManualDto>>;
}

using eHandbook.Infrastructure.Services.ServiceResponder;
using eHandbook.modules.ManualManagement.CoreDomain.DTOs.Manual;
using MediatR;

namespace eHandbook.modules.ManualManagement.Application.CQRS.Commands.SoftDeleteManualById
{
    internal sealed record SoftDeleteManualByIdCommand(Guid ManualGuid) : IRequest<ResponderService<ManualDto>>;

}

using eHandbook.Infrastructure.Services.ServiceResponder;
using eHandbook.modules.ManualManagement.CoreDomain.DTOs.Manual;
using MediatR;

namespace eHandbook.modules.ManualManagement.Application.CQRS.Commands.DeleteManual
{
    internal sealed record DeleteManualCommand(ManualToDeleteDto manualToDelete) : IRequest<ApiResponseService<string>>;

}

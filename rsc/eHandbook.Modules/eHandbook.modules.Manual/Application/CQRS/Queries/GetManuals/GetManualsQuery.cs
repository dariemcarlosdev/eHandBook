using eHandbook.Infrastructure.CrossCutting.Services.ServiceResponder;
using eHandbook.modules.ManualManagement.CoreDomain.DTOs.Manual;
using MediatR;

namespace eHandbook.modules.ManualManagement.Application.CQRS.Queries.GetManuals
{
    public sealed record GetManualsQuery() : IRequest<ResponderService<IEnumerable<ManualDto>>>;

}

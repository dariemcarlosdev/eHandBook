using eHandbook.Core.Services.Common.ServiceResponder;
using eHandbook.modules.ManualManagement.Application.Contracts;
using eHandbook.modules.ManualManagement.Application.CQRS.Queries.GetManuals;
using eHandbook.modules.ManualManagement.CoreDomain.DTOs.Manual;
using MediatR;

namespace eHandbook.modules.ManualManagement.Application.CQRS.Handlers
{
    internal sealed class GetManualsQueryHandler : IRequestHandler<GetManualsQuery, ResponderService<IEnumerable<ManualDto>>>
    {

        private readonly IManualService _manualService;

        public GetManualsQueryHandler(IManualService manualService)
        {
            _manualService = manualService;
        }

        public async Task<ResponderService<IEnumerable<ManualDto>>> Handle(GetManualsQuery request, CancellationToken cancellationToken)
        {
            return await _manualService.GetAllManualsAsync();
        }
    }
}

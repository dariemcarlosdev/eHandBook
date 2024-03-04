using eHandbook.Core.Services.Common.ServiceResponder;
using eHandbook.modules.ManualManagement.Application.Contracts;
using eHandbook.modules.ManualManagement.Application.CQRS.Queries.GetManual;
using eHandbook.modules.ManualManagement.CoreDomain.DTOs.Manual;
using MediatR;

namespace eHandbook.modules.ManualManagement.Application.CQRS.Handlers
{
    /// <summary>
    /// QueryHandler for GetManualById query. A Query Handler is responsible for handling Queries and retrieving data from the system. 
    /// It receives a Query request, performs the necessary operations to fetch the data, and returns the result to the caller.
    /// </summary>
    internal sealed class GetManualByIdQueryHandler : IRequestHandler<GetManualByIdQueryRec, ResponderService<ManualDto>>
    {
        private readonly IManualService _manualServices;

        public GetManualByIdQueryHandler(IManualService manualServices) => _manualServices = manualServices;


        public async Task<ResponderService<ManualDto>> Handle(GetManualByIdQueryRec request, CancellationToken cancellationToken)
        {
            return await _manualServices.GetManualByIdAsync(request.Id, cancellationToken);
        }
    }
}

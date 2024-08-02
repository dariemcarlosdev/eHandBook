using eHandbook.Infrastructure.Abstractions.Handler;
using eHandbook.Infrastructure.Services.ServiceResponder;
using eHandbook.modules.ManualManagement.Application.Abstractions;
using eHandbook.modules.ManualManagement.Application.CQRS.Queries;
using eHandbook.modules.ManualManagement.CoreDomain.DTOs.Manual;

namespace eHandbook.modules.ManualManagement.Application.CQRS.Handlers
{
    /// <summary>
    /// QueryHandler for GetManualById query. A Query Handler is responsible for handling Queries and retrieving data from the system. 
    /// It receives a Query request, performs the necessary operations to fetch the data, and returns the result to the caller.
    /// </summary>
    internal sealed class GetManualByIdQueryHandler : IMyCustomQueryHandler<GetManualByIdQuery, ApiResponseService<ManualDto>>
    {
        private readonly IManualService _manualServices;

        public GetManualByIdQueryHandler(IManualService manualServices) => _manualServices = manualServices;


        public async Task<ApiResponseService<ManualDto>> Handle(GetManualByIdQuery request, CancellationToken cancellationToken)
        {
            return await _manualServices.GetManualByIdAsync(request.ManualId, cancellationToken);
        }
    }
}

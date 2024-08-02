using eHandbook.Infrastructure.Abstractions.Handler;
using eHandbook.Infrastructure.Services.ServiceResponder;
using eHandbook.modules.ManualManagement.Application.Abstractions;
using eHandbook.modules.ManualManagement.Application.CQRS.Queries;
using eHandbook.modules.ManualManagement.CoreDomain.DTOs.Manual;
using Microsoft.Extensions.Logging;
using Sieve.Services;

namespace eHandbook.modules.ManualManagement.Application.CQRS.Handlers
{
    internal sealed class GetManualsQueryHandler : IMyCustomQueryHandler<GetManualsQuery, ApiResponseService<IEnumerable<ManualDto>>>
    {

        private readonly IManualService _manualService;
        private readonly ILogger<GetManualByIdQueryHandler> _logger;
        private readonly ISieveProcessor _sieveProcessor;

        public GetManualsQueryHandler(IManualService manualService, ILogger<GetManualByIdQueryHandler> logger, ISieveProcessor sieveProcessor)
        {
            _manualService = manualService;
            _logger = logger;
            _sieveProcessor = sieveProcessor;
        }

        public async Task<ApiResponseService<IEnumerable<ManualDto>>> Handle(GetManualsQuery request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Calling GetManualQueryHandler.");
            return await _manualService.GetAllManualsAsync(cancellationToken);
        }
    }
}

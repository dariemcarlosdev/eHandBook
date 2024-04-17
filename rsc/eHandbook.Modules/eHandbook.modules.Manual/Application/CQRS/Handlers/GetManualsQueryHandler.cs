using eHandbook.Infrastructure.CrossCutting.Services.ServiceResponder;
using eHandbook.modules.ManualManagement.Application.Abstractions;
using eHandbook.modules.ManualManagement.Application.CQRS.Queries.GetManuals;
using eHandbook.modules.ManualManagement.CoreDomain.DTOs.Manual;
using MediatR;
using Microsoft.Extensions.Logging;

namespace eHandbook.modules.ManualManagement.Application.CQRS.Handlers
{
    internal sealed class GetManualsQueryHandler : IMyCustomQueryHandler<GetManualsQuery, ResponderService<IEnumerable<ManualDto>>>
    {

        private readonly IManualService _manualService;
        private readonly ILogger<GetManualByIdQueryHandler> _logger;

        public GetManualsQueryHandler(IManualService manualService, ILogger<GetManualByIdQueryHandler> logger)
        {
            _manualService = manualService;
            _logger = logger;
        }

        public async Task<ResponderService<IEnumerable<ManualDto>>> Handle(GetManualsQuery request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Calling GetManualQueryHandler.");
            return await _manualService.GetAllManualsAsync(cancellationToken);
        }
    }
}

using eHandbook.modules.ManualManagement.Application.Contracts;
using eHandbook.modules.ManualManagement.Application.Service.ServiceResponder;
using eHandbook.modules.ManualManagement.CoreDomain.DTOs.Manual;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eHandbook.modules.ManualManagement.Application.CQRS.Queries.GetManual
{
    /// <summary>
    /// QueryHandler for GetManualById query. A Query Handler is responsible for handling Queries and retrieving data from the system. 
    /// It receives a Query request, performs the necessary operations to fetch the data, and returns the result to the caller.
    /// </summary>
    public class GetManualByIdRecQueryHandler : IRequestHandler<GetManualByIdQueryRec, ServiceResponse<ManualDto>>
    {
        private readonly IManualService _manualServices;

        public GetManualByIdRecQueryHandler(IManualService manualServices) =>  _manualServices = manualServices;
       

        public async Task<ServiceResponse<ManualDto>> Handle(GetManualByIdQueryRec request, CancellationToken cancellationToken)
        {
            return await _manualServices.GetManualByIdAsync(request.Id);
        }
    }
}

using eHandbook.modules.ManualManagement.Application.Contracts;
using eHandbook.modules.ManualManagement.Application.Service.ServiceResponder;
using eHandbook.modules.ManualManagement.CoreDomain.DTOs.Manual;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eHandbook.modules.ManualManagement.Application.CQRS.Commands.CreateManual
{
    /// <summary>
    /// This Command Handler, receives command CreateManualCommand, and contains the logic to handle the Command and create Manual entity accordingly.
    /// </summary>
    public class CreateManualCommandHandler : IRequestHandler<CreateManualCommand, ServiceResponse<ManualDto>>
    {
        private readonly IManualService _manualServices;
        public CreateManualCommandHandler(IManualService manualServices) => _manualServices = manualServices ;

        public async Task<ServiceResponse<ManualDto>> Handle(CreateManualCommand request, CancellationToken cancellationToken)
        {
            var manual = new CreateManualDto
            { 
                Description = request.Description,
                Path = request.Path,
            };

            return await _manualServices.AddNewManualAsync(manual);
        }
    }
}

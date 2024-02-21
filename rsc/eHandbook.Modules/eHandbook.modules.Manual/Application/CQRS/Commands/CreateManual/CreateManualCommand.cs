using eHandbook.Core.Services.Common.ServiceResponder;
using eHandbook.modules.ManualManagement.CoreDomain.DTOs.Manual;
using MediatR;

namespace eHandbook.modules.ManualManagement.Application.CQRS.Commands.CreateManual
{
    /// <summary>
    /// The code defines a Command called CreateManualCommand. It represents the intention to create a Manual entity. The command has fallowing properties: 
    /// Description and Path, which are used to provide the necessary information for creating a Manual. 
    /// It implements the IRequest<CreateManualDto> interface from the MediatR library, indicating that it expects a response of type CreateManualDto once it is handled. 
    /// </summary>
    internal sealed record CreateManualCommand(ManualToCreateDto manualToCreate) : IRequest<ResponderService<ManualDto>>;

}

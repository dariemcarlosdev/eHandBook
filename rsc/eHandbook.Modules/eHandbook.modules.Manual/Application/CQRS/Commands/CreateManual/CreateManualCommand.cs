using eHandbook.modules.ManualManagement.Application.Service.ServiceResponder;
using eHandbook.modules.ManualManagement.CoreDomain.DTOs.Manual;
using MediatR;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eHandbook.modules.ManualManagement.Application.CQRS.Commands.CreateManual
{
    /// <summary>
    /// The code defines a Command called CreateManualCommand. It represents the intention to create a Manual entity. The command has fallowing properties: 
    /// Description and Path, which are used to provide the necessary information for creating a Manual. 
    /// It implements the IRequest<CreateManualDto> interface from the MediatR library, indicating that it expects a response of type CreateManualDto once it is handled. 
    /// </summary>
    public class CreateManualCommand : IRequest<ServiceResponse<ManualDto>>
    {
        [Required(ErrorMessage = "Manual path is required")]
        [MaxLength(150, ErrorMessage = "The Description length must be less than 150 characters.")]
        [MinLength(2, ErrorMessage = "Manual Description Name can not be less than two characters")]
        public string? Description { get; set; }
        [Required(ErrorMessage = "Manual path is required.")]
        public string? Path { get; set; }
    }
}

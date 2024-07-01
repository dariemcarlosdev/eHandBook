using eHandbook.modules.ManualManagement.CoreDomain.EntitiesModels;
using System.ComponentModel.DataAnnotations;

namespace eHandbook.modules.ManualManagement.CoreDomain.DTOs.Manual
{
    //Create Id property type GUID
    //include regular expretion for path validation.
    public record ManualDto(Guid Id,
                            [property: Required(ErrorMessage = "Manual Description is required")] string? Description,
                            [property: Required(ErrorMessage = "Manual Path is required")] string? Path,
                            AuditableDetailsDto? AuditableDetails);
}
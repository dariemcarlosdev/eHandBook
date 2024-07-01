using System.ComponentModel.DataAnnotations;

namespace eHandbook.modules.ManualManagement.CoreDomain.DTOs.Manual
{
    public record ManualToCreateDto([property: Required(ErrorMessage = "Manual Description is required")] string? Description,
                                    [property: Required(ErrorMessage = "Manual Path is required")]string? Path);
}

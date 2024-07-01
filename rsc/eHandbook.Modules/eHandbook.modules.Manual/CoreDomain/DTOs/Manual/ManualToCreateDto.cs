using System.ComponentModel.DataAnnotations;

namespace eHandbook.modules.ManualManagement.CoreDomain.DTOs.Manual
{
    public record ManualToCreateDto([property: Required(ErrorMessage = "Manual Description is required")][property: MinLength(1)][property: MaxLength(150, ErrorMessage = "The Description length must be less than 150 characters.")] string? Description, [property: Required(ErrorMessage = "Manual Path is required")] string? Path);
}

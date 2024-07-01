using System.ComponentModel.DataAnnotations;

namespace eHandbook.modules.ManualManagement.CoreDomain.DTOs.Manual
{
    //targeted the property to make de property as required, and get respond back with an invalid request if prop.is empty.
    public record ManualToDeleteDto([property: Required] Guid Id, [property: Required(ErrorMessage = "Manual Description is required")] string? Description, [property: Required(ErrorMessage = "Manual path is required")] string? Path);
}

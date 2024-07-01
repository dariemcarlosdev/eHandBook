using System.ComponentModel.DataAnnotations;

namespace eHandbook.modules.ManualManagement.CoreDomain.DTOs.Manual
{
    //Convert to Record.
    public class ManualToUpdateDto
    {
        //targeted the property to make de property as required, and get respond back with an invalid request if prop.is empty.
        [property: Required]
        public Guid Id { get; set; }
        [Required(ErrorMessage = "Manual Description is required")]
        public string? Description { get; set; }
        [Required(ErrorMessage = "Manual path is required")]
        public string? Path { get; set; }
    }
}
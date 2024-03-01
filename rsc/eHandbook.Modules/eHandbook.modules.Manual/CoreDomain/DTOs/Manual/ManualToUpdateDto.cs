using System.ComponentModel.DataAnnotations;

namespace eHandbook.modules.ManualManagement.CoreDomain.DTOs.Manual
{
    //Convert to Record.
    public class ManualToUpdateDto
    {
        public Guid Id { get; set; }
        [Required(ErrorMessage = "Manual Description is required")]
        public string? Description { get; set; }
        [Required(ErrorMessage = "Manual path is required")]
        public string? Path { get; set; }
    }
}
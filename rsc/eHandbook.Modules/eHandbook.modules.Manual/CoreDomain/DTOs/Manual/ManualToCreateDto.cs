using System.ComponentModel.DataAnnotations;

namespace eHandbook.modules.ManualManagement.CoreDomain.DTOs.Manual
{
    public class ManualToCreateDto
    {
        [Required(ErrorMessage = "Manual Description is required")]
        [property: MinLength(1)]
        [MaxLength(150, ErrorMessage = "The Description length must be less than 150 characters.")]
        public string? Description { get; set; }
        [Required(ErrorMessage = "Manual Path is required")]
        public string? Path { get; set; }
    }
}

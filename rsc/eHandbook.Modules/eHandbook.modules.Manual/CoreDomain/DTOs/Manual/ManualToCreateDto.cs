using System.ComponentModel.DataAnnotations;

namespace eHandbook.modules.ManualManagement.CoreDomain.DTOs.Manual
{
    public class ManualToCreateDto
    {
        /// <summary>
        /// Data Annotation Validation.
        /// </summary>
        //[Required(ErrorMessage = "Manual path is required")]
        [MaxLength(150, ErrorMessage = "The Description length must be less than 150 characters.")]
        [MinLength(2, ErrorMessage = "Manual Description Name can not be less than two characters")]
        public string? Description { get; set; }
        [Required(ErrorMessage = "Manual path is required.")]
        public string? Path { get; set; }
    }
}

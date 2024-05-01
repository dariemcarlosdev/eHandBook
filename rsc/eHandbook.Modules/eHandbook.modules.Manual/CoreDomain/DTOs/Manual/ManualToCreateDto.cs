using System.ComponentModel.DataAnnotations;

namespace eHandbook.modules.ManualManagement.CoreDomain.DTOs.Manual
{
    public class ManualToCreateDto
    {
        /// <summary>
        /// Data Annotation  Atrributes they are not still worling.
        /// </summary>
        //[property : Required(ErrorMessage = "Manual path is required", AllowEmptyStrings =false)]
        //[DisplayFormat(ConvertEmptyStringToNull = false)]
        [property: MinLength(1)]
        [MaxLength(150, ErrorMessage = "The Description length must be less than 150 characters.")]
        public string? Description { get; set; }
        public string? Path { get; set; }
    }
}

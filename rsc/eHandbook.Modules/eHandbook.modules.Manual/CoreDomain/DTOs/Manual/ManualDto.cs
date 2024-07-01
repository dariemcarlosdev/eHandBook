using eHandbook.modules.ManualManagement.CoreDomain.Entities;
using System.ComponentModel.DataAnnotations;

namespace eHandbook.modules.ManualManagement.CoreDomain.DTOs.Manual
{
    public class ManualDto
    {
        //Create Id property type GUID
        public  Guid Id { get; set; }
        [Required(ErrorMessage = "Manual Description is required")]
        public string? Description { get; set; }
        [Required(ErrorMessage = "Manual Path is required")]
        //include regular expretion for path validation.
        public string? Path { get; set; }
        public AuditableDetailsDto? AuditableDetails { get; set; }
        //public List<CategoryEntity>? Categories { get; set; }
    }
}
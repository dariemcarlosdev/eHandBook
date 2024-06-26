using eHandbook.modules.ManualManagement.CoreDomain.Entities;
using System.ComponentModel.DataAnnotations;

namespace eHandbook.modules.ManualManagement.CoreDomain.DTOs.Manual
{
    public class ManualDto
    {
        [Required]
        public string? Description { get; set; }
        [Required]
        //include regular expretion for path validation.
        public string? Path { get; set; }
        public AuditableDetailsDto? AuditableDetails { get; set; }
        public List<CategoryEntity>? Categories { get; set; }
    }
}
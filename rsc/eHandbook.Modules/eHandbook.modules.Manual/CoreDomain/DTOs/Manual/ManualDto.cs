using eHandbook.modules.ManualManagement.CoreDomain.Entities;
using System.ComponentModel.DataAnnotations;

namespace eHandbook.modules.ManualManagement.CoreDomain.DTOs.Manual
{
    public class ManualDto
    {
        //Taking out Id from ManualDto since this is not revelant for users, for security reasons.
        //public Guid Id { get; set; }
        [Required]
        public string? Description { get; set; }
        [Required]
        //include regular expretion for path validation.
        public string? Path { get; set; }
        internal EntityDetailsDto? AuditableDetails { get; set; }
        internal List<CategoryEntity>? Categories { get; set; }
    }
}
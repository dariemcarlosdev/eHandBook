using eHandbook.modules.ManualManagement.CoreDomain.Entities;
using Sieve.Attributes;
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
        public string? CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
        public string? UpdatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public bool IsUpdated { get; set; }
        public DateTime? DeletedOn { get; set; }
        public string? DeletedBy { get; set; }
        //Soft delete capability
        public bool IsDeleted { get; set; }
        public List<Category> Categories { get; set; }
    }
}
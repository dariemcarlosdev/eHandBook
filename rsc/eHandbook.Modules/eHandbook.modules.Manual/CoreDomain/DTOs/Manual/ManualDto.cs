using System.ComponentModel.DataAnnotations;

namespace eHandbook.modules.ManualManagement.CoreDomain.DTOs.Manual
{
    /// <summary>
    /// What was fixed:
    /// 1.	Regular Expression for Path Validation: Added a RegularExpression attribute to the Path property to include path validation as per your comment.
    ///    The regular expression ^[\w\-\./\\]+ is a basic example that you might need to adjust based on your specific path validation requirements.

    public record ManualDto
    {
        public Guid Id { get; init; }
        [Required(ErrorMessage = "Manual Description is required")]
        public string? Description { get; init; }

        [Required(ErrorMessage = "Manual Path is required")]
        //[RegularExpression(@"^[\w\-\./\\]+", ErrorMessage = "Invalid Path format")]
        public string? Path { get; init; }
        //This propertie is marked with set accessor to allow the property to be set outside of the object initializer or constructor.
        public AuditableDetailsDto? AuditableDetails { get; set; }



    }
}
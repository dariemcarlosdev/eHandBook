using eHandbook.Core.Domain;
using eHandbook.Core.Domain.Abstractions;
using System.ComponentModel.DataAnnotations;

namespace eHandbook.modules.ManualManagement.CoreDomain.Entities
{
    public sealed class ManualEntity : BaseEntity, IAuditableEntity
    {

        //We are using using Data Annotations for our current validations.Implement FluentValidation to perform validation for our model classes in incoming request.

        #region SharedProps.
        //Common props are implemented in BaseEntity Class.
        #endregion

        #region Self-Props.
        [MaxLength(150, ErrorMessage = "The Description length must be less than 50 characters")]
        public string? Description { get; set; }
        public string? Path { get; set; } = string.Empty;

        public List<Category> Categories { get; set; } = new List<Category>();
        #endregion

        #region Audit Capability.
        public string? CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
        public string? UpdatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public bool IsUpdated { get; set; }
        public DateTime? DeletedOn { get; set; }
        public string? DeletedBy { get; set; }

        //Soft delete capability
        public bool IsDeleted { get; set; }
        #endregion

        //Sometimes we might have some properties that we need in our class but we don’t want it as a column inside a table. Example Data Annotations approach:

        //[NotMapped]
        //public int TotalUpdations { get; set; }
    }

}

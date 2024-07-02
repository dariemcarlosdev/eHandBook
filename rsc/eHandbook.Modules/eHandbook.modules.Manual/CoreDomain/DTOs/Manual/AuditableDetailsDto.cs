using eHandbook.Core.Domain.Abstractions;

namespace eHandbook.modules.ManualManagement.CoreDomain.DTOs.Manual
{
    //convert to read-init properties
    /// <summary>
    /// 
    /// </summary>
    public record AuditableDetailsDto
    {
        //adding init accessors to the properties will explicitly mark them as init-only, which allows them to be set during object initialization.
        //init accessors are designed to make objects immutable after initialization(cannot be modified after creation).
        //This resolves the issue of assigning values to them outside of the object initializer or constructor, which is not allowed for init props.
        public string? CreatedBy { get; init; }
        public DateTime? CreatedOn { get; init; }
        public string? UpdatedBy { get; init; }
        public DateTime? UpdatedOn { get; init; }
        public bool IsUpdated { get; init; }
        public DateTime? DeletedOn { get; init; }
        public string? DeletedBy { get; init; }
        public bool IsDeleted { get; init; }
    }
}

namespace eHandbook.Core.Domain.Abstractions
{
    /// <summary>
    /// Change tracking for audit purposes can be accomplished using IAudible class. Any entity() that needs to be change tracked inherits from IAudible interface. 
    /// We override DataContext class SaveChanges and SaveChangesAsync methods to get CreatedBy, CreatedOn, UpdatedBy and UpdatedOn values.
    /// </summary>
    public interface IAuditableEntity : ISoftdeletableEntity
    {
        string? CreatedBy { get; set; }
        DateTime? CreatedOn { get; set; }
        string? UpdatedBy { get; set; }
        DateTime? UpdatedOn { get; set; }
        public bool IsUpdated { get; set; }
        public DateTime? DeletedOn { get; set; }
        public string? DeletedBy { get; set; }


    }
}
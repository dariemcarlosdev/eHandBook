namespace eHandbook.Core.Domain.Contracts
{
    /// <summary>
    /// Change tracking for audit purposes can be accomplished using IAudible class. Any entity() that needs to be change tracked inherits from IAudible interface. 
    /// We override DataContext class SaveChanges and SaveChangesAsync methods to get CreatedBy, CreatedOn, UpdatedBy and UpdatedOn values.
    /// </summary>
    public interface IAuditableEntity
    {
        string? CreatedBy { get; set; }
        DateTime? CreatedOn { get; set; }
        string? UpdatedBy { get; set; }
        DateTime? UpdatedOn { get; set; }
        public bool IsUpdated { get; set; }
        public DateTime? DeletedOn { get; set; }
        public string? DeletedBy { get; set; }
        public bool IsDeleted { get; set; }


        //public DateTime CreatedOn { get; set; } = DateTime.Now;
        //public string CreatedBy { get; set; } = string.Empty;
        //public string? DeletedBy { get; set; } = string.Empty;

        //public bool IsDeleted { get; set; } = false;

        //Props. Not used.

        //public DateTime ModifiedOn { get; set; }

        //public string? ModifiedBy { get; set; } = string.Empty;

        //public bool IsModified { get; set; } = false;

        //public DateTime UpdatedOn { get; set; }

        //public string? UpdatedBy { get; set; } = string.Empty;

        //public bool IsUpdated { get; set; } = false;
    }
}
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.ComponentModel.DataAnnotations.Schema;
using eHandbook.Infrastructure.Persistence.Contracts;

namespace eHandbook.Core.Application.Domain;

    public abstract class BaseEntity
    {
    public int Id { get; set; }
    
    public Guid Guid { get; set; } = Guid.NewGuid();
   
    public DateTime CreatedOn { get; set; } = DateTime.Now;

    public DateTime DeletedOn { get; set; }

    public string? DeletedBy { get; set; } = string.Empty;

    public bool IsDeleted { get; set; } = false;

    public DateTime ModifiedOn { get; set; }

    public string? ModifiedBy { get; set; } = string.Empty;

    public bool IsModified { get; set; } = false;

    public DateTime UpdatedOn { get; set; }

    public string? UpdatedBy { get; set; } = string.Empty;

    public bool IsUpdated { get; set; } = false;

}
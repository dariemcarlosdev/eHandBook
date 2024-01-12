using System.ComponentModel.DataAnnotations.Schema;

namespace eHandbook.Core.Domain.Common;

public abstract class BaseEntity
{


    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }

    //public Guid Guid { get; set; } = Guid.NewGuid();
}
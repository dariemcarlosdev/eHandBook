using System.ComponentModel.DataAnnotations.Schema;

namespace eHandbook.Core.Domain.Common;

public abstract class BaseEntity
{

    //Guid is a struct that will never be null, much like int, DateTime, and other struct values.
    //That means that Id will always have a value, whether the request body includes a value for it or not.If an Id isn't provided, 
    //it will have the default value, which for Guid is a value with all zeros (also exposed as Guid.Empty):00000000-0000-0000-0000-000000000000
    //Because of this, if I add the [Required] attribute to Id achieves nothing; the attribute will always say the Id is valid.
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }

    //public Guid Guid { get; set; } = Guid.NewGuid();
}
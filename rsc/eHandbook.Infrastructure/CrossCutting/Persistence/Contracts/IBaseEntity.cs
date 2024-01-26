using System.ComponentModel.DataAnnotations.Schema;

namespace eHandbook.Infrastructure.CrossCutting.Persistence.Contracts
{
    public interface IBaseEntity<T>
    {
        //EF Core database mapping attribute approach.
        [Column("ManualId")]
        public int Id { get; set; }

        [Column("GUID")]
        public Guid Guid { get; set; }

    }
}

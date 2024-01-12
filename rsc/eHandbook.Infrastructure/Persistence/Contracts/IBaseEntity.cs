using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eHandbook.Infrastructure.Persistence.Contracts
{
    public interface IBaseEntity <T>
    {
        //EF Core database mapping attribute approach.
        [Column("ManualId")]
        public int Id { get; set; }

        [Column("GUID")]
        public Guid Guid { get; set; }

    }
}

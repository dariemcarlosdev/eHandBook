using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eHandbook.Core.Domain.Common
{
    public interface ISoftdeletableEntity
    {
        public bool IsDeleted { get; set; }
    }
}

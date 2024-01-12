using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eHandbook.modules.ManualManagement.CoreDomain.DTOs.Manual
{
    public class DeleteManualDto
    {
        public Guid Id { get; set; }
        public string? Description { get; set; }
        public string? Path { get; set; }

        //Add below other properties as required.
    }
}

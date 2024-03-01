using System.ComponentModel.DataAnnotations;

namespace eHandbook.modules.ManualManagement.CoreDomain.DTOs.Manual
{
    public class ManualToDeleteDto
    {
        //targeted the property to make de property as required, and get respond back with an invalid request if prop.is empty.
        [property : Required]
        public Guid Id { get; set; }
        public string? Description { get; set; }
        public string? Path { get; set; }

        //Add below other properties as required.

        //Dto Record definition.
        //public record ManualToDeleteDto([property : Required] Guid id, string Desciption, string Path) //targeted the property to make de property as required, and get respond back with an invalid request if prop.is empty.
    }
}

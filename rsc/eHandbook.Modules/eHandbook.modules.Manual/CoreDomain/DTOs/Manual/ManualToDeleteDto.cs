namespace eHandbook.modules.ManualManagement.CoreDomain.DTOs.Manual
{
    public class ManualToDeleteDto
    {
        public Guid Id { get; set; }
        public string? Description { get; set; }
        public string? Path { get; set; }

        //Add below other properties as required.
    }
}

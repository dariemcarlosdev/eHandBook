using eHandbook.Core.Domain;

namespace eHandbook.modules.ManualManagement.CoreDomain.Entities
{
    public class Category : BaseEntity
    {
        string CategoryName { get; set; }
        string CategoryDescription { get; set; }

    }
}
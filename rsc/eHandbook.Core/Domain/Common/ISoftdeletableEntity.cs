namespace eHandbook.Core.Domain.Common
{
    public interface ISoftdeletableEntity
    {
        public bool IsDeleted { get; set; }
    }
}

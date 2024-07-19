namespace eHandbook.Core.Domain.Abstractions
{
    public interface ISoftdeletableEntity
    {
        public bool IsDeleted { get; set; }
    }
}

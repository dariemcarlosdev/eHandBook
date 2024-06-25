using eHandbook.Core.Domain.Abstractions;

namespace eHandbook.modules.ManualManagement.CoreDomain.DTOs.Manual
{
    internal class EntityDetailsDto : IAuditableEntity
    {
        public string? CreatedBy { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public DateTime? CreatedOn { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public string? UpdatedBy { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public DateTime? UpdatedOn { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public bool IsUpdated { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public DateTime? DeletedOn { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public string? DeletedBy { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public bool IsDeleted { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    }
}
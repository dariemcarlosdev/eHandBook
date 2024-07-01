using eHandbook.Core.Domain.Abstractions;

namespace eHandbook.modules.ManualManagement.CoreDomain.DTOs.Manual
{
    public record AuditableDetailsDto(string? CreatedBy,
                                      DateTime? CreatedOn,
                                      string? UpdatedBy,
                                      DateTime? UpdatedOn,
                                      bool IsUpdated,
                                      DateTime? DeletedOn,
                                      string? DeletedBy,
                                      bool IsDeleted);
}
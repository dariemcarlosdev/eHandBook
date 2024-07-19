using eHandbook.modules.ManualManagement.CoreDomain.DTOs.Manual;
using Microsoft.Extensions.Options;
using Sieve.Models;
using Sieve.Services;

namespace eHandbook.modules.ManualManagement.Infrastructure.Configuration.FluentAPIs
{
    /// <summary>
    /// Instead of using attributes in our Entity Model Class ManualDto, Sieve provides us the ability to use the Fluent API to mark those properties we want to configure for sorting and filtering.
    /// </summary>
    internal sealed class MyCustomSieveProcessor : SieveProcessor
    {
        public MyCustomSieveProcessor(IOptions<SieveOptions> options) : base(options)
        {
        }

        protected override SievePropertyMapper MapProperties(SievePropertyMapper mapper)
        {
            mapper.Property<ManualDto>(p => p.Description)
                .CanFilter()
                .CanSort();
            mapper.Property<ManualDto>(p => p.Path)
                .CanFilter();
            mapper.Property<ManualDto>(p => p.AuditableDetails.CreatedBy)
                .CanFilter()
                .CanSort();
            mapper.Property<ManualDto>(p => p.AuditableDetails.CreatedOn)
                .CanSort();
            mapper.Property<ManualDto>(p => p.AuditableDetails.UpdatedBy)
                .CanFilter()
                .CanSort();
            mapper.Property<ManualDto>(p => p.AuditableDetails.UpdatedOn)
                .CanSort();
            mapper.Property<ManualDto>(p => p.AuditableDetails.DeletedOn)
                .CanSort();
            mapper.Property<ManualDto>(p => p.AuditableDetails.DeletedBy)
                .CanFilter()
                .CanSort();
            return mapper;
        }
    }
}

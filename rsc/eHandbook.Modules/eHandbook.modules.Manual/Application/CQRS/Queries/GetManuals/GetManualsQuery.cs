using eHandbook.Infrastructure.Abstractions.Caching;
using eHandbook.Infrastructure.Services.ServiceResponder;
using eHandbook.modules.ManualManagement.CoreDomain.DTOs.Manual;

namespace eHandbook.modules.ManualManagement.Application.CQRS.Queries.GetManuals
{
    public sealed record GetManualsQuery() : ICachedQuery<ApiResponseService<IEnumerable<ManualDto>>>
    {
        public string CacheKey => $"manuals-{Guid.NewGuid()}";

        public TimeSpan? Expiration => null;
    }
}

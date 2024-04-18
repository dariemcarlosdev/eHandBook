using eHandbook.Infrastructure.Abstractions.Caching;
using eHandbook.Infrastructure.CrossCutting.Services.ServiceResponder;
using eHandbook.modules.ManualManagement.CoreDomain.DTOs.Manual;
using MediatR;

namespace eHandbook.modules.ManualManagement.Application.CQRS.Queries.GetManuals
{
    public sealed record GetManualsQuery() : ICachedQuery<ResponderService<IEnumerable<ManualDto>>>
    {
        public string CacheKey => $"manuals-{Guid.NewGuid()}";

        public TimeSpan? Expiration => null;
    }
}

using eHandbook.Infrastructure.Abstractions.Caching;
using eHandbook.Infrastructure.Services.ServiceResponder;
using eHandbook.modules.ManualManagement.CoreDomain.DTOs.Manual;

namespace eHandbook.modules.ManualManagement.Application.CQRS.Queries.GetManual
{
    /// <summary>
    /// Query for retrieving the Person by ID. In CQRS, a query represents a request for data or information from the system without causing any changes to the system's state. 
    /// It is used to retrieve data or perform read-only operations.Queries can be designed and optimized specifically for read-intensive operations
    /// ,such as data retrieval, filtering, sorting, and aggregating.
    /// </summary>
    //Before:
    //internal sealed record GetManualByIdQueryRec(Guid Id) : IRequest<ResponderService<ManualDto>>;

    //After Including Query Chaching Implementation
    public sealed record GetManualByIdQuery(Guid ManualId) : ICachedQuery<ApiResponseService<ManualDto>>
    {
        public string CacheKey => $"manual-by-id-{ManualId}";

        public TimeSpan? Expiration => null;
    }
}

using eHandbook.Infrastructure.Abstractions.Caching;
using MediatR;

namespace eHandbook.Infrastructure.Abstractions.Handler
{
    public interface IMyCustomQueryHandler<TQuery, TResponse> : IRequestHandler<TQuery, TResponse>
        where TQuery : ICachedQuery<TResponse>
    {
    }
}
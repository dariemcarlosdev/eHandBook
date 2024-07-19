﻿using eHandbook.Infrastructure.Abstractions.Caching;
using MediatR;
using Microsoft.Extensions.Logging;

namespace eHandbook.Infrastructure.Utilities.Behaviours
{

    /// <summary>
    /// Generic Class to implement Pipeline Behaviour with MediatR to solve caching on CRQS queries. Implement IPipelineBehavior interface from Mediator
    /// that will allow to wrap the request pipeline. Also it has a generic constraint where TRequest is a ICachedQuery. This is where the non-generic interface ICachedQuery
    /// comes in allowing to scope this pipeline behaviuor to only queries that implement ICachedQuery, for other queries in the system this will not run.
    /// </summary>
    /// <typeparam name="TRequest">Generic Argument type TRequest</typeparam>
    /// <typeparam name="TResponse">Generic Argument type TResponse</typeparam>
    internal sealed class QueryCachingMediatRPipelineBehaviour<TRequest, TResponse>
        : IPipelineBehavior<TRequest, TResponse>
        where TRequest : ICachedQuery
    {

        private readonly ICacheService _cacheService;
        private readonly ILogger<QueryCachingMediatRPipelineBehaviour<TRequest, TResponse>> _logger;

        public QueryCachingMediatRPipelineBehaviour(ICacheService cacheService, ILogger<QueryCachingMediatRPipelineBehaviour<TRequest, TResponse>> logger)
        {
            _cacheService = cacheService;
            _logger = logger;
        }

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            _logger.LogInformation("QueryCachingPipeLineBehaviour instanciated and calling Handle method.");
            return await _cacheService.GetOrCreateAsync(
                request.CacheKey,
                 _ => next(), //provide a factory function which is going to be invoking the Delegate, in practice this means execute the Query handler.
                 request.Expiration,
                 cancellationToken);
        }

    }

}

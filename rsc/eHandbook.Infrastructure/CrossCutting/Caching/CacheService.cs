using eHandbook.Infrastructure.Abstractions.Caching;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eHandbook.Infrastructure.CrossCutting.Caching
{
    /// <summary>
    /// I'll be using in memory cache to keep it simple, which is natively available in asp.net core.
    /// Any other caching approach can be used later on such Redis or IDistributed Cache which connect to some sort of distrubited caching service.
    /// </summary>
    internal sealed class CacheService : ICacheService
    {
        private static readonly TimeSpan DefaultExpirationCache = TimeSpan.FromMinutes(5);
        private readonly IMemoryCache _memoryCache;

        public CacheService(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
        }

        /// <summary>
        /// Method that is going to handle my Cache Aside Pattern. 
        /// ref: https://learn.microsoft.com/en-us/azure/architecture/patterns/cache-aside
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="factory"></param>
        /// <param name="expiration"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<T> GetOrCreateAsync<T>(string key, Func<CancellationToken, Task<T>> factory, TimeSpan? expiration = null, CancellationToken cancellationToken = default)
        {
            T? result = await _memoryCache.GetOrCreateAsync(
                key,
                //lambda exp. that takes in a cache entry, to allow to configure expiration.
                cacheEntry => 
                {
                    cacheEntry.SetAbsoluteExpiration(expiration ?? DefaultExpirationCache);

                    return factory(cancellationToken);
                });

            return result;
        }
    }
}

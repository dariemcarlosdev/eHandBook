namespace eHandbook.Infrastructure.Abstractions.Caching
{

    /// <summary>
    /// This Services just expose one method.
    /// </summary>
    public interface ICacheService
    {
        /// <summary>
        /// Asynchronous method with a generic response
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key">Cache key</param>
        /// <param name="factory">Factory function that accept a CancellationToken and return a Task of T called factory</param>
        /// <param name="expiration"></param>
        /// <param name="cancellationToken">to support cancellation</param>
        /// <returns></returns>
        Task<T> GetOrCreateAsync<T>(string key, Func<CancellationToken, Task<T>> factory, TimeSpan? expiration = null, CancellationToken cancellationToken = default);
    }

}
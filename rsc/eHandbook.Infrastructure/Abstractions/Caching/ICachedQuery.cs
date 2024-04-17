namespace eHandbook.Infrastructure.Abstractions.Caching
{
    public interface IMyCachedQuery<TResponse> : ICustomQuery<TResponse>, IMyMarkerCachedQuery;

    /// <summary>
    /// This is a Marker Interface (Interface Empty),A class would implement this interface as metadata to be used for some reason
    /// This Interface will help with the pipeline behavoiur definition.
    /// </summary>
    public interface IMyMarkerCachedQuery
    {
        string CacheKey { get; } //ready-only defined with get accessor.
        TimeSpan? Expiration { get; } //ready-only defined with get accessor.

    }
}

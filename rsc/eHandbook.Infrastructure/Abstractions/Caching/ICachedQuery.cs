namespace eHandbook.Infrastructure.Abstractions.Caching
{
    public interface ICachedQuery<TResponse> : ICustomQuery<TResponse>, ICachedQuery;

    /// <summary>
    /// This is a Marker Interface (Interface Empty, with not method.It acts as a marker or flag for the implementing classes. Can be used to deliver type information at runtime.
    /// A class would implement this interface as metadata to be used for some reason.This Interface will help with the pipeline behavoiur definition.
    /// </summary>
    public interface ICachedQuery
    {
        string CacheKey { get; } //ready-only defined with get accessor.
        TimeSpan? Expiration { get; } //ready-only defined with get accessor.

    }
}

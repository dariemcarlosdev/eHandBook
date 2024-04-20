using MediatR;

namespace eHandbook.Infrastructure.Abstractions.Caching
{
    public interface ICustomQuery<TResponse> : IRequest<TResponse>
    {
    }


}

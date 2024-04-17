using eHandbook.Infrastructure.CrossCutting.Services.ServiceResponder;
using MediatR;

namespace eHandbook.Infrastructure.Abstractions.Caching
{
    public interface ICustomQuery<TResponse> : IRequest<TResponse>
    {
    }


}

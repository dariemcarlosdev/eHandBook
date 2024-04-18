using eHandbook.Infrastructure.Abstractions.Caching;
using eHandbook.Infrastructure.CrossCutting.Services.ServiceResponder;
using MediatR;
using Microsoft.AspNetCore.Authentication.OAuth;
using System.Windows.Input;

namespace eHandbook.Infrastructure.Abstractions.Handler
{
    public interface IMyCustomQueryHandler<TQuery, TResponse> : IRequestHandler<TQuery, TResponse>
        where TQuery : ICachedQuery<TResponse>
    {
    }
}
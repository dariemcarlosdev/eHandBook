using eHandbook.Infrastructure.Abstractions.Caching;
using eHandbook.Infrastructure.CrossCutting.Services.ServiceResponder;
using MediatR;
using Microsoft.AspNetCore.Authentication.OAuth;
using System.Windows.Input;

namespace eHandbook.modules.ManualManagement.Application.CQRS.Handlers
{
    public interface IMyCustomQueryHandler<TQuery, TResponse> : IRequestHandler<TQuery, TResponse>
        where TQuery : IMyCachedQuery<TResponse>
    {
    }
}
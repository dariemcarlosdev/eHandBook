using FluentValidation;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using Microsoft.AspNetCore.Builder;
using System;
using Microsoft.IdentityModel.Tokens;
using Azure.Core;

namespace eHandbook.Infrastructure.Extentions
{
    public static class SharedInfrastructureExtensions
    {
        /// <summary>
        /// Shared Infrastructure Extension Methods.
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddSharedInfraServices(this IServiceCollection services )
        {
            // Adding shared services to the DI container.

            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            //With AddValidatorsFromAssembly(), all the validators defined in the executing assembly will be automatically registered,
            //eliminating the need to manually register each validator. This approach ensures that all validators are available for request validation within our project.
            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());


            return services;
        }
    }
}

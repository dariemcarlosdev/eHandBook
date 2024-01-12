using FluentValidation;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using Microsoft.AspNetCore.Builder;
using System;

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
            //register MediatR and provide default configuration to the constructor.
            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

            //services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ExceptionLoggingBehavior<,>));
            //services.AddTransient(typeof(IPipelineBehavior<,>), typeof(RequestValidationBehavior<,>));

            return services;
        }
    }
}

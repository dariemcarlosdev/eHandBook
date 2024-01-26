using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using MediatR;
using eHandbook.Infrastructure.CrossCutting.Utilities.Behaviours;

namespace eHandbook.Infrastructure.CrossCutting.Extentions
{
    public static class SharedInfrastructureExtensions
    {
        /// <summary>
        /// Shared Infrastructure Extension Methods.
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddSharedInfraServices(this IServiceCollection services)
        {
            // Adding shared services to the DI container.

            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies())
            //With AddValidatorsFromAssembly(), all the validators defined in the executing assembly will be automatically registered,
            //eliminating the need to manually register each validator. This approach ensures that all validators are available for request validation within our project.
            .AddValidatorsFromAssembly(Assembly.GetExecutingAssembly())

            //I'm using the <,> notation to specify the behavior that can be used for any generic type parameters.
            .AddSingleton(typeof(IPipelineBehavior<,>), typeof(LoggingMadiatRPipelineBehaviour<,>));

            return services;
        }
    }
}

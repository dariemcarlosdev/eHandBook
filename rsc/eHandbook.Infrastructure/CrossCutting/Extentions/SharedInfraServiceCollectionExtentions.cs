using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using MediatR;
using eHandbook.Infrastructure.CrossCutting.Utilities.Behaviours;
using Microsoft.AspNetCore.HttpLogging;

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
            //HTTP logging is a new built-in middleware that logs information about HTTP requests and HTTP responses including the headers and entire body.
            /*
             HTTP logging provides logs of:
                HTTP Request information
                Common properties
                Headers
                Body
                HTTP Response information
            To configure the HTTP logging middleware, you can specify HttpLoggingOptions in your call to AddSharedInfraServices()
             */
            services.AddHttpLogging(logging =>
            {
                // Customize HTTP logging here.
                logging.LoggingFields = HttpLoggingFields.All;
                logging.RequestHeaders.Add("My-Request-Header");
                logging.ResponseHeaders.Add("My-Response-Header");
                logging.MediaTypeOptions.AddText("application/javascript");
                logging.RequestBodyLogLimit = 4096;
                logging.ResponseBodyLogLimit = 4096;
            });

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

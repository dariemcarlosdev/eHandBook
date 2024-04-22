using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using MediatR;
using Microsoft.AspNetCore.HttpLogging;
using eHandbook.Infrastructure.Abstractions.Caching;
using eHandbook.Infrastructure.CrossCutting.Caching;
using eHandbook.Infrastructure.Utilities.Behaviours;
using eHandbook.Infrastructure.CrossCutting.Exceptions.Middlewares;

namespace eHandbook.Infrastructure.Extentions
{
    public static class SharedInfrastructureDependencyInjection
    {
        /// <summary>
        /// Shared Infrastructure Extension Methods.
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddSharedInfraDIServiceCollection(this IServiceCollection services)
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

            // (First Component) Registering my Middleware Service for Global Errrors Exception Handeling as a Service, This is cuz we are implementing IMiddleware Interface and
            // at the runtime our middleware is going to be resolved from the IMiddleware factory
            //This cross-cutting concerns can be set in Sharead Infrastructure project.
            services.AddTransient<GlobalExceptionErrorHandlerMiddleware>(); // middleware working

            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies())
            //With AddValidatorsFromAssembly(), all the validators defined in the executing assembly will be automatically registered,
            //eliminating the need to manually register each validator. This approach ensures that all validators are available for request validation within our project.
            .AddValidatorsFromAssembly(Assembly.GetExecutingAssembly())


            //Registering MediatR with Dependency Injection at the application Domain looking for MediatR related object isntances scanning whole application..
            //ref: https://medium.com/@codebob75/mediatr-dependency-injection-net-6-71c42ae7c0dePolished
            //Use RegisterServicesFromAssembly and RegisterServicesFromAssemblies methods if you prefer to specify Assembly instances to the Type instances.For example, the following invocation will scan the whole application for the MediatR-related objects.
            .AddMediatR(cfg =>
            {

                cfg.RegisterServicesFromAssemblies(AppDomain.CurrentDomain.GetAssemblies());
                //Registering Pipeline Behaviour with Mediator Request Pipeline. So, QueryCachingPipelineBehaviour is gonna execute
                //whenever I send a ICachedQuery instance thru the request pipeline.
                cfg.AddOpenBehavior(typeof(QueryCachingMediatRPipelineBehaviour<,>));
            })

            //I'm using the <,> notation to specify the behavior that can be used for any generic type parameters.
            .AddSingleton(typeof(IPipelineBehavior<,>), typeof(LoggingMediatRPipelineBehaviour<,>))

            .AddMemoryCache()
            .AddSingleton<ICacheService, CacheService>();

            return services;
        }
    }
}

using eHandbook.Infrastructure.Abstractions.Caching;
using eHandbook.Infrastructure.CrossCutting.Caching;
using eHandbook.Infrastructure.CrossCutting.ExceptionsHandler.Middlewares;
using eHandbook.Infrastructure.Utilities.Behaviours;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.HttpLogging;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace eHandbook.Infrastructure.Extentions
{
    public static class SharedInfrastructureDependencyInjections
    {
        /// <summary>
        /// Shared Infrastructure Extension Methods.
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection SharedInfraDIRegistration(this IServiceCollection services)
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
            _ = services.AddHttpLogging(logging =>
            {
                // Customize HTTP logging here.
                logging.LoggingFields = HttpLoggingFields.All;
                logging.RequestHeaders.Add("My-Request-Header");
                logging.ResponseHeaders.Add("My-Response-Header");
                logging.MediaTypeOptions.AddText("application/json");
                logging.RequestBodyLogLimit = 4096;
                logging.ResponseBodyLogLimit = 4096;
            })

            // Adding shared services to the DI container.


            //Registering MediatR with Dependency Injection at the application Domain looking for MediatR related object isntances scanning whole application..
            //ref: https://medium.com/@codebob75/mediatr-dependency-injection-net-6-71c42ae7c0dePolished
            //Use RegisterServicesFromAssembly and RegisterServicesFromAssemblies methods if you prefer to specify Assembly instances to the Type instances.For example, the following invocation will scan the whole application for the MediatR-related objects.
            .AddMediatR(cfg =>
             {
                 cfg.RegisterServicesFromAssemblies(AppDomain.CurrentDomain.GetAssemblies());
                 //whenever I send a ICachedQuery instance thru the request pipeline.
                 cfg.AddOpenBehavior(typeof(QueryCachingMediatRPipelineBehaviour<,>));
             })
            //Registering my custom Logging pipeline behavior.I'm using the <,> notation to specify the behavior that can be used for any generic type parameters.Since we need to logging each and every request, we add it with a Transient Scope to the container.
            .AddTransient(typeof(IPipelineBehavior<,>), typeof(LoggingMediatRPipelineBehaviour<,>))
            //Registering my custom Validator pipeline behavior.Since we need to validate each and every request, we add it with a Transient Scope to the container.
            .AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidatorMediatRPipelineBehavior<,>))

            // (First Component) Registering my Middleware Service for Global Errrors Exception Handeling as a Service, This is cuz we are implementing IMiddleware Interface and
            // at the runtime our middleware is going to be resolved from the IMiddleware factory.
            //GlobalExceptionErrorHandlerMiddleware is registered as a transient service, meaning a new instance will be created each time it’s requested from the service container
            .AddTransient<GlobalExceptionErrorHandlerMiddleware>()

            .AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies())

            //With AddValidatorsFromAssembly(), all validators defined in the executing assembly will be automatically registered,
            //eliminating the need to manually register each validator. This approach ensures that all validators are available for request validation within our project.
            .AddValidatorsFromAssembly(Assembly.GetExecutingAssembly())




            //This is a configuration setup in ASP.NET Core for handling model validation errors. It does not suitable for ASP.NET Core Minimal APis approach
            //Summary: this code changes the default behavior of an ASP.NET Core application to throw an exception when model validation fails, allowing you to handle it in a way that suits your application.
            //The exception includes the details of the validation errors. This can be useful for providing more detailed error information to the client..Uncomment it if change to Controller based APIs.

            /*
            .Configure<ApiBehaviorOptions>(options => 
            {
                //The options.InvalidModelStateResponseFactory is typically used in a controller-based API approach in ASP.NET Core. The Minimal API does not support Model Binding.
                //Therefore, you can’t use InvalidModelStateResponseFactory in Minimal API. This is because the Minimal API approach is designed to be more lightweight and doesn’t include some of the features available in the controller-based approach.
                //If you need to handle model validation in a Minimal API, you might need to implement a custom solution.
                options.InvalidModelStateResponseFactory = context =>
                {
                    var problemDetails = new ValidationProblemDetails(context.ModelState);
                    throw new MyCustomInputValidationException(problemDetails.Errors);
                };
            })
            */

            .AddMemoryCache()
            .AddSingleton<ICacheService, CacheService>();

            return services;
        }
    }
}

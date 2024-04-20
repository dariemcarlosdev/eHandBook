using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System.Net;

namespace eHandbook.Infrastructure.Utilities.Filters
{
    /// <summary>
    /// Moving our validation logic into a Custom validation filter according parameter type pased through out URI http request. 
    /// I am using filters to centralise cross-cutting concerns that apply to every request.
    /// I am relying on a convention(signature) that my argument to be validated, is the first argument in my delegate signature.
    /// It works in a similar way to IActionfilters in MVC.So, this is a generic type, ensuring type-safety conversion.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public sealed class MyValidationGenericFilter<T> : IEndpointFilter
    {
        //private readonly ILogger<MyValidationGenericFilter<T>> _logger;

        //public MyValidationGenericFilter(ILogger<MyValidationGenericFilter<T>> logger)
        //{
        //    _logger = logger;
        //}

        /*
two things I have to deal with and  would still like to improve on:

1. We’re having to check the argument types and resolve the appropriate validators on every request
2. We have to duplicate the AddEndpointFilter for each endpoint

Solutions:
- In addition to attaching filters to an endpoint using AddEndpointFilter we can also create an endpoint filter factory MyCustomValidationFilterFactory that determines
at startup, the filters that should be attached.
*/
        /// <summary>
        /// It receives HttpContext and a request Delegate to execute next Middleware.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="next"></param>
        /// <returns></returns>
        public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
        {
            //signature that says that the argument to be validated, must be the first one, in the Delegate Signature of each RouteEndPoint Method(.MapPost, .MapGet..).
            T? argToValidate = context.GetArgument<T>(0);
            IValidator<T>? validator = context.HttpContext.RequestServices.GetService<IValidator<T>>();

            if (validator is not null)
            {
                var validationResult = await validator.ValidateAsync(argToValidate!);
                if (!validationResult.IsValid)
                {
                    return Results.ValidationProblem(validationResult.ToDictionary(),
                        statusCode: (int)HttpStatusCode.UnprocessableEntity);
                }
            }

            // Otherwise invoke the next filter in the pipeline
            return await next.Invoke(context);
        }

    }
}

using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace eHandbook.Infrastructure.Filters
{
    /// <summary>
    /// Custom validation filter according parameter type pased through out URI http request.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class MyCustomEndPointValidationFilter<T> : IEndpointFilter
    {
        public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
        {
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


using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace eHandbook.Infrastructure.Filters
{
    /// <summary>
    /// Filter added to SharedInfrastructorCollectionServices.This class implements the IEndpointFilter interface with my custom async ValueTask InvokeAsync(EndpointFilterInvocationContext context
    /// ,EndpointFilterDelegate next)
    /// </summary>
    public class MyCustomEndPointFilters : IEndpointFilter
    {
        private readonly ILogger<MyCustomEndPointFilters> _logger;

        public MyCustomEndPointFilters(ILogger<MyCustomEndPointFilters> logger) 
        {
            _logger = logger;
        }

        /// <summary>
        /// akes in parameter a delegate which has itself two parameters, the first one a EndpointFilterInvocationContext object, in others words the current HttpContext 
        /// and a delegate EndpointFilterDelegate which correspond to the next chained element (let’s say middleware) to get executed in the ASP.NET Core pipeline.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="next"></param>
        /// <returns></returns>
        public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
        {
            //any logic and operation before the minimal endpoint is executed,
            var Id = context.HttpContext.GetRouteData().Values["ManualId"];
            _logger.LogInformation($"Execution during Http Rquest.AddEndpointFilter before filter using parameter Id: {Id}");
            var result = await next(context);
            //any logic and operation after the minimal endpoint is executed,
            _logger.LogInformation($"Execution during Http Response.AddEndpointFilter after filter using parameter Id: {Id}");
            return result;
        }
    }
}

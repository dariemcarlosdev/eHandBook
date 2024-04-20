using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Logging;

namespace eHandbook.Infrastructure.Utilities.Filters
{
    /// <summary>
    /// Filter added to SharedInfrastructorCollectionServices.
    /// This class implements the IEndpointFilter interface with my custom async ValueTask InvokeAsync(EndpointFilterInvocationContext context
    /// ,EndpointFilterDelegate next)
    /// </summary>
    public class MyFilters : IEndpointFilter
    {
        private readonly ILogger<MyFilters> _logger;

        public MyFilters(ILogger<MyFilters> logger)
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

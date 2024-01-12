using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Text.Json;

namespace eHandbook.api.Middlewares
{
    /// <summary>
    /// Defining Custom MiddleWare by convention for Global Error Handling.
    /// </summary>
    public class GlobalExceptionHandlerMiddleware : IMiddleware
    {
        private readonly ILogger _logger;
        /// <summary>
        /// Ctor.
        /// </summary>
        /// <param name="logger"></param>
        public GlobalExceptionHandlerMiddleware(
            ILogger<GlobalExceptionHandlerMiddleware> logger) => _logger = logger;

        /// <summary>
        /// following converntion inside of the framework defining apublic async method that return a task, have a name InvokeAsync accepting http context argument.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="next" ></param>
        /// <returns></returns>
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
                await next(context);
            }
            catch (HttpRequestException e)
            {
                //Logging exception we catch here.
                _logger.LogError(e, e.Message);

                //changing Response of HTTP Context to internal server error.
                //context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                //context.Response.WriteAsync("Internal Server Error Response:" + e.Message).Wait();

                //Create new problemDeteils instance populates it with some meaninful value serialize this isntance into a Json string and
                //write it to the response body so that it is returned from the API. 

                ProblemDetails problem = new()
                {
                    Status = (int)HttpStatusCode.InternalServerError,
                    Title = "Internal Server Error.",
                    Type = "https://tools.ietf.org/html/rfc7231#section-6.6.1",
                    Detail = "An internar Server Error has occurred."
                };

                string json = JsonSerializer.Serialize(problem);

                context.Response.ContentType = "application/json";

                await context.Response.WriteAsync(json);
            }
        }
    }
}



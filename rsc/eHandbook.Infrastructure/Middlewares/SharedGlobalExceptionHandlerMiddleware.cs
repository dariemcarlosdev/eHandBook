using FluentValidation;
using Hellang.Middleware.ProblemDetails;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Net;
using System.Text.Json;

namespace eHandbook.Infrastructure.Middlewares
{
    /// <summary>
    /// Defining Custom MiddleWare by convention for Global Error Handling.
    /// </summary>
    public class SharedGlobalExceptionHandlerMiddleware : IMiddleware
    {
        private readonly ILogger<SharedGlobalExceptionHandlerMiddleware> _logger;
        /// <summary>
        /// Ctor.
        /// </summary>
        /// <param name="logger"></param>
        public SharedGlobalExceptionHandlerMiddleware(
            ILogger<SharedGlobalExceptionHandlerMiddleware> logger) => _logger = logger;

        /// <summary>
        /// following converntion inside of the framework defining a public async method that return a task, have a name InvokeAsync accepting http context argument.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="next" ></param>
        /// <returns></returns>
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
                // Custom logic to be executed before the next middleware
                // …
                await next(context);
                // Custom logic to be executed after the next middleware
                // …
            }
            catch (Exception e)
            {
                //Logging exception we catch here.
                _logger.LogError(e, "Exception occurred: {Message}", e.Message);

                // Handle the exception and generate a response
                await HandleExceptionAsync(context, e);

            }
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            // Generate an error response based on the exception
            var response = new { error = exception.Message };


            //changing Response of HTTP Context to internal server error.
            //context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            //context.Response.WriteAsync("Internal Server Error Response:" + e.Message).Wait();

            //Create new problemDeteils instance populates it with some meaninful value serialize this isntance into a Json string and
            //write it to the response body so that it is returned from the API. 

            ProblemDetails problemDetail = new()
            {
                Status = (int)HttpStatusCode.InternalServerError,
                Title = "Internal Server Error.",
                Type = "https://tools.ietf.org/html/rfc7231#section-6.6.1",
                Detail = "An internar Server Error has occurred.",
                Instance = exception.Source

            };

            problemDetail.Extensions.Add("Extended Detail", new List<string> { Convert.ToString(exception.Message) });
            

            var payload = JsonConvert.SerializeObject(problemDetail);
            
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            return context.Response.WriteAsync(payload);
        }


    }
}



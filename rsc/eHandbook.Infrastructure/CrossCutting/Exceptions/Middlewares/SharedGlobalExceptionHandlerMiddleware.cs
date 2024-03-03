using FluentValidation;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Net;
using System.Security.Authentication;

namespace eHandbook.Infrastructure.CrossCutting.Exceptions.Middlewares
{
    /// <summary>
    /// Defining Custom Factory MiddleWare by convention for Global Error Handling. The Factory Middleware class will be resolved at runtime from dependency injection
    /// </summary>
    public class GlobalExceptionErrorHandlerMiddleware : IMiddleware
    {
        private readonly ILogger<GlobalExceptionErrorHandlerMiddleware> _logger;
        /// <summary>
        /// Ctor.
        /// </summary>
        /// <param name="logger"></param>
        public GlobalExceptionErrorHandlerMiddleware(
            ILogger<GlobalExceptionErrorHandlerMiddleware> logger) => _logger = logger;

        /// <summary>
        /// following converntion inside of the framework defining a public async method that return a task, have a name InvokeAsync accepting http context argument.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="next" ></param>
        /// <returns></returns>
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            var contextFeature = context.Features.Get<IExceptionHandlerFeature>();
            try
            {
                //var routeValues = context.Request.RouteValues;

                //// Custom logic to be executed before the next middleware
                //if (routeValues.Count == 0)
                //{
                //    throw new Exception();
                //}
                var start = DateTime.UtcNow;
                _logger.LogInformation($"Timing catched by GlobalExceptionErrorHandlerMiddleware sending Request before {context.Request.Path}: {(DateTime.UtcNow - start).TotalMilliseconds} ms");
                await next(context);

                // Custom logic to be executed after the next middleware

                _logger.LogInformation($"Timing catched by GlobalExceptionErrorHandlerMiddleware sending Request after {context.Request.Path}: {(DateTime.UtcNow - start).TotalMilliseconds} ms");



            }
            catch (Exception error)
            {
                //Using ILogger exception we catch here.
                _logger.LogError($"Something went wrong. Exception: {error}, Message: {error.Message}");

                await HandleExceptionAsync(context, error);

            }
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception exception)
        {

            var errorHandlerFeature = context.Features.Get<IExceptionHandlerFeature>();


            //changing Response of HTTP Context to internal server error.
            //context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            //context.Response.WriteAsync("Internal Server Error Response:" + e.Message).Wait();

            //Create new problemDeteils instance populates it with some meaninful value serialize this isntance into a Json string and
            //write it to the response body so that it is returned from the API. 

            ProblemDetails problemDetails = new()
            {

                Title = "Internal Server Error.",
                Type = $"https://example.com/problem-types/{exception.GetType().Name}",
                Detail = $"Something went wrong. An internar Server Error has occurred",
                //Instance = context.Request.Path, //try this to check Instance String result.
                Instance = errorHandlerFeature switch
                {
                    ExceptionHandlerFeature e => e.Path,

                    _ => "unknown"
                },
                // Status = context.Response.StatusCode, //try this to check what status Code is returned in response.
                Status = StatusCodes.Status400BadRequest,
                Extensions =
                {
                    ["trace"] = Activity.Current?.Id ?? context.TraceIdentifier
                }

            };

            problemDetails.Extensions.Add("Extended Details", new List<string> { Convert.ToString(exception.Message) });


            // Handle the exception and generate a response
            switch (exception)
            {
                case BadHttpRequestException e:
                    // custom application error
                    problemDetails.Status = e.StatusCode /*StatusCodes.Status400BadRequest*/;
                    problemDetails.Title = "One or more errors occurred processing the request.";
                    problemDetails.Detail = "Request failed. More information can be found in Extended Details.";
                    problemDetails.Instance = context.Request.Path;
                    break;

                case KeyNotFoundException:
                    // custom application error
                    problemDetails.Status = StatusCodes.Status403Forbidden;
                    problemDetails.Title = "One or more validation errors occurred";
                    problemDetails.Detail = "Request Failed with errors. An operation attempts to retrieve an element from a collection using a key that does not exist in that collection. More information can be found in Extended Details.";
                    problemDetails.Instance = context.Request.Path;
                    break;
                case AuthenticationException:
                    problemDetails.Status = (int)HttpStatusCode.Forbidden;
                    problemDetails.Title = "Authentication Request Failed with errors";
                    problemDetails.Detail = "Authentication failed";
                    problemDetails.Instance = context.Request.Path;
                    break;
                case NotImplementedException:
                    problemDetails.Status = (int)HttpStatusCode.NotImplemented;
                    problemDetails.Title = "Request Failed with errors.";
                    problemDetails.Detail = "One or more errors occurred. A request method or operations were not implemented.";
                    problemDetails.Instance = context.Request.Path;
                    break;
                case ValidationException:
                    // custom application error
                    problemDetails.Status = StatusCodes.Status403Forbidden;
                    problemDetails.Title = "Request Failed with errors.";
                    problemDetails.Detail = $"Request Error. An input value does not match the expected data type, range or pattern of the data field. More information can be found in Extended Details.";
                    problemDetails.Instance = context.Request.Path;
                    break;
                default:
                    // default application error
                    problemDetails.Status = (int)HttpStatusCode.InternalServerError;
                    problemDetails.Instance = context.Request.Path;
                    break;
            }
            var payload = JsonConvert.SerializeObject(problemDetails);

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            return context.Response.WriteAsync(payload);
        }


    }
    //Another Implementation using switch case depending type of exceptions.

    //public class ErrorHandlerMiddleware
    //{
    //    private readonly RequestDelegate _next;
    //    private readonly ILogger<ErrorHandlerMiddleware> _logger;

    //    public ErrorHandlerMiddleware(RequestDelegate next, ILogger<ErrorHandlerMiddleware> logger)
    //    {
    //        _next = next;
    //        _logger = logger;
    //    }

    //    public async Task Invoke(HttpContext context)
    //    {
    //        try
    //        {
    //            await _next(context);
    //        }
    //        catch (Exception error)
    //        {
    //            var response = context.Response;
    //            response.ContentType = "application/json";
    //            var responseModel = new Response<string>() { Succeeded = false, Message = error?.Message };

    //            switch (error)
    //            {
    //                case ApiException:
    //                    // custom application error
    //                    response.StatusCode = (int)HttpStatusCode.BadRequest;
    //                    break;

    //                case ValidationException e:
    //                    // custom application error
    //                    response.StatusCode = (int)HttpStatusCode.BadRequest;
    //                    responseModel.Errors = e.Errors;
    //                    break;

    //                case KeyNotFoundException:
    //                    // not found error
    //                    response.StatusCode = (int)HttpStatusCode.NotFound;
    //                    break;

    //                default:
    //                    // unhandled error
    //                    response.StatusCode = (int)HttpStatusCode.InternalServerError;
    //                    break;
    //            }
    //            // use ILogger to log the exception message
    //            _logger.LogError(error.Message);

    //            var result = JsonSerializer.Serialize(responseModel);

    //            await response.WriteAsync(result);
    //        }
    //    }
    //}

}



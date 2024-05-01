using eHandbook.Infrastructure.Utilities.Validations;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Net.Http.Headers;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Security.Authentication;

namespace eHandbook.Infrastructure.CrossCutting.ExceptionsHandler.Middlewares
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
            ILogger<GlobalExceptionErrorHandlerMiddleware> logger)
        {
            _logger = logger;

        }

        /// <summary>
        /// following converntion inside of the framework defining a public async method that return a task, have a name InvokeAsync accepting http context argument.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="next" ></param>
        /// <returns></returns>
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            var start = DateTime.UtcNow;

            _logger.LogInformation($"[ GLOBALEXCEPTIONHANDLER MIDDLEWARE -> BEFORE ] : Timing catched before invoke from GlobalExceptionErrorHandler Middleware {context.Request.Path}: {(DateTime.UtcNow - start).TotalMilliseconds} ms");

            try
            {
                //Passing the http request to the next piece of middleware in the pipeline. If an exception is thrown in any subsequent middleware, it will be caught in the catch block where you can handle it appropriately.
                await next(context);

                // Custom logic to be executed after the next middleware
                _logger.LogInformation($"[GLOBALEXCEPTIONHANDLER MIDDLEWARE -> AFTER] : Timing catched  after invoke from GlobalExceptionErrorHandlerMiddleware {context.Request.Path}: {(DateTime.UtcNow - start).TotalMilliseconds} ms");
            }
            catch (Exception exceptionError)
            {
                //if an exception is thrown in any subsequent middleware, it's caught and Handle here. You could log the error, return a specific HTTP status code, etc. Using ILogger exception we catch here.
                _logger.LogError($"[GLOBALEXCEPTIONHANDLER MIDDLEWARE] ----> : Handling the exception. Exception: {exceptionError},Exception Message: {exceptionError.Message}");
                await HandleExceptionAsync(context, exceptionError);
            }
        }

        private static async Task HandleExceptionAsync(HttpContext context, Exception error)
        {
            //if an exception is thrown, IExceptionHandlerFeature is retrieved.IExceptionHandlerFeature is a feature interface provided by ASP.NET Core that can be used to handle exceptions.
            //It contains information about the exception, including the Error property which gets the Exception that occurred.
            //IExceptionHandlerFeature NO USED ANYMORE:

            //var errorFeature = context.Features.Get<IExceptionHandlerFeature>();

            var response = context.Response;
            response.ContentType = "application/json";

            //Create new problemDeteils instance populates it with some meaninful value serialize this instance into a Json string and write it to the response body so that it is returned from the API. 
            var problemDetails = new ProblemDetails()
            {
                Title = GetTitle(error),
                //Instance = errorFeature switch
                //{
                //    ExceptionHandlerFeature e => e.Path,
                //    _ => "unknown"
                //},
                Instance = context.Request.Path,
                Type = $"https://example.com/problem-types/{error.GetType().Name}",
                Extensions =
                {
                    ["trace"] = Activity.Current?.Id ?? context?.TraceIdentifier
                }
            };


            // Handle the exception and generate an specific response
            switch (error)
            {
                case BadHttpRequestException e: // HTTP request error
                    problemDetails.Status = e.StatusCode /*StatusCodes.Status400BadRequest*/;
                    problemDetails.Detail = "Request failed. More information can be found in Extended Details.";
                    problemDetails.Title = "One or more errors occurred processing the request.";

                    break;

                case KeyNotFoundException: // element not found error
                    problemDetails.Status = StatusCodes.Status403Forbidden;
                    problemDetails.Detail = "Request Failed with errors. An operation attempts to retrieve an element from a collection using a key that does not exist in that collection. More information can be found in Extended Details.";
                    problemDetails.Title = "One or more validation errors occurred";
                    break;

                case AuthenticationException: // Authentication error
                    problemDetails.Status = (int)HttpStatusCode.Forbidden;
                    problemDetails.Detail = "Authentication failed";
                    problemDetails.Title = "Authentication Request Failed with errors";
                    break;

                case NotImplementedException: //Operation no implemented error
                    problemDetails.Status = (int)HttpStatusCode.NotImplemented;
                    problemDetails.Detail = "Request Failed with errors.";
                    problemDetails.Title = "One or more errors occurred. A request method or operations were not implemented.";
                    break;

                case MyCustomInputValidationException e: //Validation Error
                    var failures = e.Errors.Select(x => new
                    {
                        x.PropertyName,
                        x.ErrorMessage,
                        x.AttemptedValue

                    }) ;
                    //var failures = e.Data.Values;
                    problemDetails.Status = StatusCodes.Status403Forbidden;
                    problemDetails.Detail = "Input values do not match with expected data type, range or pattern of the data fields.";
                    problemDetails.Title = "Request Failed with errors";
                    problemDetails.Extensions["Errors"] = failures;
                    break;
                // unhandled error
                default:
                    problemDetails.Status = StatusCodes.Status400BadRequest;
                    problemDetails.Detail = "Something went wrong. A Server Error has occurred.\"";
                    break;
            }


            var payload = JsonConvert.SerializeObject(problemDetails);


            response.StatusCode = (int)problemDetails.Status;
            response.GetTypedHeaders().CacheControl = new CacheControlHeaderValue()
            {
                NoCache = true,
            };

            await response.WriteAsync(payload);
        }

        private static string GetTitle(Exception exceptionError) =>
            exceptionError switch
            {
                ApplicationException applicationException => applicationException.Message,
                _ => "Internal Server Error"
            };

    }
}



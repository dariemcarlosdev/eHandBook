﻿using MediatR;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.Reflection;
using System.Text.Json;

namespace eHandbook.Infrastructure.Utilities.Behaviours
{
    //Our Loging pipeline behavior is an implementation of IPipelineBehavior<TRequest, TResponse>. It represents a similar pattern to filters in ASP.NET MVC/Web API,
    //or middlewares in asp.net core.Pipeline Behaviors serve as the MediatR library’s middleware, encapsulating the request handling process.
    //Before each request, all pipeline behaviours are called, if there are any, will wrap around the request handling process.
    //Assuming I want to log requests being executed via MediatR. MediatR pipeline behaviors provides functionality to validate or logging logic before and after 
    //your command or query handlers execute.So handler no need to write repeated logic for logging or validation.
    //ref:https://medium.com/@mlkpatel0/net-core-mediatr-with-notification-publish-and-behaviors-469d1433607a#:~:text=MediatR%20pipeline%20behaviors%20were%20introduced,command%20or%20query%20handlers%20execute.

    /// <summary>
    ///  This behaviour class implements the IPipelineBehavior<TRequest, TResponse> interface, and  can operate on any request.
    /// </summary>
    /// <typeparam name="TRequest"></typeparam>
    /// <typeparam name="TResponse"></typeparam>
    internal class LoggingMediatRPipelineBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : notnull, IRequest<TResponse>
        where TResponse : notnull
    {
        private readonly ILogger<LoggingMediatRPipelineBehaviour<TRequest, TResponse>> _logger;

        public LoggingMediatRPipelineBehaviour(ILogger<LoggingMediatRPipelineBehaviour<TRequest, TResponse>> logger) => _logger = logger;

        /// <summary>
        /// Implement the Handle method, logging before and after we call the next() delegate.
        /// </summary>
        /// <param name="request"></param>
        /// <param name="next"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            // Pre-request logic: Start timer
            var timmer = Stopwatch.StartNew();
            TResponse response;

            try
            {
                //request:logic before command or query handers execution. Loging output before next() delegate is called.
                _logger.LogInformation($"[ lOGGINGPIPELINEBEHAVIOUR -> BEFORE ] Handling Request: {typeof(TRequest).Name}");

                Type myType = request.GetType();
                var props = new List<PropertyInfo>(myType.GetProperties());

                foreach (var prop in props)
                {
                    var propValue = prop.GetValue(request, null);
                    _logger.LogInformation($"{prop.Name} : {propValue}");
                }

                try
                {

                    //log output for request data comming in.
                    var requestData = JsonSerializer.Serialize(request);
                    _logger.LogInformation($"lOGGINGPIPELINEBEHAVIOUR ] ----> [DATA] With data: {requestData}");
                }
                catch (Exception)
                {
                    _logger.LogInformation($"[lOGGINGPIPELINEBEHAVIOUR ] ----> [ JSON SERIALIZATION ERROR] Could not serialize the request.");
                }

                // Proceed with the handler
                response = await next();

                _logger.LogInformation($"[ lOGGINGPIPELINEBEHAVIOUR -> AFTER ] Handling Request: {typeof(TRequest).Name}");

            }
            finally
            {
                // Post-request logic: Log elapsed time
                //logic after your command or query handlers execution. Loging output after next() delegate is called.
                timmer.Stop();

                //Log the timing information. Get the execution time for each.
                _logger.LogInformation(
                    $"Handled {typeof(TResponse).Name}; Execution time = {timmer.ElapsedMilliseconds}ms");
            }
            //Response:Log the Response information.
            var responseData = JsonSerializer.Serialize(response);
            _logger.LogInformation($"lOGGINGPIPELINEBEHAVIOUR ] ----> [END] Handeling Response: {responseData}");
            return response;
        }
    }
}
using MediatR;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.Text.Json;

namespace eHandbook.Infrastructure.CrossCutting.Utilities.Behaviours
{
    //Our pipeline behavior is an implementation of IPipelineBehavior<TRequest, TResponse>. It represents a similar pattern to filters in ASP.NET MVC/Web API,
    //or middlewares in asp.net core. Before each request, all pipeline behaviours are called, if there are any, which wrap requests.
    //Assuming I want to log requests being executed via MediatR. MediatR pipeline behaviors provides functionality to validate or logging logic before and after 
    //your command or query handlers execute.So handler no need to write repeated logic for logging or validation.
    //ref:https://medium.com/@mlkpatel0/net-core-mediatr-with-notification-publish-and-behaviors-469d1433607a#:~:text=MediatR%20pipeline%20behaviors%20were%20introduced,command%20or%20query%20handlers%20execute.

    /// <summary>
    ///  This behaviour class implements the IPipelineBehavior<TRequest, TResponse> interface, and  can operate on any request.
    /// </summary>
    /// <typeparam name="TRequest"></typeparam>
    /// <typeparam name="TResponse"></typeparam>
    internal class LoggingMediatRPipelineBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
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


            var stopwatch = Stopwatch.StartNew();
            TResponse response;

            try
            {
                _logger.LogInformation("LoggingMadiatRPipelineBehaviour instanciated and calling Handle method.");
                //logic before command or query handers execution. Loging output before next() delegate is called.
                _logger.LogInformation($"[START] Handling Request: {typeof(TRequest).Name}");

                try
                {
                    //log output for request data comming in.
                    var requestData = JsonSerializer.Serialize(request);
                    _logger.LogInformation($"[DATA] With data: {requestData}");
                }
                catch (Exception)
                {
                    _logger.LogInformation("[Serialization ERROR] Could not serialize the request.");
                }
                response = await next();

            }
            finally
            {
                //logic after your command or query handlers execution. Loging output after next() delegate is called.
                stopwatch.Stop();

                //get the execution time for each
                _logger.LogInformation(
                    $"Handled {typeof(TResponse).Name}; Execution time = {stopwatch.ElapsedMilliseconds}ms");
            }
            _logger.LogInformation($"[END] Handeling Response: {JsonSerializer.Serialize(response)}");
            return response;
        }
    }
}

using MediatR;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.Text.Json;

namespace eHandbook.Infrastructure.CrossCutting.Utilities.Behaviours
{
    //Our pipeline behavior is an implementation of IPipelineBehavior<TRequest, TResponse>. It represents a similar pattern to filters in ASP.NET MVC/Web API,
    //or middlewares in asp.net core. Before each request, all pipeline behaviours are called, if there are any, which wrap requests.
    //Assuming I want to log requests being executed via MediatR.It provides functionality to validate or logging logic before and after 
    //your command or query handlers execute.So handler no need to write repeated logic for logging or validation.

    /// <summary>
    ///  This behaviour class implements the IPipelineBehavior<TRequest, TResponse> interface, and  can operate on any request.
    /// </summary>
    /// <typeparam name="TRequest"></typeparam>
    /// <typeparam name="TResponse"></typeparam>
    internal class LoggingMadiatRPipelineBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    {
        private readonly ILogger<LoggingMadiatRPipelineBehaviour<TRequest, TResponse>> _logger;

        public LoggingMadiatRPipelineBehaviour(ILogger<LoggingMadiatRPipelineBehaviour<TRequest, TResponse>> logger) => _logger = logger;

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

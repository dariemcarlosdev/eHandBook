using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace eHandbook.Infrastructure.Utilities.Middlewares
{
    /// <summary>
    /// Custome Middleware for log http request processing time.
    /// </summary>
    internal sealed class TimingMiddleware
    {
        private readonly ILogger<TimingMiddleware> _logger;
        private readonly RequestDelegate _next;

        /// <summary>
        /// public ctr takes as expecting arguments a logger and a request delagate, this request delegate it's gonna be the nextr call.
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="next"></param>
        public TimingMiddleware(ILogger<TimingMiddleware> logger,
            RequestDelegate next)
        {
            _logger = logger;
            _next = next;
        }
        /// <summary>
        /// Invoke next middleware.
        /// </summary>
        /// <param name="ctx"></param>
        /// <returns></returns>
        public async Task Invoke(HttpContext ctx)
        {
            var start = DateTime.UtcNow;
            _logger.LogInformation($"[TIMING MIDDLEWARE -> GOING FORWARE] ----> Timing sending Request {ctx.Request.Path}: {(DateTime.UtcNow - start).TotalMilliseconds} ms");

            await _next(ctx); // pass the context

            _logger.LogInformation($"[TIMING MIDDLEWARE -> GOING BACKWARD] ----> Timing sending Request {ctx.Request.Path}: {(DateTime.UtcNow - start).TotalMilliseconds} ms");
        }
    }

    /// <summary>
    /// Another implementation if we want to it looks and fields like other pieces of middleware or you are sharing it in a open source poject.
    /// </summary>
    public static class TimingExtensions
    {
        public static IApplicationBuilder UseTiming(this IApplicationBuilder app)
        {
            app.UseMiddleware<TimingMiddleware>();
            return app;
        }

        //public static void AddTiming(this IServiceCollection service) 
        //{  
        //    service.AddTransient<ITiming,SomeTiming>();
        //}

    }
}

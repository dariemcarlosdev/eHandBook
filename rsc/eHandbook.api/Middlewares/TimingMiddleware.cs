namespace eHandbook.api.Middlewares
{
    public class TimingMiddleware
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

        public async Task Invoke(HttpContext ctx) 
        {
            var start = DateTime.UtcNow;
            await _next(ctx); // pass the context
            _logger.LogInformation($"Timing sending Request {ctx.Request.Path}: {(DateTime.UtcNow - start).TotalMilliseconds} ms");
        }
    }
}

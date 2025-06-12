namespace AsyncImage_Fetcher_Service.Adapters.Api.Middleware
{
    public class RequestLoggingMiddleware : IMiddleware
    {
        private readonly ILogger<RequestLoggingMiddleware> _logger;

        public RequestLoggingMiddleware(ILogger<RequestLoggingMiddleware> logger)
            => _logger = logger;

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            _logger.LogInformation("Incoming request: {Method} {Path}", context.Request.Method, context.Request.Path);
            try
            {
                await next(context);
            }
            finally
            {
                _logger.LogInformation("Outgoing response: {StatusCode}", context.Response.StatusCode);
            }
        }
    }
}
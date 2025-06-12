using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Primitives;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace AsyncImage_Fetcher_Service.Adapters.Api.Middleware
{
    public class CorrelationIdMiddleware : IMiddleware
    {
        private const string CorrelationIdHeader = "X-Correlation-ID";
        private readonly ILogger<CorrelationIdMiddleware> _logger;

        public CorrelationIdMiddleware(ILogger<CorrelationIdMiddleware> logger)
        {
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            var correlationId = GetOrSetCorrelationId(context);
            context.Response.OnStarting(() =>
            {
                if (!context.Response.Headers.ContainsKey(CorrelationIdHeader))
                {
                    context.Response.Headers.Add(CorrelationIdHeader, correlationId);
                }
                return Task.CompletedTask;
            });

            using (_logger.BeginScope("{@CorrelationId}", correlationId))
            {
                await next(context);
            }
        }

        private string GetOrSetCorrelationId(HttpContext context)
        {
            if (context.Request.Headers.TryGetValue(CorrelationIdHeader, out StringValues correlationIdValues) && correlationIdValues.Any())
            {
                var id = correlationIdValues.First();
                _logger.LogInformation("Using Correlation ID from header: {CorrelationId}", id);
                return id;
            }

            var newCorrelationId = Guid.NewGuid().ToString();
            _logger.LogInformation("Generated new Correlation ID: {CorrelationId}", newCorrelationId);
            context.Request.Headers.Add(CorrelationIdHeader, newCorrelationId);
            return newCorrelationId;
        }
    }
}


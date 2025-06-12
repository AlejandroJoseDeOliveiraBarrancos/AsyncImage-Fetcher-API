using AsyncImage_Fetcher_Service.Drivers.Requests.Abstractions.Interfaces;
using AsyncImage_Fetcher_Service.Drivers.Requests.Services;
using AsyncImage_Fetcher_Service.Logic.Abstractions.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Polly;
using Polly.Extensions.Http;

namespace AsyncImage_Fetcher_Service.Drivers.Requests
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddRequestsServices(this IServiceCollection services)
        {
            services.AddHttpClient<IHttpRequestService, HttpRequestService>()
                .SetHandlerLifetime(TimeSpan.FromMinutes(5))
                .AddPolicyHandler(GetRetryPolicy())
                .AddPolicyHandler(GetCircuitBreakerPolicy());

            services.AddScoped<IImageRequestService, ImageRequestService>();

            return services;
        }

        private static IAsyncPolicy<HttpResponseMessage> GetRetryPolicy()
        {
            return HttpPolicyExtensions
                .HandleTransientHttpError()
                .OrResult(msg => msg.StatusCode == System.Net.HttpStatusCode.NotFound)
                .WaitAndRetryAsync(3, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)), onRetry: (outcome, timespan, retryAttempt, context) =>
                { });
        }

        private static IAsyncPolicy<HttpResponseMessage> GetCircuitBreakerPolicy()
        {
            return HttpPolicyExtensions
                .HandleTransientHttpError()
                .CircuitBreakerAsync(5, TimeSpan.FromSeconds(30));
        }
    }
}

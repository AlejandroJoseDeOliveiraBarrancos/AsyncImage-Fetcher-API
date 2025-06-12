using AsyncImage_Fetcher_Service.Drivers.Requests.Abstractions.Interfaces;
using Microsoft.Extensions.Logging;

namespace AsyncImage_Fetcher_Service.Drivers.Requests.Services
{
    public class HttpRequestService : IHttpRequestService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<HttpRequestService> _logger;

        public HttpRequestService(HttpClient httpClient, ILogger<HttpRequestService> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }

        public async Task<byte[]> GetAsByteArrayAsync(string url, CancellationToken cancellationToken = default)
        {
            if (!Uri.TryCreate(url, UriKind.Absolute, out var uri))
            {
                _logger.LogError("Invalid URL format: {Url}", url);
                throw new ArgumentException("Invalid URL format.", nameof(url));
            }

            try
            {
                _logger.LogInformation("Fething data from url: {Url}", url);

                var response = await _httpClient.GetAsync(uri, cancellationToken);
                response.EnsureSuccessStatusCode();

                return await response.Content.ReadAsByteArrayAsync(cancellationToken);
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "Http request failed for url: {Url}", url);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to fetch data from url: {Url}", url);
                throw;
            }
        }
    }
}
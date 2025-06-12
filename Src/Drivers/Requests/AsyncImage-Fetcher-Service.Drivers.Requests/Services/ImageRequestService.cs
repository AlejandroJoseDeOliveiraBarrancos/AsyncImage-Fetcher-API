using AsyncImage_Fetcher_Service.Drivers.Requests.Abstractions.Interfaces;
using AsyncImage_Fetcher_Service.Logic.Abstractions.Interfaces;
using AsyncImage_Fetcher_Service.Logic.Images.Dtos;
using Microsoft.Extensions.Logging;

namespace AsyncImage_Fetcher_Service.Drivers.Requests.Services
{
    public class ImageRequestService : IImageRequestService
    {
        private readonly IHttpRequestService _httpRequestService;
        private readonly ILogger<ImageRequestService> _logger;

        public ImageRequestService(IHttpRequestService httpRequestService, ILogger<ImageRequestService> logger)
        {
            _httpRequestService = httpRequestService;
            _logger = logger;
        }

        public async Task<ImageDto> FetchImageAsBase64Async(string imageUrl, CancellationToken cancellationToken = default)
        {
            try
            {
                _logger.LogInformation("Requesting image from url: {ImageUrl}", imageUrl);
                var imageBytes = await _httpRequestService.GetAsByteArrayAsync(imageUrl, cancellationToken);

                if (imageBytes is null || imageBytes.Length == 0)
                {
                    _logger.LogWarning("Image is null or empty for url: {ImageUrl}", imageUrl);
                    return null;
                }

                var base64Image = Convert.ToBase64String(imageBytes);

                return new ImageDto { Base64Content = base64Image };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to process image request for url: {ImageUrl}", imageUrl);
                return null;
            }
        }

        public async Task<byte[]> FetchImageAsByteArrayAsync(string imageUrl, CancellationToken cancellationToken = default)
        {
            try
            {
                _logger.LogInformation("Requesting image bytes from url: {ImageUrl}", imageUrl);
                return await _httpRequestService.GetAsByteArrayAsync(imageUrl, cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to process image byte request for url: {ImageUrl}", imageUrl);
                return null;
            }
        }
    }
}
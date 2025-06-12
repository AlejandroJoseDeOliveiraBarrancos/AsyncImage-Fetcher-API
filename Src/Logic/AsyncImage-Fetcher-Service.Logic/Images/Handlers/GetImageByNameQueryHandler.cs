using AsyncImage_Fetcher_Service.Logic.Abstractions.Interfaces;
using AsyncImage_Fetcher_Service.Logic.Images.Queries;
using Microsoft.Extensions.Logging;

namespace AsyncImage_Fetcher_Service.Logic.Images.Handlers
{
    public class GetImageByNameQueryHandler : IQueryHandler<GetImageByNameQuery, string>
    {
        private readonly ILogger<GetImageByNameQueryHandler> _logger;
        private readonly IImageRepository _imageRepository;

        public GetImageByNameQueryHandler(ILogger<GetImageByNameQueryHandler> logger, IImageRepository imageRepository)
        {
            _logger = logger;
            _imageRepository = imageRepository;
        }

        public async Task<string> HandleAsync(GetImageByNameQuery query, CancellationToken cancellationToken = default)
        {
            _logger.LogInformation("Handling GetImageByNameQuery for image: {ImageName}", query.ImageName);
            var image = await _imageRepository.GetImageByNameAsync(query.ImageName, cancellationToken);

            if (image is null)
            {
                _logger.LogWarning("Image not found: {ImageName}", query.ImageName);
                return null;
            }

            return image.Base64Content;
        }
    }
}
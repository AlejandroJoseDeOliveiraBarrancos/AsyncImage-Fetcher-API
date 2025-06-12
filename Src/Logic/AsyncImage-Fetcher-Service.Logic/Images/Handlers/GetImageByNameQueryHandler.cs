using AsyncImage_Fetcher_Service.Logic.Abstractions.Interfaces;
using AsyncImage_Fetcher_Service.Logic.Images.Queries;
using Microsoft.Extensions.Logging;

namespace AsyncImage_Fetcher_Service.Logic.Images.Handlers
{
    public class GetImageByNameQueryHandler : IQueryHandler<GetImageByNameQuery, string>
    {
        private readonly ILogger<GetImageByNameQueryHandler> _logger;
        private readonly IImageRequestService _imageRequestService;

        public GetImageByNameQueryHandler(ILogger<GetImageByNameQueryHandler> logger, IImageRequestService imageRequestService)
        {
            _logger = logger;
            _imageRequestService = imageRequestService;
        }

        public async Task<string> HandleAsync(GetImageByNameQuery query, CancellationToken cancellationToken = default)
        {
            _logger.LogInformation("Handling GetImageByNameQuery for image url: {ImageUrl}", query.ImageName);

            return "dummyBase64Image";
        }
    }
}
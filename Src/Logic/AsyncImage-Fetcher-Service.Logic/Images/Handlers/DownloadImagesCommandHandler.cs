using AsyncImage_Fetcher_Service.Logic.Abstractions.Interfaces;
using AsyncImage_Fetcher_Service.Logic.Images.Commands;
using AsyncImage_Fetcher_Service.Logic.Images.Dtos;
using AsyncImage_Fetcher_Service.Rules.Abstractions;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace AsyncImage_Fetcher_Service.Logic.Images.Handlers
{
    public sealed class DownloadImagesCommandHandler : ICommandHandler<DownloadImagesCommand, Dictionary<string, string>>
    {
        private readonly ILogger<DownloadImagesCommandHandler> _logger;
        private readonly IImageRequestService _imageRequestService;
        private readonly IImageRepository _imageRepository;
        private readonly IImageNameFactory _imageNameFactory;

        public DownloadImagesCommandHandler(
            ILogger<DownloadImagesCommandHandler> logger,
            IImageRequestService imageRequestService,
            IImageRepository imageRepository,
            IImageNameFactory imageNameFactory)
        {
            _logger = logger;
            _imageRequestService = imageRequestService;
            _imageRepository = imageRepository;
            _imageNameFactory = imageNameFactory;
        }

        public async Task<Dictionary<string, string>> HandleAsync(DownloadImagesCommand command, CancellationToken cancellationToken = default)
        {
            _logger.LogInformation("Handling DownloadImagesCommand for {UrlCount} URLs with a concurrency of {MaxConcurrency}.", command.ImageUrls.Count(), command.MaxDownloadAtOnce);

            var urlAndNameMap = new ConcurrentDictionary<string, string>();
            var parallelOptions = new ParallelOptions
            {
                MaxDegreeOfParallelism = command.MaxDownloadAtOnce,
                CancellationToken = cancellationToken
            };

            await Parallel.ForEachAsync(command.ImageUrls, parallelOptions, async (url, token) =>
            {
                try
                {
                    var downloadedImageBytes = await _imageRequestService.FetchImageAsByteArrayAsync(url, token);
                    if (downloadedImageBytes != null && downloadedImageBytes.Length > 0)
                    {
                        var imageDomainModel = _imageNameFactory.Create(url);
                        _logger.LogInformation("Successfully downloaded image from {Url} as {FileName}", url, imageDomainModel.SanitizedName);

                        var imageToSave = new ImageToSaveDto
                        {
                            FileName = imageDomainModel.SanitizedName,
                            Content = downloadedImageBytes
                        };

                        await _imageRepository.SaveImageAsync(imageToSave, token);
                        urlAndNameMap.TryAdd(url, imageDomainModel.SanitizedName);
                    }
                    else
                    {
                        _logger.LogWarning("Failed to download image from {Url}", url);
                        urlAndNameMap.TryAdd(url, "Failed to download");
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "An error occurred while downloading image from {Url}", url);
                    urlAndNameMap.TryAdd(url, $"Error: {ex.Message}");
                }
            });

            _logger.LogInformation("Finished handling DownloadImagesCommand.");
            return urlAndNameMap.ToDictionary();
        }
    }
}
using AsyncImage_Fetcher_Service.Logic.Abstractions.Interfaces;
using AsyncImage_Fetcher_Service.Logic.Images.Commands;
using Microsoft.Extensions.Logging;

namespace AsyncImage_Fetcher_Service.Logic.Images.Handlers
{
    public class DownloadImagesCommandHandler : ICommandHandler<DownloadImagesCommand>
    {
        private readonly ILogger<DownloadImagesCommandHandler> _logger;
        private readonly IImageRequestService _imageRequestService;

        public DownloadImagesCommandHandler(ILogger<DownloadImagesCommandHandler> logger, IImageRequestService imageRequestService)
        {
            _logger = logger;
            _imageRequestService = imageRequestService;
        }

        public async Task HandleAsync(DownloadImagesCommand command, CancellationToken cancellationToken = default)
        {
            _logger.LogInformation("Handling DownloadImagesCommand for {UrlCount} URLs with a concurrency of {MaxConcurrency}.", command.ImageUrls.Count(), command.MaxDownloadAtOnce);

            var semaphore = new SemaphoreSlim(command.MaxDownloadAtOnce);
            var downloadTasks = new List<Task>();

            foreach (var url in command.ImageUrls)
            {
                await semaphore.WaitAsync(cancellationToken);

                downloadTasks.Add(Task.Run(async () =>
                {
                    try
                    {
                        var image = await _imageRequestService.FetchImageAsBase64Async(url, cancellationToken);
                        if (image != null && !string.IsNullOrEmpty(image.Base64Content))
                        {
                            _logger.LogInformation("Successfully downloaded image from {Url}", url);
                            // Here you could save the file, send it to another service, etc.
                            // For now, we are just logging the success.
                        }
                        else
                        {
                            _logger.LogWarning("Failed to download image from {Url}", url);
                        }
                    }
                    catch (System.Exception ex)
                    {
                        _logger.LogError(ex, "An error occurred while downloading image from {Url}", url);
                    }
                    finally
                    {
                        semaphore.Release();
                    }
                }, cancellationToken));
            }

            await Task.WhenAll(downloadTasks);

            _logger.LogInformation("Finished handling DownloadImagesCommand.");
        }
    }
}
using AsyncImage_Fetcher_Service.Drivers.Data.Abstractions.Interfaces;
using AsyncImage_Fetcher_Service.Logic.Abstractions.Interfaces;
using AsyncImage_Fetcher_Service.Logic.Images.Dtos;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;

namespace AsyncImage_Fetcher_Service.Drivers.Data.Repositories
{
    public class ImageRepository : IImageRepository
    {
        private readonly IFileStorageService _fileStorageService;
        private readonly ILogger<ImageRepository> _logger;

        public ImageRepository(IFileStorageService fileStorageService, ILogger<ImageRepository> logger)
        {
            _fileStorageService = fileStorageService;
            _logger = logger;
        }

        public async Task SaveImageAsync(ImageToSaveDto imageToSave, CancellationToken cancellationToken = default)
        {
            _logger.LogInformation("Saving image: {FileName}", imageToSave.FileName);
            try
            {
                await _fileStorageService.SaveFileAsync(imageToSave.FileName, imageToSave.Content, cancellationToken);
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, "Failed to save image: {FileName}", imageToSave.FileName);
                throw;
            }
        }

        public async Task<ImageDto> GetImageByNameAsync(string imageName, CancellationToken cancellationToken = default)
        {
            _logger.LogInformation("Retrieving image: {ImageName}", imageName);
            try
            {
                var imageBytes = await _fileStorageService.ReadFileAsBytesAsync(imageName, cancellationToken);
                if (imageBytes is null || imageBytes.Length == 0)
                {
                    return null;
                }

                var base64Content = Convert.ToBase64String(imageBytes);
                return new ImageDto { Base64Content = base64Content };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to retrieve image: {ImageName}", imageName);
                throw;
            }
        }
    }
}
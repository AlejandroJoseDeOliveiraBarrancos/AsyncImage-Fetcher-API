using AsyncImage_Fetcher_Service.Drivers.Data.Abstractions.Interfaces;
using Microsoft.Extensions.Logging;

namespace AsyncImage_Fetcher_Service.Drivers.Data.Services
{
    public class LocalFileStorageService : IFileStorageService
    {
        private readonly ILogger<LocalFileStorageService> _logger;
        private readonly string _storagePath;

        public LocalFileStorageService(ILogger<LocalFileStorageService> logger)
        {
            _logger = logger;
            _storagePath = Path.Combine(Directory.GetCurrentDirectory(), "ImageStorage");
            if (!Directory.Exists(_storagePath))
            {
                Directory.CreateDirectory(_storagePath);
            }
        }

        public async Task SaveFileAsync(string fileName, byte[] content, CancellationToken cancellationToken = default)
        {
            var filePath = Path.Combine(_storagePath, fileName);
            _logger.LogInformation("Saving file to: {FilePath}", filePath);

            try
            {
                await File.WriteAllBytesAsync(filePath, content, cancellationToken);
                _logger.LogInformation("Successfully saved file: {FileName}", fileName);
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, "Failed to save file: {FileName}", fileName);
                throw;
            }
        }

        public async Task<byte[]> ReadFileAsBytesAsync(string fileName, CancellationToken cancellationToken = default)
        {
            var filePath = Path.Combine(_storagePath, fileName);
            if (!File.Exists(filePath))
            {
                _logger.LogWarning("File not found: {FilePath}", filePath);
                return null;
            }

            _logger.LogInformation("Reading file from: {FilePath}", filePath);
            try
            {
                return await File.ReadAllBytesAsync(filePath, cancellationToken);
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, "Failed to read file: {FileName}", fileName);
                throw;
            }
        }
    }
}
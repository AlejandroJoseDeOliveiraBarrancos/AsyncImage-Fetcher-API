using AsyncImage_Fetcher_Service.Drivers.Data.Services;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.IO;
using System.Threading.Tasks;
using Xunit;

namespace AsyncImage_Fetcher_Service.Drivers.Data.Test.Services
{
    public class LocalFileStorageServiceTests : IDisposable
    {
        private readonly string _testStoragePath;
        private readonly LocalFileStorageService _fileStorageService;

        public LocalFileStorageServiceTests()
        {
            _testStoragePath = Path.Combine(Path.GetTempPath(), "ImageStorageTest", Guid.NewGuid().ToString());
            Directory.CreateDirectory(_testStoragePath);

            var loggerMock = new Mock<ILogger<LocalFileStorageService>>();
            
            _fileStorageService = new LocalFileStorageService(loggerMock.Object);
            var field = typeof(LocalFileStorageService).GetField("_storagePath", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            field.SetValue(_fileStorageService, _testStoragePath);
        }

        public void Dispose()
        {
            if (Directory.Exists(_testStoragePath))
            {
                Directory.Delete(_testStoragePath, true);
            }
        }

        [Fact]
        public async Task SaveFileAsync_And_ReadFileAsBytesAsync_ShouldWorkCorrectly()
        {
            var fileName = "test.jpg";
            var content = new byte[] { 1, 2, 3, 4, 5 };

            await _fileStorageService.SaveFileAsync(fileName, content);
            var readContent = await _fileStorageService.ReadFileAsBytesAsync(fileName);

            readContent.Should().BeEquivalentTo(content);
        }

        [Fact]
        public async Task ReadFileAsBytesAsync_WhenFileDoesNotExist_ShouldReturnNull()
        {
            var result = await _fileStorageService.ReadFileAsBytesAsync("nonexistent.jpg");

            result.Should().BeNull();
        }
    }
} 
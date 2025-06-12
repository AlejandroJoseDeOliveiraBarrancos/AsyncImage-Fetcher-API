using AsyncImage_Fetcher_Service.Drivers.Data.Abstractions.Interfaces;
using AsyncImage_Fetcher_Service.Drivers.Data.Repositories;
using AsyncImage_Fetcher_Service.Logic.Images.Dtos;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace AsyncImage_Fetcher_Service.Drivers.Data.Test.Repositories
{
    public class ImageRepositoryTests
    {
        private readonly Mock<IFileStorageService> _fileStorageServiceMock;
        private readonly Mock<ILogger<ImageRepository>> _loggerMock;
        private readonly ImageRepository _imageRepository;

        public ImageRepositoryTests()
        {
            _fileStorageServiceMock = new Mock<IFileStorageService>();
            _loggerMock = new Mock<ILogger<ImageRepository>>();
            _imageRepository = new ImageRepository(_fileStorageServiceMock.Object, _loggerMock.Object);
        }

        [Fact]
        public async Task SaveImageAsync_ShouldCallFileStorageService()
        {
            var imageToSave = new ImageToSaveDto { FileName = "test.jpg", Content = new byte[] { 1, 2, 3 } };

            await _imageRepository.SaveImageAsync(imageToSave);

            _fileStorageServiceMock.Verify(s => s.SaveFileAsync(imageToSave.FileName, imageToSave.Content, It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task GetImageByNameAsync_WithExistingImage_ShouldReturnImageDto()
        {
            var imageName = "test.jpg";
            var imageBytes = new byte[] { 1, 2, 3 };
            var expectedBase64 = Convert.ToBase64String(imageBytes);
            _fileStorageServiceMock.Setup(s => s.ReadFileAsBytesAsync(imageName, It.IsAny<CancellationToken>())).ReturnsAsync(imageBytes);

            var result = await _imageRepository.GetImageByNameAsync(imageName);

            result.Should().NotBeNull();
            result.Base64Content.Should().Be(expectedBase64);
        }

        [Fact]
        public async Task GetImageByNameAsync_WithNonExistingImage_ShouldReturnNull()
        {
            var imageName = "notfound.jpg";
            _fileStorageServiceMock.Setup(s => s.ReadFileAsBytesAsync(imageName, It.IsAny<CancellationToken>())).ReturnsAsync((byte[])null);

            var result = await _imageRepository.GetImageByNameAsync(imageName);

            result.Should().BeNull();
        }
    }
} 
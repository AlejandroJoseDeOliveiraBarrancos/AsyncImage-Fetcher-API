using AsyncImage_Fetcher_Service.Logic.Abstractions.Interfaces;
using AsyncImage_Fetcher_Service.Logic.Images.Commands;
using AsyncImage_Fetcher_Service.Logic.Images.Dtos;
using AsyncImage_Fetcher_Service.Logic.Images.Handlers;
using AsyncImage_Fetcher_Service.Rules.Abstractions;
using AsyncImage_Fetcher_Service.Rules.Models;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace AsyncImage_Fetcher_Service.Logic.Test.Images.Handlers
{
    public class DownloadImagesCommandHandlerTests
    {
        private readonly Mock<IImageRequestService> _imageRequestServiceMock;
        private readonly Mock<IImageRepository> _imageRepositoryMock;
        private readonly Mock<IImageNameFactory> _imageNameFactoryMock;
        private readonly Mock<ILogger<DownloadImagesCommandHandler>> _loggerMock;
        private readonly DownloadImagesCommandHandler _handler;

        public DownloadImagesCommandHandlerTests()
        {
            _imageRequestServiceMock = new Mock<IImageRequestService>();
            _imageRepositoryMock = new Mock<IImageRepository>();
            _imageNameFactoryMock = new Mock<IImageNameFactory>();
            _loggerMock = new Mock<ILogger<DownloadImagesCommandHandler>>();

            _handler = new DownloadImagesCommandHandler(
                _loggerMock.Object,
                _imageRequestServiceMock.Object,
                _imageRepositoryMock.Object,
                _imageNameFactoryMock.Object);
        }

        [Fact]
        public async Task HandleAsync_WithValidUrls_DownloadsAndSavesImages()
        {
            var command = new DownloadImagesCommand(new[] { "http://example.com/image1.jpg" }, 1);
            var imageBytes = new byte[] { 1, 2, 3 };
            var imageName = new Image("http://example.com/image1.jpg", "sanitized_name.jpg");

            _imageRequestServiceMock.Setup(s => s.FetchImageAsByteArrayAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(imageBytes);
            _imageNameFactoryMock.Setup(f => f.Create(It.IsAny<string>())).Returns(imageName);

            var result = await _handler.HandleAsync(command);

            result.Should().ContainKey("http://example.com/image1.jpg")
                .WhoseValue.Should().Be("sanitized_name.jpg");
            _imageRepositoryMock.Verify(r => r.SaveImageAsync(It.Is<ImageToSaveDto>(dto => dto.FileName == "sanitized_name.jpg"), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task HandleAsync_WithFailedDownload_ReturnsFailedStatus()
        {
            var command = new DownloadImagesCommand(new[] { "http://example.com/image1.jpg" }, 1);

            _imageRequestServiceMock.Setup(s => s.FetchImageAsByteArrayAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((byte[])null);

            var result = await _handler.HandleAsync(command);

            result.Should().ContainKey("http://example.com/image1.jpg")
                .WhoseValue.Should().Be("Failed to download");
        }

        [Fact]
        public async Task HandleAsync_WithException_ReturnsErrorStatus()
        {
            var command = new DownloadImagesCommand(new[] { "http://example.com/image1.jpg" }, 1);
            var exception = new Exception("Test exception");

            _imageRequestServiceMock.Setup(s => s.FetchImageAsByteArrayAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .ThrowsAsync(exception);

            var result = await _handler.HandleAsync(command);

            result.Should().ContainKey("http://example.com/image1.jpg")
                .WhoseValue.Should().Be($"Error: {exception.Message}");
        }
    }
} 
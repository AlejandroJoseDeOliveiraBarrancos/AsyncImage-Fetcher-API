using AsyncImage_Fetcher_Service.Drivers.Requests.Abstractions.Interfaces;
using AsyncImage_Fetcher_Service.Drivers.Requests.Services;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace AsyncImage_Fetcher_Service.Drivers.Requests.Test.Services
{
    public class ImageRequestServiceTests
    {
        private readonly Mock<IHttpRequestService> _httpRequestServiceMock;
        private readonly Mock<ILogger<ImageRequestService>> _loggerMock;
        private readonly ImageRequestService _imageRequestService;

        public ImageRequestServiceTests()
        {
            _httpRequestServiceMock = new Mock<IHttpRequestService>();
            _loggerMock = new Mock<ILogger<ImageRequestService>>();
            _imageRequestService = new ImageRequestService(_httpRequestServiceMock.Object, _loggerMock.Object);
        }

        [Fact]
        public async Task FetchImageAsBase64Async_WithValidImage_ReturnsBase64String()
        {
            var imageUrl = "http://example.com/image.jpg";
            var imageBytes = new byte[] { 1, 2, 3 };
            var expectedBase64 = Convert.ToBase64String(imageBytes);
            _httpRequestServiceMock.Setup(s => s.GetAsByteArrayAsync(imageUrl, It.IsAny<CancellationToken>())).ReturnsAsync(imageBytes);

            var result = await _imageRequestService.FetchImageAsBase64Async(imageUrl);

            result.Should().NotBeNull();
            result.Base64Content.Should().Be(expectedBase64);
        }

        [Fact]
        public async Task FetchImageAsBase64Async_WithEmptyResponse_ReturnsNull()
        {
            var imageUrl = "http://example.com/empty.jpg";
            _httpRequestServiceMock.Setup(s => s.GetAsByteArrayAsync(imageUrl, It.IsAny<CancellationToken>())).ReturnsAsync(new byte[0]);

            var result = await _imageRequestService.FetchImageAsBase64Async(imageUrl);

            result.Should().BeNull();
        }
        
        [Fact]
        public async Task FetchImageAsByteArrayAsync_WithValidImage_ReturnsByteArray()
        {
            var imageUrl = "http://example.com/image.jpg";
            var imageBytes = new byte[] { 1, 2, 3 };
            _httpRequestServiceMock.Setup(s => s.GetAsByteArrayAsync(imageUrl, It.IsAny<CancellationToken>())).ReturnsAsync(imageBytes);

            var result = await _imageRequestService.FetchImageAsByteArrayAsync(imageUrl);

            result.Should().BeEquivalentTo(imageBytes);
        }

        [Fact]
        public async Task FetchImageAsByteArrayAsync_WithHttpRequestException_ReturnsNull()
        {
            var imageUrl = "http://example.com/image.jpg";
            _httpRequestServiceMock.Setup(s => s.GetAsByteArrayAsync(imageUrl, It.IsAny<CancellationToken>())).ThrowsAsync(new System.Net.Http.HttpRequestException());

            var result = await _imageRequestService.FetchImageAsByteArrayAsync(imageUrl);

            result.Should().BeNull();
        }
    }
} 
using AsyncImage_Fetcher_Service.Logic.Abstractions.Interfaces;
using AsyncImage_Fetcher_Service.Logic.Images.Handlers;
using AsyncImage_Fetcher_Service.Logic.Images.Queries;
using AsyncImage_Fetcher_Service.Logic.Images.Dtos;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace AsyncImage_Fetcher_Service.Logic.Test.Images.Handlers
{
    public class GetImageByNameQueryHandlerTests
    {
        private readonly Mock<IImageRepository> _imageRepositoryMock;
        private readonly Mock<ILogger<GetImageByNameQueryHandler>> _loggerMock;
        private readonly GetImageByNameQueryHandler _handler;

        public GetImageByNameQueryHandlerTests()
        {
            _imageRepositoryMock = new Mock<IImageRepository>();
            _loggerMock = new Mock<ILogger<GetImageByNameQueryHandler>>();
            _handler = new GetImageByNameQueryHandler(_loggerMock.Object, _imageRepositoryMock.Object);
        }

        [Fact]
        public async Task HandleAsync_WithExistingImage_ReturnsBase64Content()
        {
            var query = new GetImageByNameQuery("test.jpg");
            var imageDto = new ImageDto { Base64Content = "test_base64_content" };
            _imageRepositoryMock.Setup(r => r.GetImageByNameAsync(query.ImageName, It.IsAny<CancellationToken>()))
                .ReturnsAsync(imageDto);

            var result = await _handler.HandleAsync(query);

            result.Should().Be(imageDto.Base64Content);
        }

        [Fact]
        public async Task HandleAsync_WithNonExistingImage_ReturnsNull()
        {
            var query = new GetImageByNameQuery("not_found.jpg");
            _imageRepositoryMock.Setup(r => r.GetImageByNameAsync(query.ImageName, It.IsAny<CancellationToken>()))
                .ReturnsAsync((ImageDto)null);

            var result = await _handler.HandleAsync(query);

            result.Should().BeNull();
        }
    }
} 
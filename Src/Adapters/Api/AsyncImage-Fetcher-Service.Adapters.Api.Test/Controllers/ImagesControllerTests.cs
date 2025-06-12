using AsyncImage_Fetcher_Service.Adapters.Api.Contracts.V1;
using AsyncImage_Fetcher_Service.Logic.Abstractions.Interfaces;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using Moq;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Xunit;

namespace AsyncImage_Fetcher_Service.Adapters.Api.Test.Controllers
{
    public class ImagesControllerTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;
        private readonly Mock<ICommandDispatcher> _commandDispatcherMock = new();
        private readonly Mock<IQueryDispatcher> _queryDispatcherMock = new();

        public ImagesControllerTests(WebApplicationFactory<Program> factory)
        {
            _client = factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureTestServices(services =>
                {
                    services.AddSingleton(_commandDispatcherMock.Object);
                    services.AddSingleton(_queryDispatcherMock.Object);
                });
            }).CreateClient();
        }

        [Fact]
        public async Task DownloadImages_WithValidRequest_ReturnsOk()
        {
            var request = new DownloadImagesRequestDto
            {
                ImageUrls = new[] { "http://example.com/image.jpg" },
                MaxDownloadAtOnce = 1
            };
            _commandDispatcherMock
                .Setup(d => d.SendAsync(It.IsAny<Logic.Images.Commands.DownloadImagesCommand>(), default))
                .ReturnsAsync(new Dictionary<string, string> { { "http://example.com/image.jpg", "image.jpg" } });

            var response = await _client.PostAsJsonAsync("/api/v1/images/download-images", request);

            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var content = await response.Content.ReadFromJsonAsync<DownloadImagesResponseDto>();
            content.Success.Should().BeTrue();
        }

        [Fact]
        public async Task GetImageByName_WithExistingImage_ReturnsOk()
        {
            var imageName = "test.jpg";
            var base64Content = "test_content";
            _queryDispatcherMock
                .Setup(d => d.QueryAsync(It.Is<Logic.Images.Queries.GetImageByNameQuery>(q => q.ImageName == imageName), default))
                .ReturnsAsync(base64Content);

            var response = await _client.GetAsync($"/api/v1/images/get-image-by-name/{imageName}");

            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var content = await response.Content.ReadFromJsonAsync<GetImageResponseDto>();
            content.ImageBase64.Should().Be(base64Content);
        }

        [Fact]
        public async Task GetImageByName_WithNonExistingImage_ReturnsNotFound()
        {
            var imageName = "notfound.jpg";
             _queryDispatcherMock
                .Setup(d => d.QueryAsync(It.Is<Logic.Images.Queries.GetImageByNameQuery>(q => q.ImageName == imageName), default))
                .ReturnsAsync((string)null);

            var response = await _client.GetAsync($"/api/v1/images/get-image-by-name/{imageName}");

            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }
    }
} 
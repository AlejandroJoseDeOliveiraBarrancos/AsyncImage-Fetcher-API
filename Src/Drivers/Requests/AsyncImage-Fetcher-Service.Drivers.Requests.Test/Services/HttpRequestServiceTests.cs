using AsyncImage_Fetcher_Service.Drivers.Requests.Services;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using Moq.Protected;
using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace AsyncImage_Fetcher_Service.Drivers.Requests.Test.Services
{
    public class HttpRequestServiceTests
    {
        private readonly Mock<HttpMessageHandler> _httpMessageHandlerMock;
        private readonly HttpClient _httpClient;
        private readonly Mock<ILogger<HttpRequestService>> _loggerMock;
        private readonly HttpRequestService _httpRequestService;

        public HttpRequestServiceTests()
        {
            _httpMessageHandlerMock = new Mock<HttpMessageHandler>();
            _httpClient = new HttpClient(_httpMessageHandlerMock.Object);
            _loggerMock = new Mock<ILogger<HttpRequestService>>();
            _httpRequestService = new HttpRequestService(_httpClient, _loggerMock.Object);
        }

        [Fact]
        public async Task GetAsByteArrayAsync_WithValidUrlAndSuccessResponse_ReturnsByteArray()
        {
            var url = "http://example.com/image.jpg";
            var expectedBytes = new byte[] { 1, 2, 3 };
            var responseMessage = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new ByteArrayContent(expectedBytes)
            };
            
            _httpMessageHandlerMock.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(responseMessage);

            var result = await _httpRequestService.GetAsByteArrayAsync(url);

            result.Should().BeEquivalentTo(expectedBytes);
        }

        [Fact]
        public async Task GetAsByteArrayAsync_WithInvalidUrl_ThrowsArgumentException()
        {
            var url = "not-a-valid-url";

            Func<Task> act = () => _httpRequestService.GetAsByteArrayAsync(url);

            await act.Should().ThrowAsync<ArgumentException>();
        }

        [Fact]
        public async Task GetAsByteArrayAsync_WithFailedResponse_ThrowsHttpRequestException()
        {
            var url = "http://example.com/notfound.jpg";
            var responseMessage = new HttpResponseMessage { StatusCode = HttpStatusCode.NotFound };
            _httpMessageHandlerMock.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(responseMessage);

            Func<Task> act = () => _httpRequestService.GetAsByteArrayAsync(url);
            
            await act.Should().ThrowAsync<HttpRequestException>();
        }
    }
} 
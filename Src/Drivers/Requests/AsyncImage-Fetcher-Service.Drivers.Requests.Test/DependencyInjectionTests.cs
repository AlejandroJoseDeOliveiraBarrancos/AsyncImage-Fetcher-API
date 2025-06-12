using AsyncImage_Fetcher_Service.Drivers.Requests.Abstractions.Interfaces;
using AsyncImage_Fetcher_Service.Logic.Abstractions.Interfaces;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace AsyncImage_Fetcher_Service.Drivers.Requests.Test
{
    public class DependencyInjectionTests
    {
        [Fact]
        public void AddRequestsServices_ShouldRegisterServices()
        {
            var services = new ServiceCollection();

            services.AddRequestsServices();
            var serviceProvider = services.BuildServiceProvider();

            serviceProvider.GetService<IHttpRequestService>().Should().NotBeNull();
            serviceProvider.GetService<IImageRequestService>().Should().NotBeNull();
            
            var httpClientFactory = serviceProvider.GetService<IHttpClientFactory>();
            httpClientFactory.Should().NotBeNull();
            var httpClient = httpClientFactory.CreateClient(nameof(IHttpRequestService));
            httpClient.Should().NotBeNull();

        }
    }
} 
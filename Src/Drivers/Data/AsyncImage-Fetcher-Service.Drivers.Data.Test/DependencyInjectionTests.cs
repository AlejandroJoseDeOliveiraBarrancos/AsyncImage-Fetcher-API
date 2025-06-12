using AsyncImage_Fetcher_Service.Drivers.Data.Abstractions.Interfaces;
using AsyncImage_Fetcher_Service.Logic.Abstractions.Interfaces;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace AsyncImage_Fetcher_Service.Drivers.Data.Test
{
    public class DependencyInjectionTests
    {
        [Fact]
        public void AddDataServices_ShouldRegisterServices()
        {
            var services = new ServiceCollection();

            services.AddDataServices();
            var serviceProvider = services.BuildServiceProvider();

            serviceProvider.GetService<IFileStorageService>().Should().NotBeNull();
            serviceProvider.GetService<IImageRepository>().Should().NotBeNull();
        }
    }
} 
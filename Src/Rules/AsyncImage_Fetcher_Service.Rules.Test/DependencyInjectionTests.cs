using AsyncImage_Fetcher_Service.Rules.Abstractions;
using AsyncImage_Fetcher_Service.Rules.Factories;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace AsyncImage_Fetcher_Service.Rules.Test
{
    public class DependencyInjectionTests
    {
        [Fact]
        public void AddRulesServices_ShouldRegisterServices()
        {
            var services = new ServiceCollection();

            services.AddRulesServices();
            var serviceProvider = services.BuildServiceProvider();

            var imageNameFactory = serviceProvider.GetService<IImageNameFactory>();
            imageNameFactory.Should().NotBeNull();
            imageNameFactory.Should().BeOfType<ImageNameFactory>();
        }
    }
} 
using AsyncImage_Fetcher_Service.Logic.Abstractions.Interfaces;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Xunit;

namespace AsyncImage_Fetcher_Service.Logic.Test
{
    public class DependencyInjectionTests
    {
        [Fact]
        public void AddLogicServices_ShouldRegisterAllServices()
        {
            var services = new ServiceCollection();

            services.AddSingleton(Mock.Of<IImageRepository>());
            services.AddSingleton(Mock.Of<IImageRequestService>());
            services.AddSingleton(Mock.Of<Rules.Abstractions.IImageNameFactory>());


            services.AddLogicServices();
            var serviceProvider = services.BuildServiceProvider();

            serviceProvider.GetService<IDispatcher>().Should().NotBeNull();
            serviceProvider.GetService<ICommandDispatcher>().Should().NotBeNull();
            serviceProvider.GetService<IQueryDispatcher>().Should().NotBeNull();
            serviceProvider.GetService<ICommandHandler<Logic.Images.Commands.DownloadImagesCommand, System.Collections.Generic.Dictionary<string, string>>>().Should().NotBeNull();
            serviceProvider.GetService<IQueryHandler<Logic.Images.Queries.GetImageByNameQuery, string>>().Should().NotBeNull();
        }
    }
} 
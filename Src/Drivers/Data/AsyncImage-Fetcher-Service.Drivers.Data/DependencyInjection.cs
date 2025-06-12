using AsyncImage_Fetcher_Service.Drivers.Data.Abstractions.Interfaces;
using AsyncImage_Fetcher_Service.Drivers.Data.Repositories;
using AsyncImage_Fetcher_Service.Drivers.Data.Services;
using AsyncImage_Fetcher_Service.Logic.Abstractions.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace AsyncImage_Fetcher_Service.Drivers.Data
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddDataServices(this IServiceCollection services)
        {
            services.AddSingleton<IFileStorageService, LocalFileStorageService>();
            services.AddScoped<IImageRepository, ImageRepository>();

            return services;
        }
    }
}

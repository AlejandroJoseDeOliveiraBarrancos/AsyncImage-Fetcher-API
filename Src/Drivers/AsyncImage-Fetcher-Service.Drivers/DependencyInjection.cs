using AsyncImage_Fetcher_Service.Logic;
using Microsoft.Extensions.DependencyInjection;

namespace AsyncImage_Fetcher_Service.Drivers
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddDriversServices(this IServiceCollection services)
        {
            services.AddLogicServices();

            // Register other infrastructure services here in the future
            // e.g., services.AddScoped<IImageStorage, FileSystemImageStorage>();

            return services;
        }
    }
}

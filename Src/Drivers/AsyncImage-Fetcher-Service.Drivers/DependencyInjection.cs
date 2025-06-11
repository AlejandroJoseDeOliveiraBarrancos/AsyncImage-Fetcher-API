using Microsoft.Extensions.DependencyInjection;

namespace AsyncImage_Fetcher_Service.Drivers
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddDriversServices(this IServiceCollection services)
        {
            return services;
        }
    }
}

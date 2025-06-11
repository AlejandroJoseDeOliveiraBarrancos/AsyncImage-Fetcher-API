using Microsoft.Extensions.DependencyInjection;

namespace AsyncImage_Fetcher_Service.Adapters
{

    public static class DependencyInjection
    {
        public static IServiceCollection AddAdapterServices(this IServiceCollection services)
        {
            return services;
        }
    }
}

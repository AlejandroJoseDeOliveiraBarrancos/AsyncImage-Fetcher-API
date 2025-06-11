using Microsoft.Extensions.DependencyInjection;

namespace AsyncImage_Fetcher_Service.Drivers.Requests
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddRequestsServices(this IServiceCollection services)
        {
            return services;
        }
    }
}

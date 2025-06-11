using Microsoft.Extensions.DependencyInjection;

namespace AsyncImage_Fetcher_Service.Logic.Images;

public static class DependencyInjection
{
    public static IServiceCollection AddImageServices(this IServiceCollection services)
    {
        // Add image logic-related services here
        return services;
    }
}

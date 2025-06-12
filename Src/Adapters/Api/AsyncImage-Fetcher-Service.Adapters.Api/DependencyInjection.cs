using AsyncImage_Fetcher_Service.Adapters.Api.Middleware;
using Microsoft.Extensions.DependencyInjection;

namespace AsyncImage_Fetcher_Service.Adapters.Api;

public static class DependencyInjection
{
    public static IServiceCollection AddApiServices(this IServiceCollection services)
    {
        services.AddControllers();
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();

        return services;
    }

    public static IServiceCollection AddPresentation(this IServiceCollection services)
    {
        services.AddSingleton<ErrorHandlingMiddleware>();
        services.AddSingleton<RequestLoggingMiddleware>();
        services.AddSingleton<CorrelationIdMiddleware>();
        return services;
    }
}

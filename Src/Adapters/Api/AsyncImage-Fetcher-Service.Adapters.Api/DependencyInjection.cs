using AsyncImage_Fetcher_Service.Adapters.Api.Middleware;
using AsyncImage_Fetcher_Service.Drivers.Data;
using AsyncImage_Fetcher_Service.Drivers.Requests;
using AsyncImage_Fetcher_Service.Logic;

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

        services.AddLogicServices();
        services.AddRequestsServices();
        services.AddDataServices();

        return services;
    }
}

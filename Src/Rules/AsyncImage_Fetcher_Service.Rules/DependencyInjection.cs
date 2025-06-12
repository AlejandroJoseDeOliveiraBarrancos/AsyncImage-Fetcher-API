using AsyncImage_Fetcher_Service.Rules.Abstractions;
using AsyncImage_Fetcher_Service.Rules.Factories;
using Microsoft.Extensions.DependencyInjection;

namespace AsyncImage_Fetcher_Service.Rules
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddRulesServices(this IServiceCollection services)
        {
            services.AddSingleton<IImageNameFactory, ImageNameFactory>();
            return services;
        }
    }
}
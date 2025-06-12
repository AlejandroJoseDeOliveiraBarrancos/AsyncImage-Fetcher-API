using AsyncImage_Fetcher_Service.Logic.Abstractions.Interfaces;
using AsyncImage_Fetcher_Service.Logic.Images.Commands;
using AsyncImage_Fetcher_Service.Logic.Images.Handlers;
using AsyncImage_Fetcher_Service.Logic.Images.Queries;
using AsyncImage_Fetcher_Service.Rules;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace AsyncImage_Fetcher_Service.Logic
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddLogicServices(this IServiceCollection services)
        {
            services.AddScoped<IQueryHandler<GetImageByNameQuery, string>, GetImageByNameQueryHandler>();
            services.AddScoped<ICommandHandler<DownloadImagesCommand, Dictionary<string,string>>, DownloadImagesCommandHandler>();

            services.AddScoped<IDispatcher, Dispatcher>();
            services.AddScoped<ICommandDispatcher, Dispatcher>();
            services.AddScoped<IQueryDispatcher, Dispatcher>();

            var assembly = Assembly.GetExecutingAssembly();

            services.Scan(s => s.FromAssemblies(assembly)
                .AddClasses(c => c.AssignableTo(typeof(ICommandHandler<>)))
                .AsImplementedInterfaces()
                .WithScopedLifetime());

            services.Scan(s => s.FromAssemblies(assembly)
                .AddClasses(c => c.AssignableTo(typeof(IQueryHandler<,>)))
                .AsImplementedInterfaces()
                .WithScopedLifetime());

            services.AddRulesServices();

            return services;
        }
    }
}

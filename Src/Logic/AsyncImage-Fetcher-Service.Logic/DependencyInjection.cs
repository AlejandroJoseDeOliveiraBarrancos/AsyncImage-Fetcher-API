using AsyncImage_Fetcher_Service.Logic.Abstractions.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace AsyncImage_Fetcher_Service.Logic
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddLogicServices(this IServiceCollection services)
        {
            services.AddSingleton<ICommandDispatcher, Dispatcher>();
            services.AddSingleton<IQueryDispatcher, Dispatcher>();

            var assembly = Assembly.GetExecutingAssembly();

            services.Scan(s => s.FromAssemblies(assembly)
                .AddClasses(c => c.AssignableTo(typeof(ICommandHandler<>)))
                .AsImplementedInterfaces()
                .WithScopedLifetime());

            services.Scan(s => s.FromAssemblies(assembly)
                .AddClasses(c => c.AssignableTo(typeof(IQueryHandler<,>)))
                .AsImplementedInterfaces()
                .WithScopedLifetime());

            return services;
        }
    }
}

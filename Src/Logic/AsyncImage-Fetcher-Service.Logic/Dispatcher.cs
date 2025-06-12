using AsyncImage_Fetcher_Service.Logic.Abstractions.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace AsyncImage_Fetcher_Service.Logic
{
    internal sealed class Dispatcher : ICommandDispatcher, IQueryDispatcher
    {
        private readonly IServiceProvider _serviceProvider;

        public Dispatcher(IServiceProvider serviceProvider)
            => _serviceProvider = serviceProvider;

        public async Task SendAsync<TCommand>(TCommand command, CancellationToken cancellationToken = default) where TCommand : ICommand
        {
            await using var scope = _serviceProvider.CreateAsyncScope();
            var handler = scope.ServiceProvider.GetRequiredService<ICommandHandler<TCommand>>();
            await handler.HandleAsync(command, cancellationToken);
        }

        public Task<TResult> SendAsync<TResult>(ICommand<TResult> command, CancellationToken cancellationToken = default)
        {
            var handlerType = typeof(ICommandHandler<,>).MakeGenericType(command.GetType(), typeof(TResult));
            dynamic handler = _serviceProvider.GetRequiredService(handlerType);
            return handler.HandleAsync((dynamic)command, cancellationToken);
        }

        public async Task<TResult> QueryAsync<TResult>(IQuery<TResult> query, CancellationToken cancellationToken = default)
        {
            await using var scope = _serviceProvider.CreateAsyncScope();
            var handlerType = typeof(IQueryHandler<,>).MakeGenericType(query.GetType(), typeof(TResult));
            var handler = scope.ServiceProvider.GetRequiredService(handlerType);
            return await (Task<TResult>)handlerType.GetMethod(nameof(IQueryHandler<IQuery<TResult>, TResult>.HandleAsync))?
                .Invoke(handler, new object[] { query, cancellationToken });
        }
    }
}
namespace AsyncImage_Fetcher_Service.Logic.Abstractions.Interfaces
{
    public interface IDispatcher { }

    public interface ICommandDispatcher : IDispatcher
    {
        Task SendAsync<TCommand>(TCommand command, CancellationToken cancellationToken = default) where TCommand : ICommand;
    }

    public interface IQueryDispatcher : IDispatcher
    {
        Task<TResult> QueryAsync<TResult>(IQuery<TResult> query, CancellationToken cancellationToken = default);
    }
}
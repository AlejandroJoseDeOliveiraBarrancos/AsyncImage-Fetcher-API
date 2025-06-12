namespace AsyncImage_Fetcher_Service.Logic.Abstractions.Interfaces
{
    public interface ICommandHandler<in TCommand> where TCommand : ICommand
    {
        Task HandleAsync(TCommand command, CancellationToken cancellationToken = default);
    }

    public interface ICommandHandler<in TCommand, TResult> where TCommand : ICommand<TResult>
    {
        Task<TResult> HandleAsync(TCommand command, CancellationToken cancellationToken = default);
    }

    public interface IQueryHandler<in TQuery, TResult> where TQuery : IQuery<TResult>
    {
        Task<TResult> HandleAsync(TQuery query, CancellationToken cancellationToken = default);
    }
}
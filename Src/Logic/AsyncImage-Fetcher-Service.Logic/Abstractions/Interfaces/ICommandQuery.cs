namespace AsyncImage_Fetcher_Service.Logic.Abstractions.Interfaces
{
    public interface ICommand { }

    public interface IQuery<out TResult> { }
}
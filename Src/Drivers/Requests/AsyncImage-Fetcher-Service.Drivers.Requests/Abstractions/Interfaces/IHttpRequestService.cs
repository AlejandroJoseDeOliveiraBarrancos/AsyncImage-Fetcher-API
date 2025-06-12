namespace AsyncImage_Fetcher_Service.Drivers.Requests.Abstractions.Interfaces
{
    public interface IHttpRequestService
    {
        Task<byte[]> GetAsByteArrayAsync(string url, CancellationToken cancellationToken = default);
    }
}
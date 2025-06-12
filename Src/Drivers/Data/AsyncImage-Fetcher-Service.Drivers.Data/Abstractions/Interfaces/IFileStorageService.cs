namespace AsyncImage_Fetcher_Service.Drivers.Data.Abstractions.Interfaces
{
    public interface IFileStorageService
    {
        Task SaveFileAsync(string path, byte[] content, CancellationToken cancellationToken = default);
        Task<byte[]> ReadFileAsBytesAsync(string path, CancellationToken cancellationToken = default);
    }
}
using AsyncImage_Fetcher_Service.Logic.Images.Dtos;

namespace AsyncImage_Fetcher_Service.Logic.Abstractions.Interfaces
{
    public interface IImageRequestService
    {
        Task<ImageDto> FetchImageAsBase64Async(string imageUrl, CancellationToken cancellationToken = default);
    }
}
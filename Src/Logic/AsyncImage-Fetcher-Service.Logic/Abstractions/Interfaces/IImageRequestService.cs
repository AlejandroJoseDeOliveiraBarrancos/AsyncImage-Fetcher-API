using AsyncImage_Fetcher_Service.Logic.Images.Dtos;
using System.Threading;
using System.Threading.Tasks;

namespace AsyncImage_Fetcher_Service.Logic.Abstractions.Interfaces
{
    public interface IImageRequestService
    {
        Task<ImageDto> FetchImageAsBase64Async(string imageUrl, CancellationToken cancellationToken = default);
        Task<byte[]> FetchImageAsByteArrayAsync(string imageUrl, CancellationToken cancellationToken = default);
    }
}
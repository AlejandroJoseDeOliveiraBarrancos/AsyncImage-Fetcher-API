using AsyncImage_Fetcher_Service.Adapters.Api.Contracts.V1;
using AsyncImage_Fetcher_Service.Logic.Images.Queries;

namespace AsyncImage_Fetcher_Service.Adapters.Api.Abstractions
{
    public interface IImageMapper
    {
        GetImageByNameQuery ToQuery(string imageName);
        GetImageResponseDto ToDto(string imageBase64);
        ErrorResponseDto ToErrorDto(string message);
    }
} 
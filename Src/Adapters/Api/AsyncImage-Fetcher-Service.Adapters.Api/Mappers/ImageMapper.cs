using AsyncImage_Fetcher_Service.Adapters.Api.Abstractions;
using AsyncImage_Fetcher_Service.Adapters.Api.Contracts.V1;
using AsyncImage_Fetcher_Service.Logic.Images.Queries;

namespace AsyncImage_Fetcher_Service.Adapters.Api.Mappers
{
    public class ImageMapper : IImageMapper
    {
        public GetImageByNameQuery ToQuery(string imageName)
        {
            return new GetImageByNameQuery(imageName);
        }

        public GetImageResponseDto ToDto(string imageBase64)
        {
            return new GetImageResponseDto
            {
                Success = true,
                ImageBase64 = imageBase64
            };
        }

        public ErrorResponseDto ToErrorDto(string message)
        {
            return new ErrorResponseDto
            {
                Success = false,
                Message = message
            };
        }
    }
}
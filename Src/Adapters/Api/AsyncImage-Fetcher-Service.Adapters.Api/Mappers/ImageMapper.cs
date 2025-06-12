using AsyncImage_Fetcher_Service.Adapters.Api.Contracts.V1;
using AsyncImage_Fetcher_Service.Logic.Images.Commands;
using AsyncImage_Fetcher_Service.Logic.Images.Queries;

namespace AsyncImage_Fetcher_Service.Adapters.Api.Mappers
{
    public static class ImageMapper
    {
        public static DownloadImagesCommand ToCommand(this DownloadImagesRequestDto request)
        {
            return new DownloadImagesCommand(request.ImageUrls, request.MaxDownloadAtOnce);
        }

        public static GetImageByNameQuery ToQuery(string imageName)
        {
            return new GetImageByNameQuery(imageName);
        }

        public static GetImageResponseDto ToDto(string imageBase64)
        {
            return new GetImageResponseDto
            {
                Success = true,
                ImageBase64 = imageBase64
            };
        }

        public static ErrorResponseDto ToErrorDto(string message)
        {
            return new ErrorResponseDto
            {
                Message = message
            };
        }
    }
}
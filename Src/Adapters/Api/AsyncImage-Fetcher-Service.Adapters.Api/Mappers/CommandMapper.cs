using AsyncImage_Fetcher_Service.Adapters.Api.Contracts.V1;
using AsyncImage_Fetcher_Service.Logic.Images.Commands;

namespace AsyncImage_Fetcher_Service.Adapters.Api.Mappers
{
    public static class CommandMapper
    {
        public static DownloadImagesCommand ToCommand(this DownloadImagesRequestDto request)
        {
            return new DownloadImagesCommand(request.ImageUrls, request.MaxDownloadAtOnce);
        }
    }
} 
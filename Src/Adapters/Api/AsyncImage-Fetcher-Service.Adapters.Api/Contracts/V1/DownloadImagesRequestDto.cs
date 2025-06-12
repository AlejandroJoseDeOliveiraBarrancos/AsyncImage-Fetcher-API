namespace AsyncImage_Fetcher_Service.Adapters.Api.Contracts.V1
{
    public class DownloadImagesRequestDto
    {
        public IEnumerable<string> ImageUrls { get; set; }
        public int MaxDownloadAtOnce { get; set; }
    }
}
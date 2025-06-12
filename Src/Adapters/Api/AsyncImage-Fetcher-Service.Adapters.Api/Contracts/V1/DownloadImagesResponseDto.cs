namespace AsyncImage_Fetcher_Service.Adapters.Api.Contracts.V1
{
    public class DownloadImagesResponseDto
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public Dictionary<string, string> UrlAndNames { get; set; }
    }
}
namespace AsyncImage_Fetcher_Service.Adapters.Api.Contracts.V1
{
    public class GetImageResponseDto
    {
        public bool Success { get; set; }
        public string ImageBase64 { get; set; }
    }
}
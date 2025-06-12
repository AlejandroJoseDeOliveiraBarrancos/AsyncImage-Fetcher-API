namespace AsyncImage_Fetcher_Service.Adapters.Api.Contracts.V1
{
    public class ErrorResponseDto
    {
        public bool Success { get; set; } = false;
        public string Message { get; set; }
    }
}
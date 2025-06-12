namespace AsyncImage_Fetcher_Service.Rules.Models
{
    public class Image
    {
        public string OriginalUrl { get; }
        public string SanitizedName { get; }

        public Image(string originalUrl, string sanitizedName)
        {
            OriginalUrl = originalUrl;
            SanitizedName = sanitizedName;
        }
    }
}

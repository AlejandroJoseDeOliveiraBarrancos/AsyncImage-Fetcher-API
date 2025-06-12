using AsyncImage_Fetcher_Service.Rules.Abstractions;
using AsyncImage_Fetcher_Service.Rules.Models;
using System.Text.RegularExpressions;

namespace AsyncImage_Fetcher_Service.Rules.Factories
{
    public class ImageNameFactory : IImageNameFactory
    {
        public Image Create(string url)
        {
            if (!Uri.TryCreate(url, UriKind.Absolute, out var uri))
            {
                throw new ArgumentException("Invalid URL format.", nameof(url));
            }

            var fileName = Path.GetFileName(uri.AbsolutePath);
            var sanitizedFileName = SanitizeFileName(fileName);
            var uniquePart = $"{DateTime.UtcNow:yyyyMMddHHmmss}_{Guid.NewGuid().ToString("N").Substring(0, 8)}";
            var finalName = $"{uniquePart}_{sanitizedFileName}";

            return new Image(url, finalName);
        }

        private string SanitizeFileName(string fileName)
        {
            var invalidChars = new string(Path.GetInvalidFileNameChars());
            var escapedInvalidChars = Regex.Escape(invalidChars);
            var invalidRegex = new Regex($"[{escapedInvalidChars}]", RegexOptions.Compiled);

            var sanitized = invalidRegex.Replace(fileName, "_");
            return sanitized.Length > 100 ? sanitized.Substring(0, 100) : sanitized;
        }
    }
}
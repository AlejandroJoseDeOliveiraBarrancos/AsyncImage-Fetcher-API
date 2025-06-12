using AsyncImage_Fetcher_Service.Rules.Models;
using FluentAssertions;
using Xunit;

namespace AsyncImage_Fetcher_Service.Rules.Test.Models
{
    public class ImageTests
    {
        [Fact]
        public void Constructor_ShouldSetPropertiesCorrectly()
        {
            var originalUrl = "http://example.com/image.jpg";
            var sanitizedName = "20231027120000_abcdef12_image.jpg";

            var image = new Image(originalUrl, sanitizedName);

            image.OriginalUrl.Should().Be(originalUrl);
            image.SanitizedName.Should().Be(sanitizedName);
        }
    }
} 
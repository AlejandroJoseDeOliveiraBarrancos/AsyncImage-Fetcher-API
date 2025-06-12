using AsyncImage_Fetcher_Service.Rules.Factories;
using FluentAssertions;
using System;
using Xunit;

namespace AsyncImage_Fetcher_Service.Rules.Test.Factories
{
    public class ImageNameFactoryTests
    {
        private readonly ImageNameFactory _factory = new();

        [Theory]
        [InlineData("http://example.com/image.jpg")]
        [InlineData("https://example.com/some/path/to/image.png")]
        [InlineData("ftp://example.com/resource.gif")]
        public void Create_WithValidUrl_ReturnsImageWithCorrectOriginalUrl(string url)
        {
            var image = _factory.Create(url);

            image.OriginalUrl.Should().Be(url);
        }

        [Fact]
        public void Create_WithValidUrl_ReturnsImageWithUniqueSanitizedName()
        {
            var url = "http://example.com/image.jpg";

            var image1 = _factory.Create(url);
            var image2 = _factory.Create(url);

            image1.SanitizedName.Should().NotBe(image2.SanitizedName);
            image1.SanitizedName.Should().EndWith("_image.jpg");
        }

        [Theory]
        [InlineData("not a url")]
        [InlineData("http:/invalid-url.com")]
        [InlineData(null)]
        public void Create_WithInvalidUrl_ThrowsArgumentException(string url)
        {
            Action act = () => _factory.Create(url);

            act.Should().Throw<ArgumentException>();
        }

        [Fact]
        public void Create_WithUrlContainingInvalidChars_SanitizesName()
        {
            var url = "http://example.com/a<b>c:d*e?f\"g<h>i|j.png";
            var expectedEnd = "_a_b_c_d_e_f_g_h_i_j.png";

            var image = _factory.Create(url);

            image.SanitizedName.Should().EndWith(expectedEnd);
        }

        [Fact]
        public void Create_WithUrlEndingInLongFileName_TruncatesSanitizedName()
        {
            var longFileName = new string('a', 150) + ".jpg";
            var url = $"http://example.com/{longFileName}";

            var image = _factory.Create(url);
            
            var sanitizedPart = image.SanitizedName.Split('_').Last();
            var expectedEnd = new string('a', 100);
            sanitizedPart.Should().Be(expectedEnd);
        }
    }
} 
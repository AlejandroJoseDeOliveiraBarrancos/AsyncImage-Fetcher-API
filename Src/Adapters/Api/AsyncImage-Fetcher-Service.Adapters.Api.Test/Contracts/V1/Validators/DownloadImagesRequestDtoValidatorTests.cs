using AsyncImage_Fetcher_Service.Adapters.Api.Contracts.V1;
using AsyncImage_Fetcher_Service.Adapters.Api.Contracts.V1.Validators;
using FluentValidation.TestHelper;
using Xunit;

namespace AsyncImage_Fetcher_Service.Adapters.Api.Test.Contracts.V1.Validators
{
    public class DownloadImagesRequestDtoValidatorTests
    {
        private readonly DownloadImagesRequestDtoValidator _validator = new();

        [Fact]
        public void Should_Have_Error_When_ImageUrls_Is_Null()
        {
            var model = new DownloadImagesRequestDto { ImageUrls = null };
            var result = _validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(x => x.ImageUrls);
        }

        [Fact]
        public void Should_Have_Error_When_ImageUrls_Contains_Invalid_Url()
        {
            var model = new DownloadImagesRequestDto { ImageUrls = new[] { "http://valid.com", "not-a-valid-url" } };
            var result = _validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(x => x.ImageUrls);
        }

        [Fact]
        public void Should_Not_Have_Error_When_ImageUrls_Are_Valid()
        {
            var model = new DownloadImagesRequestDto { ImageUrls = new[] { "http://valid.com/image.jpg" }, MaxDownloadAtOnce = 1 };
            var result = _validator.TestValidate(model);
            result.ShouldNotHaveValidationErrorFor(x => x.ImageUrls);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public void Should_Have_Error_When_MaxDownloadAtOnce_Is_Not_Greater_Than_0(int value)
        {
            var model = new DownloadImagesRequestDto { MaxDownloadAtOnce = value };
            var result = _validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(x => x.MaxDownloadAtOnce);
        }
    }
} 
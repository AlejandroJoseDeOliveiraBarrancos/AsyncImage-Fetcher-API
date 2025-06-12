using AsyncImage_Fetcher_Service.Adapters.Api.Contracts.V1;
using AsyncImage_Fetcher_Service.Adapters.Api.Contracts.V1.Validators;
using FluentValidation.TestHelper;
using Xunit;

namespace AsyncImage_Fetcher_Service.Adapters.Api.Test.Contracts.V1.Validators
{
    public class GetImageByNameRequestDtoValidatorTests
    {
        private readonly GetImageByNameRequestDtoValidator _validator = new();

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void Should_Have_Error_When_ImageName_Is_Null_Or_Empty(string imageName)
        {
            var model = new GetImageByNameRequestDto { ImageName = imageName };
            var result = _validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(x => x.ImageName);
        }

        [Fact]
        public void Should_Have_Error_When_ImageName_Is_Too_Long()
        {
            var model = new GetImageByNameRequestDto { ImageName = new string('a', 101) };
            var result = _validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(x => x.ImageName);
        }

        [Fact]
        public void Should_Not_Have_Error_When_ImageName_Is_Valid()
        {
            var model = new GetImageByNameRequestDto { ImageName = "valid-image-name.jpg" };
            var result = _validator.TestValidate(model);
            result.ShouldNotHaveValidationErrorFor(x => x.ImageName);
        }
    }
} 
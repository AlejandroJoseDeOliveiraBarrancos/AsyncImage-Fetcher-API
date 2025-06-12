using AsyncImage_Fetcher_Service.Adapters.Api.Mappers;
using FluentAssertions;
using Xunit;

namespace AsyncImage_Fetcher_Service.Adapters.Api.Test.Mappers
{
    public class ImageMapperTests
    {
        private readonly ImageMapper _mapper = new();

        [Fact]
        public void ToQuery_Should_Create_Correct_Query()
        {
            var imageName = "test.jpg";
            var query = _mapper.ToQuery(imageName);
            query.ImageName.Should().Be(imageName);
        }

        [Fact]
        public void ToDto_Should_Create_Correct_Dto()
        {
            var base64 = "base64string";
            var dto = _mapper.ToDto(base64);
            dto.Success.Should().BeTrue();
            dto.ImageBase64.Should().Be(base64);
        }

        [Fact]
        public void ToErrorDto_Should_Create_Correct_Error_Dto()
        {
            var message = "Error message";
            var dto = _mapper.ToErrorDto(message);
            dto.Success.Should().BeFalse();
            dto.Message.Should().Be(message);
        }
    }
} 
using AsyncImage_Fetcher_Service.Adapters.Api.Contracts.V1;
using AsyncImage_Fetcher_Service.Adapters.Api.Mappers;
using FluentAssertions;
using System.Collections.Generic;
using Xunit;

namespace AsyncImage_Fetcher_Service.Adapters.Api.Test.Mappers
{
    public class CommandMapperTests
    {
        [Fact]
        public void ToCommand_Should_Create_Correct_Command()
        {
            var request = new DownloadImagesRequestDto
            {
                ImageUrls = new List<string> { "http://example.com/image.jpg" },
                MaxDownloadAtOnce = 5
            };

            var command = request.ToCommand();

            command.ImageUrls.Should().BeEquivalentTo(request.ImageUrls);
            command.MaxDownloadAtOnce.Should().Be(request.MaxDownloadAtOnce);
        }
    }
} 
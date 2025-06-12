using AsyncImage_Fetcher_Service.Adapters.Api.Contracts.V1;
using AsyncImage_Fetcher_Service.Adapters.Api.Controllers.Base;
using AsyncImage_Fetcher_Service.Adapters.Api.Mappers;
using AsyncImage_Fetcher_Service.Logic.Abstractions.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace AsyncImage_Fetcher_Service.Adapters.Api.Controllers
{
    public class ImagesController : BaseController
    {
        private readonly ICommandDispatcher _commandDispatcher;
        private readonly IQueryDispatcher _queryDispatcher;

        public ImagesController(ICommandDispatcher commandDispatcher, IQueryDispatcher queryDispatcher)
        {
            _commandDispatcher = commandDispatcher;
            _queryDispatcher = queryDispatcher;
        }

        [HttpPost("download-images")]
        public async Task<IActionResult> DownloadImages([FromBody] DownloadImagesRequestDto request)
        {
            var command = request.ToCommand();
            await _commandDispatcher.SendAsync(command);

            var response = new DownloadImagesResponseDto
            {
                Success = true,
                Message = "Download started."
            };

            return Accepted(response);
        }

        [HttpGet("get-image-by-name/{imageName}")]
        public async Task<IActionResult> GetImageByName(string imageName)
        {
            var query = ImageMapper.ToQuery(imageName);
            var imageBase64 = await _queryDispatcher.QueryAsync(query);

            if (string.IsNullOrEmpty(imageBase64))
            {
                return NotFound(ImageMapper.ToErrorDto("Image not found"));
            }

            var response = ImageMapper.ToDto(imageBase64);

            return Ok(response);
        }
    }
}
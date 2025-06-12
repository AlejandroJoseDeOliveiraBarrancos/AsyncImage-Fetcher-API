using AsyncImage_Fetcher_Service.Adapters.Api.Abstractions;
using AsyncImage_Fetcher_Service.Adapters.Api.Contracts.V1;
using AsyncImage_Fetcher_Service.Adapters.Api.Contracts.V1.Validators;
using AsyncImage_Fetcher_Service.Adapters.Api.Controllers.Base;
using AsyncImage_Fetcher_Service.Adapters.Api.Mappers;
using AsyncImage_Fetcher_Service.Logic.Abstractions.Interfaces;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AsyncImage_Fetcher_Service.Adapters.Api.Controllers
{
    [AllowAnonymous]
    public sealed class ImagesController : BaseController
    {
        private readonly ICommandDispatcher _commandDispatcher;
        private readonly IQueryDispatcher _queryDispatcher;
        private readonly IImageMapper _imageMapper;
        private readonly IValidator<GetImageByNameRequestDto> _getImageValidator;

        public ImagesController(
            ICommandDispatcher commandDispatcher, 
            IQueryDispatcher queryDispatcher,
            IImageMapper imageMapper,
            IValidator<GetImageByNameRequestDto> getImageValidator)
        {
            _commandDispatcher = commandDispatcher;
            _queryDispatcher = queryDispatcher;
            _imageMapper = imageMapper;
            _getImageValidator = getImageValidator;
        }

        [HttpPost("download-images")]
        public async Task<IActionResult> DownloadImages([FromBody] DownloadImagesRequestDto request)
        {
            var command = request.ToCommand();
            var resultMap = await _commandDispatcher.SendAsync(command);

            var response = new DownloadImagesResponseDto
            {
                Success = true,
                Message = "Downloads processed. Check status for each URL.",
                UrlAndNames = resultMap
            };

            return Ok(response);
        }

        [HttpGet("get-image-by-name/{imageName}")]
        public async Task<IActionResult> GetImageByName(string imageName)
        {
            var requestDto = new GetImageByNameRequestDto { ImageName = imageName };
            var validationResult = _getImageValidator.Validate(requestDto);

            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors.Select(e => new { e.PropertyName, e.ErrorMessage }));
            }

            var query = _imageMapper.ToQuery(imageName);
            var imageBase64 = await _queryDispatcher.QueryAsync(query);

            if (string.IsNullOrEmpty(imageBase64))
            {
                return NotFound(_imageMapper.ToErrorDto("Image not found"));
            }

            var response = _imageMapper.ToDto(imageBase64);

            return Ok(response);
        }
    }
}
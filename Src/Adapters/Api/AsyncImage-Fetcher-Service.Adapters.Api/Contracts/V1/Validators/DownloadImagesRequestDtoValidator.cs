using FluentValidation;

namespace AsyncImage_Fetcher_Service.Adapters.Api.Contracts.V1.Validators
{
    public class DownloadImagesRequestDtoValidator : AbstractValidator<DownloadImagesRequestDto>
    {
        public DownloadImagesRequestDtoValidator()
        {
            RuleFor(x => x.ImageUrls)
                .NotEmpty().WithMessage("Image URLs list cannot be empty.")
                .Must(urls => urls.All(url => Uri.TryCreate(url, UriKind.Absolute, out _)))
                .WithMessage("All provided URLs must be valid absolute URIs.");

            RuleFor(x => x.MaxDownloadAtOnce)
                .GreaterThan(0).WithMessage("Maximum downloads at once must be greater than 0.");
        }
    }
}
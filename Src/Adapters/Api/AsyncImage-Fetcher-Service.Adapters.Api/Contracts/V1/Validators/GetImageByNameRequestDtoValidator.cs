using FluentValidation;

namespace AsyncImage_Fetcher_Service.Adapters.Api.Contracts.V1.Validators
{
    public class GetImageByNameRequestDtoValidator : AbstractValidator<GetImageByNameRequestDto>
    {
        public GetImageByNameRequestDtoValidator()
        {
            RuleFor(x => x.ImageName)
                .NotEmpty().WithMessage("Image name cannot be empty.")
                .MaximumLength(100).WithMessage("Image name cannot be longer than 100 characters.");
        }
    }
}
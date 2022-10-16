using FluentValidation;

namespace NZWalks.API.Validations
{
    public class UpdateRegionRequestValidator :AbstractValidator<API.Models.DTO.UpdateRegionRequest>
    {
        public UpdateRegionRequestValidator()
        {
            RuleFor(x => x.Area).NotEmpty();
            RuleFor(x=>x.Name).NotEmpty();
            RuleFor(x => x.Code).MinimumLength(2);
            RuleFor(x => x.Population).GreaterThan(0);
        }
    }
}

using FluentValidation;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace NZWalks.API.Validations
{
    public class AddRegionRequestValidator : AbstractValidator<API.Models.DTO.AddRegionRequest>
    {
        public AddRegionRequestValidator()
        {
            RuleFor(x=>x.Area).NotEmpty();
            RuleFor(x=>x.Name).NotEmpty();
            RuleFor(x => x.Code).MinimumLength(2);
            RuleFor(x => x.Population).GreaterThan(0);
        }
    }
}

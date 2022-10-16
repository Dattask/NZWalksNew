using FluentValidation;

namespace NZWalks.API.Validations
{
    public class UpdateWalkRequestValidator : AbstractValidator<API.Models.DTO.UpdateWalkRequest>
    {
        public UpdateWalkRequestValidator()
        {
            RuleFor(x => x.Name).NotEmpty();
            RuleFor(x => x.Length).GreaterThan(2);
        }
    }
}

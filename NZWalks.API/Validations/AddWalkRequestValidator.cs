using FluentValidation;

namespace NZWalks.API.Validations
{
    public class AddWalkRequestValidator :AbstractValidator<API.Models.DTO.AddWalkRequest>
    {
        public AddWalkRequestValidator()
        {
            RuleFor(x => x.Name).NotEmpty();
            RuleFor(x => x.Length).GreaterThan(2);
        }
    }
}

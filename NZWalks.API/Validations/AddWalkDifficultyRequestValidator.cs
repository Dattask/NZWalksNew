using FluentValidation;

namespace NZWalks.API.Validations
{
    public class AddWalkDifficultyRequestValidator :AbstractValidator<API.Models.DTO.AddWalkDifficulty>
    {
        public AddWalkDifficultyRequestValidator()
        {
            RuleFor(x => x.Code).MinimumLength(2);
            RuleFor(x => x.Code).NotEmpty();
            RuleFor(x => x.Code).MaximumLength(10);
        }
    }
}

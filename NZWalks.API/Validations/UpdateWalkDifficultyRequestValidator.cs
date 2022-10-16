using FluentValidation;

namespace NZWalks.API.Validations
{
    public class UpdateWalkDifficultyRequestValidator :AbstractValidator<API.Models.DTO.UpdateWalkDifficulty>
    {
        public UpdateWalkDifficultyRequestValidator()
        {
            RuleFor(x => x.Code).MinimumLength(3);
            RuleFor(x => x.Code).MaximumLength(12);
        }
    }
}

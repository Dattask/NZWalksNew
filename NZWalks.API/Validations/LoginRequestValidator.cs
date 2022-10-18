using FluentValidation;

namespace NZWalks.API.Validations
{
    public class LoginRequestValidator : AbstractValidator<API.Models.DTO.LoginRequest>
    {
        public LoginRequestValidator()
        {
            RuleFor(x => x.Username).NotEmpty();
            RuleFor(x => x.Password).NotEmpty();
        }
    }
}

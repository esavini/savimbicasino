using FluentValidation;
using SavimbiCasino.WebApi.Dtos;

namespace SavimbiCasino.WebApi.FluentValidation
{
    public class CredentialsDtoValidator : AbstractValidator<CredentialsDto>
    {
        public CredentialsDtoValidator()
        {
            RuleFor(p => p.Username).MinimumLength(3).MaximumLength(10);
            RuleFor(p => p.Password).MinimumLength(6);
        }
    }
}
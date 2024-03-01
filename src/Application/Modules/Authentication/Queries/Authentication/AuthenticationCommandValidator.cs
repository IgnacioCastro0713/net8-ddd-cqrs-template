namespace Application.Modules.Authentication.Queries.Authentication;

public sealed class AuthenticationCommandValidator : AbstractValidator<AuthenticationCommand>
{
    public AuthenticationCommandValidator()
    {
        RuleFor(c => c.Username)
            .NotEmpty();

        RuleFor(c => c.Password)
            .NotEmpty();
    }
}
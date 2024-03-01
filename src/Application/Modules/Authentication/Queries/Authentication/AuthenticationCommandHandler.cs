using Domain.Dtos.Authentication.Token;

namespace Application.Modules.Authentication.Queries.Authentication;

public sealed class AuthenticationCommandHandler(
    ITokenProvider tokenProvider,
    IActiveDirectoryProvider activeDirectoryProvider,
    IUserRepository userRepository) : ICommandHandler<AuthenticationCommand, Result<AuthenticationResponse>>
{
    public async Task<Result<AuthenticationResponse>> Handle(
        AuthenticationCommand request,
        CancellationToken cancellationToken)
    {
        var hasValidCredentials = activeDirectoryProvider.ValidateCredentials(request.Username, request.Password);

        if (!hasValidCredentials)
        {
            return AuthenticationErrors.UserActiveDirectoryInvalidCredential;
        }

        var userPrincipalResult = activeDirectoryProvider.FindByIdentity(request.Username);

        if (userPrincipalResult.IsFailure)
        {
            return userPrincipalResult.Error;
        }

        var user = await userRepository
            .GetAuthenticatedUserByNtWithRoleAsync(userPrincipalResult.Value.Name, cancellationToken);

        if (user is null)
        {
            return UserErrors.UserNotFound(request.Username);
        }

        var tokenResponse = tokenProvider.Generate(user.Adapt<TokenRequestDto>());
        var userResponse = user with { DisplayName = userPrincipalResult.Value.DisplayName };

        return new AuthenticationResponse(userResponse, tokenResponse.Token, tokenResponse.Expires);
    }
}
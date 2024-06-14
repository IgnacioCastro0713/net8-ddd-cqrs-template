using Domain.Dtos.ActiveDirectory;

namespace Application.Abstractions.Providers;

public interface IActiveDirectoryProvider
{
    Result<UserPrincipalResponseDto> FindByIdentity(string? identityValue);
    bool ValidateCredentials(string ntUser, string password);
}
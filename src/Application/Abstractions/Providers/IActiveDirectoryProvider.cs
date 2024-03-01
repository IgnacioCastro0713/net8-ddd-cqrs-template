using Domain.Dtos.ActiveDirectory;

namespace Application.Abstractions.Providers;

public interface IActiveDirectoryProvider
{
    public Result<UserPrincipalResponseDto> FindByIdentity(string? identityValue);
    public bool ValidateCredentials(string ntUser, string password);
}
using Domain.Dtos.Authentication;

namespace Application.Mappings;

public sealed class AuthenticationMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<User, AuthenticatedUserResponseDto>()
            .Map(response => response.Role, source => source.Role.Name);
    }
}
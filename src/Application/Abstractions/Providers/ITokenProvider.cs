using Domain.Dtos.Authentication.Token;

namespace Application.Abstractions.Providers;

public interface ITokenProvider
{
    public TokenResponseDto Generate(TokenRequestDto request);
}
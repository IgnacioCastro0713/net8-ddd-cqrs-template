using Domain.Dtos.Authentication.Token;

namespace Application.Abstractions.Providers;

public interface ITokenProvider
{
    TokenResponseDto Generate(TokenRequestDto request);
}
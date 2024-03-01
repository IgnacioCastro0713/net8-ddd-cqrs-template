using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Domain.Dtos.Authentication.Token;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Infrastructure.Providers;

public sealed class TokenProvider(
    IOptions<JwtOptions> jwtOptions
) : ITokenProvider
{
    private readonly JwtOptions _jwtOptions = jwtOptions.Value;

    public TokenResponseDto Generate(TokenRequestDto request)
    {
        List<Claim> claims =
        [
            new Claim(JwtRegisteredClaimNames.UniqueName, request.NtUser),
            new Claim(JwtRegisteredClaimNames.Email, request.Email),
            new Claim(JwtRegisteredClaimNames.Sub, request.Id.ToString())
        ];

        var key = Encoding.UTF8.GetBytes(_jwtOptions.SecretKey!);

        var credentials = new SigningCredentials(
            new SymmetricSecurityKey(key),
            SecurityAlgorithms.HmacSha256);

        var tokenDescriptor = new JwtSecurityToken(
            _jwtOptions.Issuer,
            _jwtOptions.Audience,
            claims,
            null,
            DateTime.UtcNow.AddDays(_jwtOptions.DurationDays),
            credentials
        );

        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.WriteToken(tokenDescriptor);

        return new TokenResponseDto
        {
            Token = token,
            Expires = tokenDescriptor.ValidTo
        };
    }
}
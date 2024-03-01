namespace Domain.Dtos.Authentication.Token;

public sealed class TokenResponseDto
{
    public string Token { get; set; } = default!;
    public DateTime Expires { get; set; }
}
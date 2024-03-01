namespace Domain.Dtos.Authentication.Token;

public sealed record TokenRequestDto
{
    public int Id { get; set; }
    public string NtUser { get; set; } = default!;
    public string Email { get; set; } = default!;
}
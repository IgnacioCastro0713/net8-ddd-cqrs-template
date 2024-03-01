namespace Domain.Dtos.ActiveDirectory;

public sealed class UserPrincipalResponseDto
{
    public string Name { get; set; } = default!;
    public string DisplayName { get; set; } = default!;
}
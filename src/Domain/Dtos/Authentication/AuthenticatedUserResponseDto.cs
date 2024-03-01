namespace Domain.Dtos.Authentication;

public sealed record AuthenticatedUserResponseDto
{
    public int Id { get; set; }
    public string? DisplayName { get; set; }
    public string? Email { get; set; }
    public string? NtUser { get; set; }
    public int RoleId { get; set; }
    public string? Role { get; set; }
    public bool PrivacyPolicyApproval { get; set; }
    public bool UserAgreementApproval { get; set; }
}
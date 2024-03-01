using Domain.Shared;

namespace Domain.Entities;

public class User : BaseEntity
{
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? Email { get; set; }
    public string? NtUser { get; set; }
    public bool PrivacyPolicyApproval { get; set; }
    public bool UserAgreementApproval { get; set; }
    public int RoleId { get; set; }
    public Role Role { get; set; } = null!;

    public static User Create(
        string? firstName,
        string? lastName,
        string? email,
        string? ntUser,
        Role role)
    {
        return new User
        {
            FirstName = firstName,
            LastName = lastName,
            Email = email,
            NtUser = ntUser,
            RoleId = role.Id,
            Role = role
        };
    }

    public void Update(
        string? firstName,
        string? lastName,
        string? email,
        string? ntUser,
        Role role)
    {
        FirstName = firstName;
        LastName = lastName;
        Email = email;
        NtUser = ntUser;
        RoleId = role.Id;
        Role = role;
    }
}
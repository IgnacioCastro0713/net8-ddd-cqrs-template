namespace Application.Modules.Users.Commands.UpdateUser;

public sealed record UpdateUserRequest(
    int Id,
    string? FirstName,
    string? LastName,
    string? Email,
    string? NtUser,
    int RoleId);
namespace Application.Modules.Users.Commands.InsertUser;

public sealed record InsertUserRequest(
    string? FirstName,
    string? LastName,
    string? Email,
    string? NtUser,
    int RoleId);
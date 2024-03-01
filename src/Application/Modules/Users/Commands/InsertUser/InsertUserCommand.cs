namespace Application.Modules.Users.Commands.InsertUser;

public sealed record InsertUserCommand(
    string? FirstName,
    string? LastName,
    string? Email,
    string? NtUser,
    int RoleId) : ICommand<Result<Unit>>;
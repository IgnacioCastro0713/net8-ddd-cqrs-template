namespace Application.Modules.Users.Commands.UpdateUser;

public sealed record UpdateUserCommand(
    int Id,
    string? FirstName,
    string? LastName,
    string? Email,
    string? NtUser,
    int RoleId) : ICommand<Result<Unit>>;
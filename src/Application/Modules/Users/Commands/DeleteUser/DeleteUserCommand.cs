namespace Application.Modules.Users.Commands.DeleteUser;

public sealed record DeleteUserCommand(int Id) : ICommand<Result<Unit>>;
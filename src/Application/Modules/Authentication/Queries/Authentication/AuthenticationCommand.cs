namespace Application.Modules.Authentication.Queries.Authentication;

public sealed record AuthenticationCommand(string Username, string Password) : ICommand<Result<AuthenticationResponse>>;
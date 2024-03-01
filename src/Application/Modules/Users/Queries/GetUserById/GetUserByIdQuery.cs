namespace Application.Modules.Users.Queries.GetUserById;

public sealed record GetUserByIdQuery(int Id) : IQuery<Result<User>>;
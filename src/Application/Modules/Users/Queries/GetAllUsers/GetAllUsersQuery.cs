namespace Application.Modules.Users.Queries.GetAllUsers;

public record GetAllUsersQuery : IQuery<Result<IEnumerable<User>>>;
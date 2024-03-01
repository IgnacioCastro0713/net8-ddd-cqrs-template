namespace Application.Modules.Users.Queries.GetAllUsers;

public sealed class GetAllUsersQueryHandler(IUserRepository userRepository)
	: IQueryHandler<GetAllUsersQuery, Result<IEnumerable<User>>>
{
	public async Task<Result<IEnumerable<User>>> Handle(GetAllUsersQuery request, CancellationToken cancellationToken)
	{
		return (await userRepository.GetAllWithRoleAsync(cancellationToken)).ToList();
	}
}
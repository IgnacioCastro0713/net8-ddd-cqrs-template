namespace Application.Modules.Users.Queries.GetUserById;

public sealed class GetUserByIdQueryHandler(IUserRepository userRepository)
    : IQueryHandler<GetUserByIdQuery, Result<User>>
{
    public async Task<Result<User>> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
    {
        var user = await userRepository.FindByIdAsync(request.Id);

        if (user is null)
        {
            return UserErrors.UserNotFound(request.Id);
        }

        return user;
    }
}
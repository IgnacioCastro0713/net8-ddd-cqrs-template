using Application.Abstractions;

namespace Application.Modules.Users.Commands.InsertUser;

public sealed class InsertUserCommandHandler(
    IUserRepository userRepository,
    IRoleRepository roleRepository,
    IUnitOfWork unitOfWork) : ICommandHandler<InsertUserCommand, Result<Unit>>
{
    public async Task<Result<Unit>> Handle(InsertUserCommand request, CancellationToken cancellationToken)
    {
        var role = await roleRepository.FindByIdAsync(request.RoleId);

        var user = User.Create(request.FirstName, request.LastName, request.Email, request.NtUser, role!);

        userRepository.Add(user);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
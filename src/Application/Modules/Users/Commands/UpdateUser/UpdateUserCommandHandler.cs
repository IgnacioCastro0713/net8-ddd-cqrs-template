using Application.Abstractions;

namespace Application.Modules.Users.Commands.UpdateUser;

public sealed class UpdateUserCommandHandler(
    IUserRepository userRepository,
    IRoleRepository roleRepository,
    IUnitOfWork unitOfWork) : ICommandHandler<UpdateUserCommand, Result<Unit>>
{
    public async Task<Result<Unit>> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
    {
        var role = await roleRepository.FindByIdAsync(request.RoleId);
        var user = await userRepository.FindByIdAsync(request.Id);

        if (user is null)
        {
            return UserErrors.UserNotFound(request.Id);
        }

        user.Update(request.FirstName, request.LastName, request.Email, request.NtUser, role!);

        userRepository.Update(user);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
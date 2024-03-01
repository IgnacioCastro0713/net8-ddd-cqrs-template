namespace Application.Modules.Users.Commands.UpdateUser;

public sealed class UpdateUserCommandValidator : AbstractValidator<UpdateUserCommand>
{
    private readonly IUserRepository _userRepository;
    private readonly IRoleRepository _roleRepository;

    public UpdateUserCommandValidator(IUserRepository userRepository, IRoleRepository roleRepository)
    {
        _userRepository = userRepository;
        _roleRepository = roleRepository;

        RuleFor(command => command.FirstName)
            .NotEmpty()
            .MaximumLength(100);

        RuleFor(command => command.LastName)
            .NotEmpty()
            .MaximumLength(100);

        RuleFor(command => command.Email)
            .NotEmpty()
            .EmailAddress()
            .MaximumLength(150)
            .MustAsync(IsUniqueEmail).WithMessage($"{nameof(UpdateUserCommand.Email)} is already exist.");

        RuleFor(command => command.NtUser)
            .NotEmpty()
            .MaximumLength(10)
            .MustAsync(IsUniqueNtUser).WithMessage($"{nameof(UpdateUserCommand.NtUser)} is already exist.");

        RuleFor(command => command.RoleId)
            .MustAsync(ExistRole).WithMessage($"{nameof(UpdateUserCommand.RoleId)} must be an existing role id.");
    }

    private async Task<bool> IsUniqueEmail(
        UpdateUserCommand command,
        string email,
        CancellationToken cancellationToken = default)
    {
        return !await _userRepository.AnyAsync(u => u.Email == email && u.Id != command.Id, cancellationToken);
    }

    private async Task<bool> IsUniqueNtUser(
        UpdateUserCommand command,
        string ntUser,
        CancellationToken cancellationToken = default)
    {
        return !await _userRepository.AnyAsync(u => u.NtUser == ntUser && u.Id != command.Id, cancellationToken);
    }

    private async Task<bool> ExistRole(int roleId, CancellationToken cancellationToken = default)
    {
        return await _roleRepository.AnyAsync(r => r.Id == roleId, cancellationToken);
    }
}
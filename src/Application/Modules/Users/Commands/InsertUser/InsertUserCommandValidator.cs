namespace Application.Modules.Users.Commands.InsertUser;

public sealed class InsertUserCommandValidator : AbstractValidator<InsertUserCommand>
{
    private readonly IUserRepository _userRepository;
    private readonly IRoleRepository _roleRepository;

    public InsertUserCommandValidator(IUserRepository userRepository, IRoleRepository roleRepository)
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
            .MustAsync(IsUniqueEmail).WithMessage($"{nameof(InsertUserCommand.Email)} is already exist.");

        RuleFor(command => command.NtUser)
            .NotEmpty()
            .MaximumLength(10)
            .MustAsync(IsUniqueNtUser).WithMessage($"{nameof(InsertUserCommand.NtUser)} is already exist.");

        RuleFor(command => command.RoleId)
            .MustAsync(ExistRole).WithMessage($"{nameof(InsertUserCommand.RoleId)} must be an existing role id.");
    }

    private async Task<bool> IsUniqueEmail(string email, CancellationToken cancellationToken = default)
    {
        return !await _userRepository.AnyAsync(u => u.Email == email, cancellationToken);
    }

    private async Task<bool> IsUniqueNtUser(string ntUser, CancellationToken cancellationToken = default)
    {
        return !await _userRepository.AnyAsync(u => u.NtUser == ntUser, cancellationToken);
    }

    private async Task<bool> ExistRole(int roleId, CancellationToken cancellationToken = default)
    {
        return await _roleRepository.AnyAsync(r => r.Id == roleId, cancellationToken);
    }
}
using Domain.Dtos.Authentication;

namespace Application.Abstractions.Repositories;

public interface IUserRepository : IRepository<User>
{
    Task<IEnumerable<User>> GetAllWithRoleAsync(CancellationToken cancellationToken = default);

    Task<AuthenticatedUserResponseDto?> GetAuthenticatedUserByNtWithRoleAsync(
        string? ntUser,
        CancellationToken cancellationToken = default);
}
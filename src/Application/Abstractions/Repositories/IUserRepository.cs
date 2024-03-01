using Domain.Dtos.Authentication;

namespace Application.Abstractions.Repositories;

public interface IUserRepository : IRepository<User>
{
    public Task<IEnumerable<User>> GetAllWithRoleAsync(CancellationToken cancellationToken = default);

    public Task<AuthenticatedUserResponseDto?> GetAuthenticatedUserByNtWithRoleAsync(
        string? ntUser,
        CancellationToken cancellationToken = default);
}
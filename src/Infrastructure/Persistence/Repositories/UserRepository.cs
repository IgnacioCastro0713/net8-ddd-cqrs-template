using Domain.Dtos.Authentication;

namespace Infrastructure.Persistence.Repositories;

public sealed class UserRepository(ApplicationDbContext context)
    : Repository<User>(context), IUserRepository
{
    public async Task<IEnumerable<User>> GetAllWithRoleAsync(CancellationToken cancellationToken = default)
    {
        return await context.Users
            .Include(user => user.Role)
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }

    public async Task<AuthenticatedUserResponseDto?> GetAuthenticatedUserByNtWithRoleAsync(
        string? ntUser,
        CancellationToken cancellationToken = default)
    {
        return await context.Users
            .Include(user => user.Role)
            .AsNoTracking()
            .ProjectToType<AuthenticatedUserResponseDto>()
            .FirstOrDefaultAsync(user => user.NtUser == ntUser, cancellationToken);
    }
}
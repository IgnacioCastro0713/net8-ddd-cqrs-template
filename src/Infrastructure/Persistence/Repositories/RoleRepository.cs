namespace Infrastructure.Persistence.Repositories;

public sealed class RoleRepository(ApplicationDbContext context)
    : Repository<Role>(context), IRoleRepository
{
}
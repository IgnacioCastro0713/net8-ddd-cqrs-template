using Microsoft.EntityFrameworkCore.Diagnostics;

namespace Infrastructure.Interceptors;

public sealed class AuditableEntityInterceptor(
    IUserContextProvider userContextProvider,
    TimeProvider timeProvider)
    : SaveChangesInterceptor
{
    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(
        DbContextEventData eventData,
        InterceptionResult<int> result,
        CancellationToken cancellationToken = new())
    {
        if (eventData.Context is null)
        {
            return base.SavingChangesAsync(eventData, result, cancellationToken);
        }

        var entries = eventData.Context.ChangeTracker
            .Entries()
            .Where(entry => entry.State is EntityState.Added or EntityState.Modified);

        foreach (var entry in entries)
        {
            var utcNow = timeProvider.GetUtcNow().UtcDateTime;
            if (entry is { State: EntityState.Added, Entity: IAuditable createdEntity })
            {
                createdEntity.CreatedBy = userContextProvider.Id;
                createdEntity.CreatedAt = utcNow;
            }

            if (entry is { State: EntityState.Modified, Entity: IAuditable updatedEntity })
            {
                updatedEntity.UpdatedBy = userContextProvider.Id;
                updatedEntity.UpdatedAt = utcNow;
            }
        }

        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }
}
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace Infrastructure.Persistence.Interceptors;

public sealed class SoftDeleteInterceptor(
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
            .Where(entry => entry.State is EntityState.Deleted);

        foreach (var entry in entries)
        {
            if (entry is not { Entity: ISoftDelete entity })
            {
                continue;
            }

            var utcNow = timeProvider.GetUtcNow().UtcDateTime;
            entry.State = EntityState.Modified;
            entity.DeletedBy = userContextProvider.Id;
            entity.DeletedAt = utcNow;
        }

        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }
}
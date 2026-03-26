using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace mini_project_aspnet.Models
{
    public class AuditInterceptor : SaveChangesInterceptor
    {
        public override ValueTask<InterceptionResult<int>> SavingChangesAsync(
            DbContextEventData eventData,
            InterceptionResult<int> result,
            CancellationToken cancellationToken = default)
        {
            var context = eventData.Context;

            foreach (var entry in context.ChangeTracker.Entries<BaseEntity>())
            {
                if (entry.State == EntityState.Added)
                {
                    entry.Entity.createdAt = DateTime.UtcNow;
                    entry.Entity.updatedAt = DateTime.UtcNow;
                }
                else if (entry.State == EntityState.Modified)
                {
                    entry.Entity.updatedAt = DateTime.UtcNow;
                    entry.Property(e => e.createdAt).IsModified = false;
                }
            }

            return base.SavingChangesAsync(eventData, result, cancellationToken);
        }
    }
}

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using MinimalAPI.Interfaces;

namespace MinimalAPI.Interceptors
{
    public class AuditAndSoftDeleteInterceptor : SaveChangesInterceptor
    {
        private readonly ICurrentActor _currentActor;

        public AuditAndSoftDeleteInterceptor(ICurrentActor currentActor)
        {
            _currentActor = currentActor;
        }

        public override InterceptionResult<int> SavingChanges(
            DbContextEventData eventData,
            InterceptionResult<int> result)
        {
            ApplyRules(eventData.Context);
            return base.SavingChanges(eventData, result);
        }


        public override ValueTask<InterceptionResult<int>> SavingChangesAsync(
             DbContextEventData eventData,
             InterceptionResult<int> result,
             CancellationToken cancellationToken = default)
        {
            ApplyRules(eventData.Context);
            return base.SavingChangesAsync(eventData, result, cancellationToken);
        }


        private void ApplyRules(DbContext? context)
        {
            if (context == null) return;
            var now = DateTime.UtcNow;
            var user = _currentActor.UserId;
            foreach (var entry in context.ChangeTracker.Entries())
            {
                if (entry.Entity is IAuditableEntity auditable)
                {
                    if (entry.State == EntityState.Added)
                    {
                        auditable.CreatedAt = now;
                        auditable.CreatedBy = user;
                    }

                    if (entry.State is EntityState.Added or EntityState.Modified)
                    {
                        auditable.UpdatedAt = now;
                        auditable.UpdatedBy = user;
                    }
                }
                if (entry.Entity is ISoftDelete softDeletable && entry.State == EntityState.Deleted)
                {
                    entry.State = EntityState.Modified;
                    softDeletable.IsDeleted = true;
                    if (softDeletable is IAuditableEntity audit)
                    {
                        audit.UpdatedAt = now;
                        audit.UpdatedBy = user;
                    }
                }
            }
        }
    }
}


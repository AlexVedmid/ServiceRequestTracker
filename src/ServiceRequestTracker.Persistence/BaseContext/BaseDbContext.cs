using Microsoft.EntityFrameworkCore;
using ServiceRequestTracker.Persistence.Entities;

namespace ServiceRequestTracker.Persistence.BaseContext
{
    public abstract class BaseDbContext : DbContext
    {
        protected BaseDbContext()
        {
        }

        protected BaseDbContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql();
        }

        public override int SaveChanges()
        {
            HandleModifiedEntities();
            HandleCreatedEntities();

            return base.SaveChanges();
        }

        public override int SaveChanges(bool acceptAllChangesOnSuccess)
        {
            HandleCreatedEntities();
            HandleModifiedEntities();

            return base.SaveChanges(acceptAllChangesOnSuccess);
        }

        public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
        {
            HandleDeletedEntities();
            HandleCreatedEntities();
            HandleModifiedEntities();

            return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }

        protected virtual void HandleCreatedEntities()
        {
            var entityEntries = ChangeTracker.Entries()
                .Where(e => (e.Entity is BaseEntity) && e.State == EntityState.Added);

            foreach (var entry in entityEntries)
            {
                var currentValue = entry.Property(nameof(BaseEntity.CreatedAt)).CurrentValue;

                if (currentValue == null || (DateTimeOffset)currentValue == DateTimeOffset.MinValue)
                {
                    entry.Property(nameof(BaseEntity.CreatedAt)).CurrentValue = DateTimeOffset.UtcNow;
                    entry.Property(nameof(BaseEntity.CreatedAt)).IsModified = true;
                }
            }
        }

        protected virtual void HandleModifiedEntities()
        {
            var entityEntries = ChangeTracker.Entries()
                .Where(e => (e.Entity is BaseEntity) && e.State == EntityState.Modified);

            foreach (var entry in entityEntries)
            {
                entry.Property(nameof(BaseEntity.UpdatedAt)).CurrentValue = DateTimeOffset.UtcNow;
                entry.Property(nameof(BaseEntity.UpdatedAt)).IsModified = true;
            }
        }

        protected virtual void HandleDeletedEntities()
        {
        }
    }
}

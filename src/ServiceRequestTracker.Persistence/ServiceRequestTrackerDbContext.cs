using MassTransit;
using Microsoft.EntityFrameworkCore;
using ServiceRequestTracker.Persistence.BaseContext;
using ServiceRequestTracker.Persistence.Entities;

namespace ServiceRequestTracker.Persistence
{
    public class ServiceRequestTrackerDbContext : BaseDbContext
    {
        public ServiceRequestTrackerDbContext()
        {
        }

        public ServiceRequestTrackerDbContext(DbContextOptions<ServiceRequestTrackerDbContext> options) : base(options)
        {
        }

        public DbSet<ServiceRequestEntity> ServiceRequests { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            // MassTransit tables
            modelBuilder.AddInboxStateEntity();
            modelBuilder.AddOutboxMessageEntity();
            modelBuilder.AddOutboxStateEntity();
            // Entities
            modelBuilder.Entity<ServiceRequestEntity>().HasKey(e => e.Id);
            modelBuilder.Entity<ServiceRequestEntity>().HasIndex(e => e.Id).IsUnique();
            modelBuilder.Entity<ServiceRequestEntity>().HasIndex(e => e.CreatedAt);
            modelBuilder.Entity<ServiceRequestEntity>().HasIndex(e => e.Status);
        }
    }
}

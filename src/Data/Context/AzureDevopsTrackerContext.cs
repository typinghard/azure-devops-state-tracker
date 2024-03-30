using AzureDevopsTracker.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace AzureDevopsTracker.Data.Context
{
    public class AzureDevopsTrackerContext : DbContext, IDisposable
    {
        public AzureDevopsTrackerContext(DbContextOptions options) : base(options)
        { }

        public DbSet<WorkItem> WorkItems { get; set; }
        public DbSet<WorkItemChange> WorkItemsChange { get; set; }
        public DbSet<TimeByState> TimeByStates { get; set; }
        public DbSet<ChangeLogItem> ChangeLogItems { get; set; }
        public DbSet<ChangeLog> ChangeLogs { get; set; }
        public DbSet<WorkItemCustomField> CustomFields { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            foreach (var property in modelBuilder.Model.GetEntityTypes().SelectMany(
                e => e.GetProperties().Where(p => p.ClrType == typeof(string))))
                property.SetColumnType("varchar(200)");

            modelBuilder.HasDefaultSchema(DataBaseConfig.SchemaName);

            modelBuilder.ApplyConfigurationsFromAssembly(typeof(AzureDevopsTrackerContext).Assembly);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
                optionsBuilder.UseSqlServer(DataBaseConfig.ConnectionsString);
        }
    }
}
using Microsoft.EntityFrameworkCore;
using MixedDbUnitTests.Persistance.Domain;
using System.Linq;

namespace MixedDbUnitTests.Persistance
{
    public class SampleDbContext : DbContext
    {
        public SampleDbContext(DbContextOptions options)
            : base(options)
        {
        }

        public DbSet<TestData> TestDatas { get; set; }
        public DbSet<ComplexData> ComlexDatas { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<TestData>().HasQueryFilter(e => !e.IsDeleted);
            SetGlobalRemoveDeletedQuery<ComplexData>(modelBuilder);
        }

        public override int SaveChanges(bool acceptAllChangesOnSuccess)
        {
            OnSavingChanges();
            return base.SaveChanges(acceptAllChangesOnSuccess);
        }

        private void OnSavingChanges()
        {
            var changedTrackableEntities = ChangeTracker.Entries().Where(x => x.Entity is AbstractData).ToList();
            foreach (var entityEntry in changedTrackableEntities)
            {
                if (entityEntry.State == EntityState.Deleted)
                {
                    entityEntry.State = EntityState.Modified;
                    ((AbstractData)entityEntry.Entity).IsDeleted = true;
                }
            }
        }

        public void SetGlobalRemoveDeletedQuery<T>(ModelBuilder builder) where T : AbstractData
        {
            builder.Entity<T>().HasQueryFilter(e => !e.IsDeleted);
        }
    }
}

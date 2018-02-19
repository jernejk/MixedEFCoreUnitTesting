using Microsoft.EntityFrameworkCore;
using MixedDbUnitTests.Persistance.Domain;

namespace MixedDbUnitTests.Persistance
{
    public class SampleDbContext : DbContext
    {
        public SampleDbContext(DbContextOptions options)
            : base(options)
        {
        }

        public DbSet<TestData> TestDatas { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<TestData>().HasQueryFilter(e => !e.IsDeleted);
        }
    }
}

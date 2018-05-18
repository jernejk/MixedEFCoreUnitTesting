using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using MixedDbUnitTests.Persistance.Domain;

namespace MixedDbUnitTests.Persistance
{
    public class SampleDbContext : DbContext
    {
        public SampleDbContext(DbContextOptions options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            if (Database.ProviderName == "Microsoft.EntityFrameworkCore.Sqlite")
            {
                // SQLite doesn't support GUID and this adds GUID support for SQLite provider.
                var converter = new GuidToBytesConverter();
                modelBuilder
                    .Entity<Parent>()
                    .Property(e => e.Id)
                    .HasConversion(converter);

                modelBuilder
                    .Entity<Parent>()
                    .Property(e => e.ChildId)
                    .HasConversion(converter);

                modelBuilder
                    .Entity<Child>()
                    .Property(e => e.Id)
                    .HasConversion(converter);
            }

            base.OnModelCreating(modelBuilder);
        }

        public DbSet<Parent> Parents { get; set; }
        public DbSet<Child> Children { get; set; }
    }
}

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

        public DbSet<Parent> Parents { get; set; }
        public DbSet<Child> Children { get; set; }
    }
}

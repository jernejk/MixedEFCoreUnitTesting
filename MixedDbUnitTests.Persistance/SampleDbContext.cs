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

        public DbSet<ParentData> Parents { get; set; }
        public DbSet<ChildData> Childs { get; set; }
    }
}

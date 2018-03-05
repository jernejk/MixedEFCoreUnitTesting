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

        public DbSet<ParentData> TestDatas { get; set; }
        public DbSet<ChildData> ChildDatas { get; set; }
    }
}

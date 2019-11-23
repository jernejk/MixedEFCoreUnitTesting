using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using MixedDbUnitTests.Persistance;
using System;
using System.Threading.Tasks;

namespace MixedDbUnitTests.Tests
{
    public abstract class TestBase
    {
        private bool _useSqlite;

        public async Task<SampleDbContext> GetDbContext()
        {
            DbContextOptionsBuilder builder = new DbContextOptionsBuilder();
            if (_useSqlite)
            {
                // Use Sqlite DB.
                builder.UseSqlite("DataSource=:memory:", x => { });
            }
            else
            {
                // Use In-Memory DB.
                builder.UseInMemoryDatabase(Guid.NewGuid().ToString()).ConfigureWarnings(w =>
                {
                    w.Ignore(InMemoryEventId.TransactionIgnoredWarning);
                });
            }
            
            var dbContext = new SampleDbContext(builder.Options);
            if (_useSqlite)
            {
                // SQLite needs to open connection to the DB.
                // Not required for in-memory-database and MS SQL.
                await dbContext.Database.OpenConnectionAsync();
            }

            await dbContext.Database.EnsureCreatedAsync();

            return dbContext;
        }

        public void UseSqlite()
        {
            _useSqlite = true;
        }
    }
}

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using MixedDbUnitTests.Persistance;
using System;

namespace MixedDbUnitTests.Tests
{
    public abstract class TestBase
    {
        private bool useSqlite;

        public void UseSqlite()
        {
            useSqlite = true;
        }

        public SampleDbContext GetDbContext()
        {
            DbContextOptionsBuilder builder = new DbContextOptionsBuilder();
            if (!useSqlite)
            {
                builder.UseInMemoryDatabase(Guid.NewGuid().ToString()).ConfigureWarnings(w =>
                    {
                        w.Ignore(InMemoryEventId.TransactionIgnoredWarning);
                    }).EnableSensitiveDataLogging(true);
            }
            else
            {
                builder.UseSqlite("DataSource=:memory:", x => { })
                    .EnableSensitiveDataLogging(true);
            }

            var dbContext = new SampleDbContext(builder.Options);
            if (useSqlite)
            {
                // SQLite needs to open connection to the DB.
                // Not required for in-memory-database and MS SQL.
                dbContext.Database.OpenConnection();
            }

            dbContext.Database.EnsureCreated();

            return dbContext;
        }
    }
}

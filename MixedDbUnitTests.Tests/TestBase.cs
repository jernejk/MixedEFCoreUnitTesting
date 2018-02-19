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
                builder.UseSqlite("DataSource=:memory:", x =>
                    {
                        x.SuppressForeignKeyEnforcement();
                    }).EnableSensitiveDataLogging(true);
            }

            var dbContext = new SampleDbContext(builder.Options);
            if (useSqlite)
            {
                dbContext.Database.OpenConnection();
            }

            dbContext.Database.EnsureCreated();

            return dbContext;
        }
    }
}

using Microsoft.EntityFrameworkCore;
using MixedDbUnitTests.Persistance.Domain;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace MixedDbUnitTests.Tests
{
    public class SqliteTests : TestBase
    {
        public SqliteTests()
        {
            UseSqlite();
        }

        [Fact]
        public async Task ShouldBeAbleToAddAndGetEntity()
        {
            // Prepare
            using var context = await GetDbContext();
            Guid id = Guid.NewGuid();
            context.Parents.Add(new Parent
            {
                Id = id,
                Name = "Parent name",
                Child = new Child
                {
                    Name = "Child name"
                }
            });
            await context.SaveChangesAsync();

            // Execute
            var data = await context.Parents.ToListAsync();

            // Assert
            Assert.Single(data);
            Assert.Contains(data, d => d.Id == id);
            Assert.Contains(data, d => d.Name == "Parent name");
            Assert.Contains(data, d => d.Child.Name == "Child name");
        }

        [Fact]
        [Trait("DbDependat", "")]
        public async Task ShouldBeAbleToExeuteSql()
        {
            // Prepare
            using var context = await GetDbContext();

            Guid id = Guid.NewGuid();
            context.Parents.Add(new Parent
            {
                Id = id,
                Name = "Parent name",
                Child = new Child
                {
                    Name = "Child name"
                }
            });

            await context.SaveChangesAsync();

            // Execute
            var result = await context.Parents
                .FromSqlRaw("select * from Parents")
                .ToListAsync();

            // It should get first line and convert byte[] into GUID.
            Assert.Equal(id, result.First().Id);
        }

        /// <summary>
        /// SQL allows you to check if constrains are set correctly.
        /// Sqlite can check most of simple constrains.
        /// </summary>
        [Fact]
        [Trait("DbDependat", "")]
        public async Task ShouldFailWhenIncludeIsNotUsed()
        {
            // Prepare
            using var context = await GetDbContext();

            // Add a child with non-existing ID.
            context.Parents.Add(new Parent
            {
                Name = "Parent name",
                ChildId = Guid.NewGuid()
            });

            // Execute and assert
            // Will throw expected exception.
            await Assert.ThrowsAsync<DbUpdateException>(
                () => context.SaveChangesAsync());

            // This line will be asserted only for SQLite.
            var parents = await context.Parents.ToListAsync();
            Assert.Empty(parents);
        }

        /// <summary>
        /// SQLite is slower than In-memory DB.
        /// Use it with SQL queries and when testing for constrains.
        /// </summary>
        [Fact]
        [Trait("Performance", "SQLite")]
        public async Task PerformanceTestSqlite()
        {
            using var context = await GetDbContext();
            for (int i = 0; i < 10000; ++i)
            {
                context.Parents.Add(new Parent
                {
                    Name = "Parent name",
                    Child = new Child
                    {
                        Name = "Child name"
                    }
                });
            }

            await context.SaveChangesAsync();
        }
    }
}

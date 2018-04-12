using Dapper;
using Microsoft.EntityFrameworkCore;
using MixedDbUnitTests.Persistance.Domain;
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
        public void ShouldBeAbleToAddAndGetEntity()
        {
            // Prepare
            var context = GetDbContext();
            context.Parents.Add(new Parent
            {
                Name = "Parent name",
                Child = new Child
                {
                    Name = "Child name"
                }
            });
            context.SaveChanges();

            // Execute
            var data = context.Parents.ToList();

            // Assert
            Assert.Single(data);
            Assert.Contains(data, d => d.Name == "Parent name");
            Assert.Contains(data, d => d.Child.Name == "Child name");
        }

        [Fact]
        public async Task ShouldWork()
        {
            // Prepare
            var context = GetDbContext();
            context.Parents.Add(new Parent
            {
                Name = "Parent name",
                Child = new Child
                {
                    Name = "Child name"
                }
            });

            context.SaveChanges();

            // Execute
            var result = await context.Database.GetDbConnection()
                .QueryAsync<Parent>(@"select * from Parents");

            // Assert
            Assert.Single(result);
        }

        /// <summary>
        /// SQL allows you to check if constrains are set correctly.
        /// Sqlite can check most of simple constrains.
        /// </summary>
        [Fact]
        public void ShouldFailWhenIncludeIsNotUsed()
        {
            // Prepare
            var context = GetDbContext();

            // Add a child with non-existing ID.
            context.Parents.Add(new Parent
            {
                Name = "Parent name",
                ChildId = 1
            });

            // Execute and assert
            // Will throw expected exception.
            Assert.Throws<DbUpdateException>(
                () => context.SaveChanges());

            Assert.Empty(context.Parents.ToList());
        }

        /// <summary>
        /// SQLite is slower than In-memory DB.
        /// Use it with SQL queries and when testing for constrains.
        /// </summary>
        [Fact]
        [Trait("Performance", "SQLite")]
        public void PerformanceTestSqlite()
        {
            var context = GetDbContext();
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

            context.SaveChanges();
        }
    }
}

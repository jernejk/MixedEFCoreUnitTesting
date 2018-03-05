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
            context.Parents.Add(new ParentData
            {
                Name = "Parent name",
                Child = new ChildData
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
        }

        [Fact]
        public async Task ShouldWork()
        {
            // Prepare
            var context = GetDbContext();
            context.Parents.Add(new ParentData
            {
                Name = "Parent name",
                Child = new ChildData
                {
                    Name = "Child name"
                }
            });

            // Checks if Child property is correctly populated
            context.SaveChanges();

            // Execute
            var result = await context.Database.GetDbConnection()
                .QueryAsync<ParentData>(@"select * from Parents");

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
            var child = new ChildData
            {
                Name = "Child name"
            };
            context.Childs.Add(child);
            context.SaveChanges();

            // Add a child with non-existing ID.
            context.Parents.Add(new ParentData
            {
                Name = "Parent name",
                ChildId = child.Id + 1
            });

            // Execute and assert
            // Will throw expected exception.
            Assert.Throws<DbUpdateException>(() => context.SaveChanges());
        }

        /// <summary>
        /// SQLite is slower than In-memory DB.
        /// Use it with SQL queries and when testing for constrains.
        /// </summary>
        [Fact]
        public void PerformanceTestSqlite()
        {
            var context = GetDbContext();
            for (int i = 0; i < 1000; ++i)
            {
                context.Parents.Add(new ParentData
                {
                    Name = "Parent name",
                    Child = new ChildData
                    {
                        Name = "Child name"
                    }
                });
            }

            context.SaveChanges();
        }
    }
}

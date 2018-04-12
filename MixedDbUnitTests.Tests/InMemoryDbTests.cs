using Dapper;
using Microsoft.EntityFrameworkCore;
using MixedDbUnitTests.Persistance.Domain;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace MixedDbUnitTests.Tests
{
    public class InMemoryDbTests : TestBase
    {
        // Added new lines to have same line numbers as
        // the Sqlite unit test file.
        public InMemoryDbTests() { }


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
        public async Task ShouldNotBeAbleToWorkCorrectly()
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

            // Assert (will not reach)
            Assert.Single(result);
        }

        /// <summary>
        /// SQL allows you to check if constrains are set correctly.
        /// In-memory DB doesn't check for constrains.
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
            // Will because no exception is thrown.
            Assert.Throws<DbUpdateException>(
                () => context.SaveChanges());

            Assert.Empty(context.Parents.ToList());
        }

        /// <summary>
        /// In-memory DB is faster than Sqlite.
        /// Use it when constrains are not required.
        /// </summary>
        [Fact]
        [Trait("Performance", "InMemory")]
        public void PerformanceTestInMemory()
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

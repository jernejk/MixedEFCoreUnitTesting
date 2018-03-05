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
        // Added space to have same line numbers as
        // the Sqlite unit test file.



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
        public async Task ShouldNotBeAbleToWorkCorrectly()
        {
            var context = GetDbContext();
            context.Parents.Add(new ParentData
            {
                Name = "Parent name",
                Child = new ChildData
                {
                    Name = "Child name"
                }
            });

            // Will not fail even though Child is required.
            context.SaveChanges();

            // Execute
            // This fails because in-memory-database does not support SQL.
            var result = await context.Database.GetDbConnection()
                .QueryAsync<ParentData>(@"select * from Parents");

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
            // Will because no exception is thrown.
            Assert.Throws<DbUpdateException>(() => context.SaveChanges());
        }

        /// <summary>
        /// In-memory DB is faster than Sqlite.
        /// Use it when constrains are not required.
        /// </summary>
        [Fact]
        public void PerformanceTestInMemory()
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

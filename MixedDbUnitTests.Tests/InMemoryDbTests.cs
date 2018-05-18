using Microsoft.EntityFrameworkCore;
using MixedDbUnitTests.Persistance.Domain;
using System;
using System.Linq;
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
            context.SaveChanges();

            // Execute
            var data = context.Parents.ToList();

            // Assert
            Assert.Single(data);
            Assert.Contains(data, d => d.Id == id);
            Assert.Contains(data, d => d.Name == "Parent name");
            Assert.Contains(data, d => d.Child.Name == "Child name");
        }

        [Fact]
        public void ShouldNotBeAbleToExecuteSql()
        {
            // Prepare
            var context = GetDbContext();

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

            context.SaveChanges();

            // Execute
            var result = context.Parents
                .FromSql(@"select * from Parents")
                .ToList();

            // Assert
            // This line will never be reached.
            Assert.Equal(id, result.First().Id);
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
                ChildId = Guid.NewGuid()
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

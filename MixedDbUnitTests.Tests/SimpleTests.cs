using Dapper;
using Microsoft.EntityFrameworkCore;
using MixedDbUnitTests.Persistance.Domain;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace MixedDbUnitTests.Tests
{
    public class SimpleTests : TestBase
    {
        [Fact]
        public void ShouldBeAbleToAddAndGetEntity()
        {
            // Prepare
            var context = GetDbContext();
            context.TestDatas.Add(new TestData
            {
                Text = "Test"
            });
            context.SaveChanges();

            // Execute
            var data = context.TestDatas.ToList();

            // Assert
            Assert.Single(data);
            Assert.Contains(data, d => d.Text == "Test");
        }

        [Fact]
        public void ShouldBeAbleToIgnoreDeletedEntity()
        {
            // Prepare
            var context = GetDbContext();
            context.TestDatas.Add(new TestData
            {
                Text = "Deleted",
                IsDeleted = true
            });
            context.TestDatas.Add(new TestData
            {
                Text = "Test",
                IsDeleted = false
            });
            context.SaveChanges();

            // Execute
            var data = context.TestDatas.ToList();
            Assert.Single(data);
            Assert.All(data, d => Assert.False(d.IsDeleted));
        }

        [Fact]
        public void ShouldBeAbleToIgnoreComplexDeletedEntity()
        {
            // Prepare
            var context = GetDbContext();
            context.ComlexDatas.Add(new ComplexData
            {
                Text = "Deleted"
            });
            context.ComlexDatas.Add(new ComplexData
            {
                Text = "Test",
                IsDeleted = false
            });
            context.SaveChanges();
            context.ComlexDatas.Remove(context.ComlexDatas.FirstOrDefault());
            context.SaveChanges();

            // Execute
            var data = context.ComlexDatas.ToList();
            Assert.Single(data);
            Assert.All(data, d => Assert.False(d.IsDeleted));
        }

        [Fact]
        public async Task ShouldNotWork()
        {
            // Prepare
            UseSqlite();

            var context = GetDbContext();
            context.TestDatas.Add(new TestData
            {
                Text = "Test",
                IsDeleted = false
            });
            context.SaveChanges();
            
            // Execute
            var data = await context.Database.GetDbConnection()
                .QueryAsync<TestData>(@"select * from TestDatas");

            // Assert
            Assert.Single(data);
        }

        /// <summary>
        /// Use in-memory DB if SQL queries or relationship constraints are not required.
        /// In-memory DB is about 5 times faster. (the code below takes about 2s while SQLite takes about 13s)
        /// </summary>
        [Fact]
        public void PerformanceTestInMemory()
        {
            var context = GetDbContext();

            for (int i = 0; i < 100000; ++i)
            {
                context.TestDatas.Add(new TestData
                {
                    Text = "Test",
                    IsDeleted = false
                });
            }

            context.SaveChanges();
        }

        /// <summary>
        /// SQLite will take almost 5 times longer than in-memory DB.
        /// </summary>
        [Fact]
        public void PerformanceTestSqlite()
        {
            UseSqlite();

            var context = GetDbContext();
            for (int i = 0; i < 100000; ++i)
            {
                context.TestDatas.Add(new TestData
                {
                    Text = "Test",
                    IsDeleted = false
                });
            }

            context.SaveChanges();
        }
    }
}

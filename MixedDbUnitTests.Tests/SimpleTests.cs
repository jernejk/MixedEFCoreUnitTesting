using MixedDbUnitTests.Persistance.Domain;
using System.Linq;
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
                Data = "Test"
            });
            context.SaveChanges();

            // Execute
            var data = context.TestDatas.ToList();

            // Assert
            Assert.Single(data);
            Assert.Contains(data, d => d.Data == "Test");
        }

        [Fact]
        public void ShouldBeAbleToIgnoreDeletedEntity()
        {
            // Prepare
            var context = GetDbContext();
            context.TestDatas.Add(new TestData
            {
                Data = "Deleted",
                IsDeleted = true
            });
            context.TestDatas.Add(new TestData
            {
                Data = "Test",
                IsDeleted = false
            });
            context.SaveChanges();

            // Execute
            var data = context.TestDatas.ToList();
            Assert.Single(data);
            Assert.All(data, d => Assert.False(d.IsDeleted));
        }
    }
}

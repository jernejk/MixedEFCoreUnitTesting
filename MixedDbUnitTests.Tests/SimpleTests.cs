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
            var context = GetDbContext();
            context.TestDatas.Add(new TestData
            {
                Data = "Test"
            });

            context.SaveChanges();

            var data = context.TestDatas.ToList();

            Assert.Single(data);
            Assert.Contains(data, d => d.Data == "Test");
        }
    }
}

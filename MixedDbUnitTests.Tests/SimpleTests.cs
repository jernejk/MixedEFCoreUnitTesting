using Xunit;

namespace MixedDbUnitTests.Tests
{
    public class SimpleTests : TestBase
    {
        [Fact]
        public void ShouldBeAbleToAddAndGet()
        {
            var context = GetDbContext();

            Assert.NotNull(context);
        }
    }
}

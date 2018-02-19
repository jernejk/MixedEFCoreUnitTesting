using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.DependencyInjection;
using MixedDbUnitTests.Persistance;
using System;

namespace MixedDbUnitTests.Tests
{
    public abstract class TestBase
    {
        public SampleDbContext GetDbContext()
        {
            var serviceCollection = new ServiceCollection();
            serviceCollection.AddDbContext<SampleDbContext>(o =>
            {
                o.UseInMemoryDatabase(Guid.NewGuid().ToString()).ConfigureWarnings(w =>
                {
                    w.Ignore(InMemoryEventId.TransactionIgnoredWarning);
                }).EnableSensitiveDataLogging(true);
            });

            var serviceProvider = serviceCollection.BuildServiceProvider();
            return serviceProvider.GetService<SampleDbContext>();
        }
    }
}

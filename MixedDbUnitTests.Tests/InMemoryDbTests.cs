using Microsoft.EntityFrameworkCore;
using MixedDbUnitTests.Persistance.Domain;
using Xunit;

namespace MixedDbUnitTests.Tests;

public class InMemoryDbTests : TestBase
{
    // Added new lines to have same line numbers as
    // the Sqlite unit test file.
    public InMemoryDbTests() { }


    [Fact]
    public async Task ShouldBeAbleToAddAndGetEntity()
    {
        // Prepare
        using var context = await GetDbContext();
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
        await context.SaveChangesAsync();

        // Execute
        var data = await context.Parents.ToListAsync();

        // Assert
        Assert.Single(data);
        Assert.Contains(data, d => d.Id == id);
        Assert.Contains(data, d => d.Name == "Parent name");
        Assert.Contains(data, d => d.Child.Name == "Child name");
    }

    [Fact]
    [Trait("DbDependat", "")]
    public async Task ShouldNotBeAbleToExecuteSql()
    {
        // Prepare
        using var context = await GetDbContext();

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

        await context.SaveChangesAsync();

        // NOTE: This will fail on InMemory DB because we can't execute SQL.
        var result = await context.Parents
            .FromSqlRaw("select * from Parents")
            .ToListAsync();

        // This line will never be reached.
        Assert.Equal(id, result.First().Id);
    }

    /// <summary>
    /// SQL allows you to check if constrains are set correctly.
    /// In-memory DB doesn't check for constrains.
    /// </summary>
    [Fact]
    [Trait("DbDependat", "")]
    public async Task ShouldFailWhenIncludeIsNotUsed()
    {
        // Prepare
        using var context = await GetDbContext();

        // Add a child with non-existing ID.
        context.Parents.Add(new Parent
        {
            Name = "Parent name",
            ChildId = Guid.NewGuid()
        });

        // Execute and assert
        // NOTE: Will fail because no exception was thrown.
        await Assert.ThrowsAsync<DbUpdateException>(
            () => context.SaveChangesAsync());

        // This line will never be reached.
        var parents = await context.Parents.ToListAsync();
        Assert.Empty(parents);
    }

    /// <summary>
    /// In-memory DB is faster than Sqlite.
    /// Use it when constrains are not required.
    /// </summary>
    [Fact]
    [Trait("Performance", "InMemory")]
    public async Task PerformanceTestInMemory()
    {
        using var context = await GetDbContext();
        for (int i = 0; i < Constants.NumberOfEntitiesToAdd; ++i)
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

        await context.SaveChangesAsync();
    }

    /// <summary>
    /// In-memory DB is faster than Sqlite.
    /// AddRange is faster for SQLite but is the same for in-memory DB.
    /// </summary>
    [Fact]
    [Trait("Performance", "InMemory")]
    public async Task PerformanceTestBitFaster()
    {
        using var context = await GetDbContext();

        var parents = new List<Parent>(Constants.NumberOfEntitiesToAdd);
        for (int i = 0; i < Constants.NumberOfEntitiesToAdd; ++i)
        {
            parents.Add(new Parent
            {
                Name = "Parent name",
                Child = new Child
                {
                    Name = "Child name"
                }
            });
        }

        context.Parents.AddRange(parents);
        await context.SaveChangesAsync();
    }
}

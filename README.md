**UPDATE December 2023:** Updated for .NET 8! While the talk is originally for EF Core 2.1, majority of the points as well as code is still relevant in EF Core 8!

# Mixed EF Core unit testing with in-memory and SQLite DBs

When doing unit tests for EF Core, we can use in-memory-database to test functionalities but sometimes we needs to use SQL features. That's where SQLite comes into play.

When running the tests, 2 tests will fail:

- InMemoryDbTests.ShouldFailWhenIncludeIsNotUsed
- InMemoryDbTests.ShouldNotBeAbleToExecuteSql

This is intended to demonstrate when and why EF Core In-Memory DB provider won't work and SQLite is a better choice.

## Original content for this source code

Original blog post: [.NET Core complex EF Core unit testing](https://jkdev.me/ef-core-unit-tests/) (February 2018)

[![EF Core Unit Testing with SQLite YouTube video](/assets/blog-ef-core-deb-2018-yt.png)](https://www.youtube.com/watch?v=PppmuvsFO78)

## Additional content on C# unit tests and EF Core 

[![Making unit tests simple again with .Net Core and EF Core | Jernej Kavka at DDD Sydney 2018](/assets/ddd-sydney-2018-ef-core-unit-tests-yt.png)](https://www.youtube.com/watch?v=PppmuvsFO78)

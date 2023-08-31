using Testcontainers.MsSql;

namespace ToDoApp.Web.Tests;

public class DatabaseFixture : IAsyncLifetime
{
    public MsSqlContainer MsSqlContainer { get; } = new MsSqlBuilder().Build();

    public Task InitializeAsync() => MsSqlContainer.StartAsync();

    public Task DisposeAsync() => MsSqlContainer.DisposeAsync().AsTask();
}
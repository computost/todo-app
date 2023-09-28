using Microsoft.EntityFrameworkCore;
using Testcontainers.MsSql;
using ToDoApp.Application;
using ToDoApp.Infrastructure;

namespace ToDoApp.Web.Tests;

public class DatabaseFixture : IAsyncLifetime
{
    public MsSqlContainer MsSqlContainer { get; } = new MsSqlBuilder().Build();
    public ToDosContext ToDosContext { get; private set; } = null!;

    public async Task InitializeAsync()
    {
        await MsSqlContainer.StartAsync();
        ToDosContext = new ToDosContextImpl(
            new DbContextOptionsBuilder<ToDosContextImpl>()
                .UseSqlServer(MsSqlContainer.GetConnectionString())
                .Options
        );
        await ToDosContext.Database.EnsureCreatedAsync();
    }

    public async Task DisposeAsync()
    {
        await ToDosContext.DisposeAsync();
        await MsSqlContainer.DisposeAsync();
    }
}
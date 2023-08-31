using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Testcontainers.MsSql;
using ToDoApp.Domain;

namespace ToDoApp.Web.Tests;

public class DatabaseFixture : IAsyncLifetime
{
    public MsSqlContainer MsSqlContainer { get; } = new MsSqlBuilder().Build();
    public ToDosContext ToDosContext { get; private set; }

    public async Task InitializeAsync()
    {
        await MsSqlContainer.StartAsync();
        ToDosContext = new ToDosContext(
            new DbContextOptionsBuilder<ToDosContext>()
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
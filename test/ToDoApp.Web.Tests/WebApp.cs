using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Net;
using System.Net.Http.Json;
using ToDoApp.Domain;

namespace ToDoApp.Web.Tests;

public class WebApp : IAsyncLifetime, IClassFixture<DatabaseFixture>
{
    private readonly DatabaseFixture _databaseFixture;
    private WebApplicationFactory<Program> _factory = null!;
    private ToDosContext _dbContext = null!;
    private HttpClient _client = null!;

    #region set up and tear down

    public WebApp(DatabaseFixture databaseFixture)
    {
        _databaseFixture = databaseFixture;
    }

    public async Task InitializeAsync()
    {
        _factory = new WebApplicationFactory<Program>()
            .WithWebHostBuilder(builder =>
                builder.ConfigureServices(services =>
                    services.AddDbContext<ToDosContext>(
                        options => options.UseSqlServer(
                            _databaseFixture.MsSqlContainer.GetConnectionString()
                        ),
                        ServiceLifetime.Singleton
                    )
                )
            );
        _dbContext = _factory.Services.GetRequiredService<ToDosContext>();
        await _dbContext.Database.EnsureCreatedAsync();
        _client = _factory.CreateClient();
    }

    public async Task DisposeAsync()
    {
        _client.Dispose();
        await _factory.DisposeAsync();
    }

    #endregion

    [Theory]
    [InlineData("Make an app")]
    [InlineData("Test the app")]
    public async Task CreateToDo(string name)
    {
        // Act
        var response = await _client.PostAsJsonAsync("todos", name);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Created);
        var toDo = await response.Content.ReadFromJsonAsync<ToDo>();
        _dbContext
            .ToDos.Should().ContainEquivalentOf(toDo)
            .Which.Should().BeEquivalentTo(
                new Domain.Entities.ToDo
                {
                    Name = name,
                    IsDone = false
                },
                options => options.Excluding(toDo => toDo.Id)
            );
    }
}
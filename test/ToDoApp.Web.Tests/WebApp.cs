using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Net.Http.Json;
using ToDoApp.Application;

namespace ToDoApp.Web.Tests;

public class WebApp : IAsyncLifetime, IClassFixture<DatabaseFixture>
{
    private readonly DatabaseFixture _databaseFixture;
    private WebApplicationFactory<Program> _factory = null!;
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
                builder.UseSetting("ConnectionStrings::Sql", _databaseFixture.MsSqlContainer.GetConnectionString())
            );
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
        response.Should().Be201Created();
        var toDo = await response.Content.ReadFromJsonAsync<ToDo>();
        _databaseFixture.ToDosContext
            .ToDos.Should().ContainEquivalentOf(toDo)
            .Which.Should().BeEquivalentTo(
                new
                {
                    Name = name,
                    IsDone = false
                }
            );
    }
}
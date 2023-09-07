using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Net.Http.Json;
using ToDoApp.Application;

namespace ToDoApp.Web.Tests.Features;

public class CreateToDo : IAsyncLifetime, IClassFixture<DatabaseFixture>
{
    private readonly DatabaseFixture _databaseFixture;
    private WebApplicationFactory<Program> _factory = null!;
    private HttpClient _client = null!;

    #region set up and tear down

    public CreateToDo(DatabaseFixture databaseFixture)
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
    public async Task WithName(string name)
    {
        await WhenRequestingToCreateAToDoWithName(name);
        ThenTheResponseShouldBe201Created();
        AndTheToDoResponseIsStoredInTheDatabase();
        AndTheToDoResponseShouldHaveName(name);
        AndTheToDoResponseShouldNotBeDone();
    }

    HttpResponseMessage _theResponse;

    private async Task WhenRequestingToCreateAToDoWithName(string name) =>
        _theResponse = await _client.PostAsJsonAsync("todos", name);

    private void ThenTheResponseShouldBe201Created() =>
        _theResponse.Should().Be201Created();

    private void AndTheToDoResponseIsStoredInTheDatabase() =>
        _theResponse.Should().Satisfy<ToDo>(theToDo =>
            _databaseFixture.ToDosContext.ToDos.Should().ContainEquivalentOf(theToDo)
        );

    private void AndTheToDoResponseShouldHaveName(string name) =>
        _theResponse.Should().Satisfy<ToDo>(theToDo =>
            theToDo.Should().BeEquivalentTo(new { Name = name })
        );

    private void AndTheToDoResponseShouldNotBeDone() =>
        _theResponse.Should().Satisfy<ToDo>(theToDo =>
            theToDo.Should().BeEquivalentTo(new { IsDone = false })
        );
}
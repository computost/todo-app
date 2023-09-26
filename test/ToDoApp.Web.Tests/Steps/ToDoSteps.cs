using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using ToDoApp.Application;

namespace ToDoApp.Web.Tests.Steps;

public class ToDoSteps : IAsyncLifetime
{
    private readonly DatabaseFixture _databaseFixture;
    private WebApplicationFactory<Program> _factory = null!;
    private HttpClient _client = null!;

    #region set up and tear down

    protected ToDoSteps(DatabaseFixture databaseFixture)
    {
        _databaseFixture = databaseFixture;
    }

    public Task InitializeAsync()
    {
        _factory = new WebApplicationFactory<Program>()
            .WithWebHostBuilder(builder =>
                builder.UseSetting("ConnectionStrings::Sql", _databaseFixture.MsSqlContainer.GetConnectionString())
            );
        _client = _factory.CreateClient();
        return Task.CompletedTask;
    }

    public async Task DisposeAsync()
    {
        _client.Dispose();
        await _factory.DisposeAsync();
    }

    #endregion

    private int? _toDoId;
    private HttpResponseMessage? _theResponse;

    protected async Task GivenAToDoInTheDatabase()
    {
        var toDo = new Domain.Entities.ToDo(string.Empty);
        await _databaseFixture.ToDosContext.ToDos.AddAsync(toDo);
        await _databaseFixture.ToDosContext.SaveChangesAsync();
        _toDoId = toDo.Id;
    }

    protected async Task WhenRequestingToCreateAToDoWithName(string name) =>
        _theResponse = await _client.PostAsJsonAsync("todos", name);

    protected async Task WhenRequestingToCompleteTheToDo() =>
        _theResponse = await _client.PostAsync($"todos/{_toDoId}/complete", null);
    
    protected async Task WhenRequestingToDeleteTheToDo() =>
        _theResponse = await _client.DeleteAsync($"todos/{_toDoId}");

    protected void ThenTheResponseShouldBe201Created() =>
        _theResponse.Should().Be201Created();

    protected void ThenTheResponseShouldBe200Ok() =>
        _theResponse.Should().Be200Ok();
    
    protected void ThenTheResponseShouldBe204NoContent() =>
        _theResponse.Should().Be204NoContent();

    protected void AndTheToDoResponseIsStoredInTheDatabase()
    {
        _databaseFixture.ToDosContext.ChangeTracker.Clear();
        _theResponse.Should().Satisfy<ToDo>(theToDo =>
            _databaseFixture.ToDosContext.ToDos.Should().ContainEquivalentOf(theToDo)
        );
    }
    
    protected void AndTheToDoResponseIsNotStoredInTheDatabase()
    {
        _databaseFixture.ToDosContext.ChangeTracker.Clear();
        _databaseFixture.ToDosContext.ToDos.Should().NotContain(theToDo => theToDo.Id == _toDoId);
    }

    protected void AndTheToDoResponseShouldHaveName(string name) =>
        _theResponse.Should().Satisfy<ToDo>(theToDo =>
            theToDo.Should().BeEquivalentTo(new { Name = name })
        );

    protected void AndTheToDoResponseShouldBeDone() =>
        _theResponse.Should().Satisfy<ToDo>(theToDo =>
            theToDo.Should().BeEquivalentTo(new { IsDone = true })
        );

    protected void AndTheToDoResponseShouldNotBeDone() =>
        _theResponse.Should().Satisfy<ToDo>(theToDo =>
            theToDo.Should().BeEquivalentTo(new { IsDone = false })
        );
}

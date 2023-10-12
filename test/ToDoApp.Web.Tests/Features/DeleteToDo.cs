using ToDoApp.Web.Tests.Steps;

namespace ToDoApp.Web.Tests.Features;

public class DeleteToDo : ToDoSteps, IClassFixture<DatabaseFixture>
{
    public DeleteToDo(DatabaseFixture databaseFixture) : base(databaseFixture) { }

    [Fact]
    public async Task ExistingToDo()
    {
        await GivenAToDoInTheDatabase();
        await WhenRequestingToDeleteTheToDo();
        ThenTheResponseShouldBe204NoContent();
        AndTheToDoResponseIsNotStoredInTheDatabase();
    }

    [Fact]
    public async Task NonExistingToDo()
    {
        await WhenRequestingToDeleteTheToDo();
        ThenTheResponseShouldBe404NotFound();
    }
}
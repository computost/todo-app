using ToDoApp.Web.Tests.Steps;

namespace ToDoApp.Web.Tests.Features;

public class MarkToDoComplete : ToDoSteps, IClassFixture<DatabaseFixture>
{
    public MarkToDoComplete(DatabaseFixture databaseFixture) : base(databaseFixture)
    {
    }

    [Fact]
    public async Task IncompleteToDo()
    {
        await GivenAToDoInTheDatabase();
        await WhenRequestingToCompleteTheToDo();
        ThenTheResponseShouldBe200Ok();
        AndTheToDoResponseIsStoredInTheDatabase();
        AndTheToDoResponseShouldBeDone();
    }
    
    [Fact]
    public async Task ToDoIsNotInTheDatabase()
    {
        await WhenRequestingToCompleteTheToDo();
        ThenTheResponseShouldBe404NotFound();
    }
    
    // TODO ToDoIsComplete
}

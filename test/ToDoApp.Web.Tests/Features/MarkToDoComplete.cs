using ToDoApp.Web.Tests.Steps;

namespace ToDoApp.Web.Tests.Features;

public class MarkToDoComplete : ToDoSteps, IClassFixture<DatabaseFixture>
{
    public MarkToDoComplete(DatabaseFixture databaseFixture) : base(databaseFixture)
    {
    }

    [Fact]
    public async Task ToDoIsIncomplete()
    {
        await GivenAToDoInTheDatabase();
        await WhenRequestingToCompleteTheToDo();
        ThenTheResponseShouldBe200Ok();
        AndTheToDoResponseIsStoredInTheDatabase();
        AndTheToDoResponseShouldBeDone();
    }

    // TODO ToDoIsNotInTheDatabase
    // TODO ToDoIsComplete
}

using ToDoApp.Web.Tests.Steps;

namespace ToDoApp.Web.Tests.Features;

public class GetToDo : ToDoSteps, IClassFixture<DatabaseFixture>
{
    public GetToDo(DatabaseFixture databaseFixture) : base(databaseFixture) { }

    [Fact]
    public async Task ExistingToDo()
    {
        await GivenAToDoInTheDatabase("Make an app");
        await WhenFetchingTheToDo();
        ThenTheResponseShouldBe200Ok();
        AndTheToDoResponseShouldHaveName("Make an app");
    }
}

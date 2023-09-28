using ToDoApp.Web.Tests.Steps;

namespace ToDoApp.Web.Tests.Features;

public class CreateToDo : ToDoSteps, IClassFixture<DatabaseFixture>
{
    public CreateToDo(DatabaseFixture databaseFixture) : base(databaseFixture) { }

    [Theory]
    [InlineData("Make an app")]
    [InlineData("Test the app")]
    public async Task ValidName(string name)
    {
        await WhenRequestingToCreateAToDoWithName(name);
        ThenTheResponseShouldBe201Created();
        AndTheToDoResponseIsStoredInTheDatabase();
        AndTheToDoResponseShouldHaveName(name);
        AndTheToDoResponseShouldNotBeDone();
    }
}
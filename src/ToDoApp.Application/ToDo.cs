namespace ToDoApp.Application;

public record ToDo
{
    public required int Id { get; init; }
    public required string Name { get; init; }
    public required bool IsDone { get; init; }
}

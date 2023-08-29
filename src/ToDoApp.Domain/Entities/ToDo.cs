namespace ToDoApp.Web.Entities;

public class ToDo
{
    public int? Id { get; set; }

    public required string Name { get; set; }

    public required bool IsDone { get; set; }
}
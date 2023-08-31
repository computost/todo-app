using System.Diagnostics.CodeAnalysis;

namespace ToDoApp.Domain.Entities;

public class ToDo
{
    public  int? Id { get; [ExcludeFromCodeCoverage] set; }

    public required string Name { get; set; }

    public required bool IsDone { get; set; }
}
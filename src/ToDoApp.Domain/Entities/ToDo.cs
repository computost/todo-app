namespace ToDoApp.Domain.Entities;

public class ToDo
{
    public ToDo(string name)
    {
        Name = name;
        IsDone = false;
    }

    public int? Id { get; private set; }
    public string Name { get; private set; }
    public bool IsDone { get; private set; }

    public void Complete() => IsDone = true;
}
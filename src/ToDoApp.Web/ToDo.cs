namespace ToDoApp.Web;

public record CreateToDo(string Name);

public record ToDo(int Id, string Name, bool IsDone);

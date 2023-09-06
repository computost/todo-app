namespace ToDoApp.Application;

public class ToDoService
{
    private readonly ToDosContext _toDosContext;
    public ToDoService(ToDosContext toDosContext)
    {
        _toDosContext = toDosContext;
    }
    
    public async Task<ToDo> CreateToDo(string name, CancellationToken cancellationToken)
    {
        var toDo = new Domain.Entities.ToDo(name);

        await _toDosContext.ToDos.AddAsync(toDo, cancellationToken);
        await _toDosContext.SaveChangesAsync(cancellationToken);

        return new ToDo(toDo.Id!.Value, toDo.Name, toDo.IsDone);
    }
}
using Microsoft.EntityFrameworkCore;

namespace ToDoApp.Application;

public class ToDoService
{
    private readonly ToDosContext _toDosContext;
    public ToDoService(ToDosContext toDosContext)
    {
        _toDosContext = toDosContext;
    }

    public async Task<ToDo> Create(string name, CancellationToken cancellationToken)
    {
        var toDo = new Domain.Entities.ToDo(name);

        await _toDosContext.ToDos.AddAsync(toDo, cancellationToken);
        await _toDosContext.SaveChangesAsync(cancellationToken);

        return new ToDo(toDo.Id!.Value, toDo.Name, toDo.IsDone);
    }

    public async Task<ToDo> Complete(int id, CancellationToken cancellationToken)
    {
        var toDo = await _toDosContext.ToDos.FindAsync(id, cancellationToken);

        toDo.Complete();

        await _toDosContext.SaveChangesAsync(cancellationToken);
        return new ToDo(toDo.Id!.Value, toDo.Name, toDo.IsDone);
    }
}
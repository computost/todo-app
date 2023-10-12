using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace ToDoApp.Application;

public class ToDoService
{
    private readonly ToDosContext _toDosContext;
    private readonly IMapper _mapper;

    public ToDoService(ToDosContext toDosContext, IMapper mapper)
    {
        _toDosContext = toDosContext;
        _mapper = mapper;
    }

    public async Task<ToDo> Create(string name, CancellationToken cancellationToken)
    {
        var toDo = new Domain.Entities.ToDo(name);

        await _toDosContext.ToDos.AddAsync(toDo, cancellationToken);
        await _toDosContext.SaveChangesAsync(cancellationToken);

        return _mapper.Map<ToDo>(toDo);
    }

    public async Task<ToDo?> Complete(int id, CancellationToken cancellationToken)
    {
        var toDo = await _toDosContext.ToDos.FindAsync(id, cancellationToken);

        if (toDo is null) return null;
        toDo.Complete();
        await _toDosContext.SaveChangesAsync(cancellationToken);
        return _mapper.Map<ToDo>(toDo);
    }

    public async Task Delete(int id, CancellationToken cancellationToken)
    {
        var toDo = await _toDosContext.ToDos.FindAsync(id, cancellationToken);

        toDo.Delete();

        await _toDosContext.SaveChangesAsync(cancellationToken);
    }

    public Task<ToDo> Get(int id, CancellationToken cancellationToken)
        => _mapper.ProjectTo<ToDo>(_toDosContext.ToDos)
            .FirstAsync(toDo => toDo.Id == id, cancellationToken);
}
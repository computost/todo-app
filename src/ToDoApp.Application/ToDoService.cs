using AutoMapper;
using FluentResults;
using Microsoft.EntityFrameworkCore;
using ToDoApp.Application.Errors;

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

    public async Task<Result<ToDo>> Complete(int id, CancellationToken cancellationToken)
    {
        var toDo = await _toDosContext.ToDos.FindAsync(id, cancellationToken);

        if (toDo is null) return Result.Fail(new NotFoundError());
        toDo.Complete();
        await _toDosContext.SaveChangesAsync(cancellationToken);
        return _mapper.Map<ToDo>(toDo);
    }

    public async Task<Result> Delete(int id, CancellationToken cancellationToken)
    {
        var toDo = await _toDosContext.ToDos.FindAsync(id, cancellationToken);

        if (toDo is null) return Result.Fail(new NotFoundError());
        toDo.Delete();

        await _toDosContext.SaveChangesAsync(cancellationToken);
        return Result.Ok();
    }

    public async Task<Result<ToDo>> Get(int id, CancellationToken cancellationToken)
    {
        var todo = await _mapper.ProjectTo<ToDo>(_toDosContext.ToDos)
            .FirstOrDefaultAsync(toDo => toDo.Id == id, cancellationToken);
        
        if(todo is null) return Result.Fail(new NotFoundError());
        return todo;
    }
}
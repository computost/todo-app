using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace ToDoApp.Web.Controllers;

[ApiController]
[Route("todos")]
public class ToDosController : ControllerBase
{
    private readonly ToDosContext _toDosContext;
    public ToDosController(ToDosContext toDosContext)
    {
        _toDosContext = toDosContext;
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateToDo createToDo, CancellationToken cancellationToken)
    {
        var toDo = new Entities.ToDo
        {
            Name = createToDo.Name,
            IsDone = false
        };

        await _toDosContext.ToDos.AddAsync(toDo, cancellationToken);
        await _toDosContext.SaveChangesAsync(cancellationToken);

        Debug.Assert(toDo.Id is not null);

        return Created(
            string.Empty,
            new ToDo(toDo.Id.Value, toDo.Name, toDo.IsDone)
        );
    }
}
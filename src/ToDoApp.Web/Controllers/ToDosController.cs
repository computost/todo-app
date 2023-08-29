using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using ToDoApp.Domain;

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
    public async Task<IActionResult> Create(
        [FromBody] string name,
        CancellationToken cancellationToken
    )
    {
        var toDo = new Domain.Entities.ToDo
        {
            Name = name,
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
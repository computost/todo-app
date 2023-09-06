using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using ToDoApp.Application;
using ToDoApp.Domain;

namespace ToDoApp.Web.Controllers;

[ApiController]
[Route("todos")]
public class ToDosController : ControllerBase
{
    private readonly ToDoService _toDoService;
    public ToDosController(ToDoService toDoService)
    {
        _toDoService = toDoService;
    }

    [HttpPost]
    public async Task<IActionResult> Create(
        [FromBody] string name,
        CancellationToken cancellationToken
    )
    {
        var todo = await _toDoService.CreateToDo(name, cancellationToken);
        return Created(string.Empty, todo);
    }
}
using FluentResults;
using Microsoft.AspNetCore.Mvc;
using ToDoApp.Application;
using ToDoApp.Application.Errors;

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
        var todo = await _toDoService.Create(name, cancellationToken);
        return Created(string.Empty, todo);
    }

    [HttpPost, Route("{id}/complete")]
    public async Task<IActionResult> Complete(
        int id,
        CancellationToken cancellationToken
    )
    {
        var result = await _toDoService.Complete(id, cancellationToken);
        if (result.HasError<NotFoundError>()) return NotFound();
        return Ok(result.Value);
    }

    [HttpDelete, Route("{id}")]
    public async Task<IActionResult> Delete(
        int id,
        CancellationToken cancellationToken
    )
    {
        var result = await _toDoService.Delete(id, cancellationToken);
        if (result.HasError<NotFoundError>()) return NotFound();
        return NoContent();
    }

    [HttpGet, Route("{id}")]
    public async Task<IActionResult> Get(
        int id,
        CancellationToken cancellationToken
    )
    {
        var result = await _toDoService.Get(id, cancellationToken);
        if (result.HasError<NotFoundError>()) return NotFound();
        return Ok(result.Value);
    }
}
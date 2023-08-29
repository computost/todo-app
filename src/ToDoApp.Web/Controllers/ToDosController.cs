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
    public IActionResult Create(CreateToDo createToDo)
    {
        var toDo = new Entities.ToDo
        {
            Name = createToDo.Name,
            IsDone = false
        };

        _toDosContext.ToDos.Add(toDo);
        _toDosContext.SaveChanges();

        Debug.Assert(toDo.Id is not null);

        return Created(
            string.Empty,
            new ToDo(toDo.Id.Value, toDo.Name, toDo.IsDone)
        );
    }
}
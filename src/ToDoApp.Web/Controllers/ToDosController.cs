using Microsoft.AspNetCore.Mvc;

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
        _toDosContext.ToDos.Add(new Entities.ToDo
        {
            Name = createToDo.Name,
            IsDone = false
        });
        _toDosContext.SaveChanges();
        return Created(string.Empty, new ToDo(1, createToDo.Name, false));
    }
}
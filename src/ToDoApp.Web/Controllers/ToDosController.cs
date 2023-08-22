using Microsoft.AspNetCore.Mvc;

namespace ToDoApp.Web.Controllers;

[ApiController]
[Route("todos")]
public class ToDosController : ControllerBase
{
    [HttpPost]
    public IActionResult Create(CreateToDo createToDo) =>
        Created(string.Empty, new ToDo(1, createToDo.Name, false));
}
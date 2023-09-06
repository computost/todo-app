using Microsoft.EntityFrameworkCore;

namespace ToDoApp.Domain;

public class ToDosContext : DbContext
{
    public DbSet<Entities.ToDo> ToDos { get; set; } = null!;
    public ToDosContext(DbContextOptions<ToDosContext> options) : base(options)
    {

    }
}
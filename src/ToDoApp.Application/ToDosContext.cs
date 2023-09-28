using Microsoft.EntityFrameworkCore;

namespace ToDoApp.Application;

public abstract class ToDosContext : DbContext
{
    protected ToDosContext(DbContextOptions options) : base(options)
    {
    }

    public DbSet<Domain.Entities.ToDo> ToDos { get; set; } = null!;
}

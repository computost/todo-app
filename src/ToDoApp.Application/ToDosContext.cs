using Microsoft.EntityFrameworkCore;

namespace ToDoApp.Application;

public class ToDosContext : DbContext
{
    public DbSet<Domain.Entities.ToDo> ToDos { get; set; } = null!;
    public ToDosContext(DbContextOptions<ToDosContext> options) : base(options)
    {

    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Domain.Entities.ToDo>().Ignore(_ => _.IsDeleted);
        base.OnModelCreating(modelBuilder);
    }
}
using Microsoft.EntityFrameworkCore;
using ToDoApp.Application;

namespace ToDoApp.Infrastructure;

public class ToDosContextImpl : ToDosContext
{
    public ToDosContextImpl(DbContextOptions<ToDosContextImpl> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Domain.Entities.ToDo>().Ignore(_ => _.IsDeleted);
        base.OnModelCreating(modelBuilder);
    }
}

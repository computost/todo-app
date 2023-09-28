using Microsoft.EntityFrameworkCore;
using ToDoApp.Application;

namespace ToDoApp.Infrastructure;

public class ToDosContextImpl : ToDosContext
{
    public ToDosContextImpl(DbContextOptions<ToDosContextImpl> options) : base(options)
    {
        SavingChanges += DeleteToDos;
    }

    private void DeleteToDos(object? sender, SavingChangesEventArgs e)
    {
        foreach (var toDoEntry in ChangeTracker.Entries<Domain.Entities.ToDo>())
        {
            if (toDoEntry.Entity.IsDeleted)
            {
                toDoEntry.State = EntityState.Deleted;
            }
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Domain.Entities.ToDo>().Ignore(_ => _.IsDeleted);
        base.OnModelCreating(modelBuilder);
    }
}

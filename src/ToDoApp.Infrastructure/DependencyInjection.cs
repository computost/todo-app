using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ToDoApp.Application;

namespace ToDoApp.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services.AddDbContext<ToDosContext, ToDosContextImpl>(options =>
            options.UseSqlServer("name=ConnectionStrings::Sql")
        );
        return services;
    }
}
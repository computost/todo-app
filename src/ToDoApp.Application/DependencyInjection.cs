using Microsoft.Extensions.DependencyInjection;

namespace ToDoApp.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddTransient<ToDoService>();
        services.AddAutoMapper(config =>
        {
            config.CreateMap<Domain.Entities.ToDo, ToDo>();
        });
        return services;
    }
}
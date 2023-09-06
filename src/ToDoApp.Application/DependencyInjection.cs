using Microsoft.Extensions.DependencyInjection;

namespace ToDoApp.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddTransient<ToDoService>();
        return services;
    }
}
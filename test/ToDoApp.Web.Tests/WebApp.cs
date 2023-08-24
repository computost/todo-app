using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace ToDoApp.Web.Tests;

public class WebApp
{
    [Fact]
    public async Task CreateToDo()
    {
        await using var factory = new WebApplicationFactory<Program>().WithWebHostBuilder(builder => builder.ConfigureServices(
            services =>
            {
                services.AddDbContext<ToDosContext>(options => options.UseInMemoryDatabase("ToDoControllerTest"), ServiceLifetime.Singleton);
            }));

        using var client = factory.CreateClient();
        var response = await client.PostAsync("todos", JsonContent.Create(new CreateToDo("Make an app")));
        response.StatusCode.Should().Be(HttpStatusCode.Created);
        var toDo = await response.Content.ReadFromJsonAsync<ToDo>();
        toDo.Should().BeEquivalentTo(new ToDo(1, "Make an app", false));
        var dbContext = factory.Services.GetRequiredService<ToDosContext>();
        dbContext.ToDos.Should().ContainEquivalentOf(new Entities.ToDo
        {
            Id = 1,
            Name = "Make an app",
            IsDone = false
        }!);
    }
}

using Microsoft.EntityFrameworkCore;
using MinimalAPI.Data;
using MinimalAPI.Data.Repositories;
using MinimalAPI.Entities;
using MinimalAPI.Interfaces;

namespace MinimalAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddAuthorization();

            builder.Services.AddOpenApi();

            builder.Services.AddDbContext<AppDbContext>(opt =>
                 opt.UseInMemoryDatabase("TestDb"));
           
            builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
            }

            app.UseAuthorization();

            app.MapGet("/", () => "Minimal API .NET 9 Test");

            // Users
            app.MapGet("/users", async (IGenericRepository<User> repo) =>
                Results.Ok(await repo.GetAllAsync()));

            app.MapPost("/users", async (User user, IGenericRepository<User> repo) =>
            {
                await repo.AddAsync(user);
                await repo.SaveAsync();
                return Results.Created($"/users/{user.Id}", user);
            });

            // Orders
            app.MapGet("/orders", async (IGenericRepository<Order> repo) =>
                Results.Ok(await repo.GetAllAsync()));

            app.MapPost("/orders", async (Order order, IGenericRepository<Order> repo) =>
            {
                await repo.AddAsync(order);
                await repo.SaveAsync();
                return Results.Created($"/orders/{order.Id}", order);
            });

            app.Run();
        }
    }
}

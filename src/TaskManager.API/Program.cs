using FluentValidation;
using Microsoft.EntityFrameworkCore;
using TaskManager.Application.Interfaces;
using TaskManager.Application.Validators;
using TaskManager.Infrastructure.Persistence;
using TaskManager.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddControllers();

builder.Services.AddValidatorsFromAssemblyContaining<RegisterRequestValidator>();

builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseMySql(builder.Configuration.GetConnectionString("DefaultConnection"),
    ServerVersion.AutoDetect(builder.Configuration.GetConnectionString("DefaultConnection")));

});

builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<ITaskRepository, TaskRepository>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();



app.Run();

record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}

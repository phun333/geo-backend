using geoproject.Interfaces;
using geoproject.Models;
using geoproject.Services;
using geoproject.Repositories;

var builder = WebApplication.CreateBuilder(args);

// saved the repository as a singleton
// Singleton, Scoped, Transient -> Dependency Injection lifetime management
//! if we dont use one repo for saving points, the all stats always will be empty
builder.Services.AddSingleton<IPointRepository, PointRepository>();

// saved the services as scoped
// for dependency injection
builder.Services.AddScoped<IPointGetAllService, PointGetAllService>();
builder.Services.AddScoped<IPointAddService, PointAddService>();
builder.Services.AddScoped<IPointGetByIdService, PointGetByIdService>();
builder.Services.AddScoped<IPointUpdateService, PointUpdateService>();
builder.Services.AddScoped<IPointDeleteService, PointDeleteService>();

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseSwagger();
app.UseSwaggerUI();

// app.UseHttpsRedirection(); -> i ignore https redirection for simplicity

app.UseAuthorization();
app.MapControllers();

//! i will remove this usage when i publish the project
// this is just for testing purposes

app.Run("http://localhost:5000");


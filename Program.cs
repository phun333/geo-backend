using geoproject.Interfaces;
using geoproject.Models;
using geoproject.Services;
using geoproject.Repositories;
using geoproject.Data;
using geoproject.Resources;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

//! Database connection
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

//! we use postresql so no more using memory shared singleton repository
builder.Services.AddScoped<IPointRepository, PointRepository>();

//! all services are scoped
//! this means that a new instance of the service is created for each request
//! this is important for database operations to ensure that each request has its own context
builder.Services.AddScoped<IPointGetAllService, PointGetAllService>();
builder.Services.AddScoped<IPointAddService, PointAddService>();
builder.Services.AddScoped<IPointGetByIdService, PointGetByIdService>();
builder.Services.AddScoped<IPointUpdateService, PointUpdateService>();
builder.Services.AddScoped<IPointDeleteService, PointDeleteService>();

// Add services to the container.

//! CORS policy for frontend communication
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.WithOrigins("http://localhost:3000")  // React frontend URL
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials();
    });
});

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

//! Auto migrate on startup (Development only)
#if DEBUG
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    try
    {
        context.Database.EnsureCreated(); // Creates database and tables if they dont exist
        Console.WriteLine(Messages.Success.DatabaseConnected);
    }
    catch (Exception ex)
    {
        Console.WriteLine(string.Format(Messages.Errors.DatabaseConnectionFailed, ex.Message));
        Console.WriteLine(Messages.Errors.DatabaseSetupGuide);
    }
}
#endif

// Configure the HTTP request pipeline.
app.UseSwagger();
app.UseSwaggerUI();

//! Enable CORS before authorization
app.UseCors();

// app.UseHttpsRedirection(); -> i ignore https redirection for simplicity

app.UseAuthorization();
app.MapControllers();

//! i will remove this usage when i publish the project
// this is just for testing purposes

app.Run("http://localhost:5000");


using FastEndpoints;
using FastEndpoints.Swagger;
using Microsoft.EntityFrameworkCore;
using TripBookingApi.Data;
using TripBookingApi.Repositories;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseInMemoryDatabase("TripBookingDB"));
builder.Services
    .AddFastEndpoints()
    .SwaggerDocument();
builder.Services.AddTransient<ITripRepository, TripRepository>();
builder.Services.AddFastEndpoints();

var app = builder.Build();

app.UseFastEndpoints()
    .UseDefaultExceptionHandler()
    .UseSwaggerGen();

app.Run();
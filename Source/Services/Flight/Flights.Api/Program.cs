using Carter;
using Core.Endpoints;
using Flights;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<IErrorHandlerFactory, DefaultErrorHandlerFactory>();


builder.Services.AddFlights();
builder.Services.AddCarter();

var app = builder.Build();
app.MapCarter();
app.Run();
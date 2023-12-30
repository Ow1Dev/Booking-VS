using Carter;
using Core.CQRS;
using Flights.Api.Flights.Features.CreatingFlight.V1;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCQRS(typeof(Program).Assembly);
builder.Services.AddCarter();

var app = builder.Build();

app.MapCarter();

app.Run();
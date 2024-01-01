using Carter;
using Flights;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddFlights();
var app = builder.Build();
app.MapCarter();
app.Run();
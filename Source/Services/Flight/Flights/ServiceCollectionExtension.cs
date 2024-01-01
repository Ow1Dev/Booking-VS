using Carter;
using Core.CQRS;
using Flights.Api.Flights.Errors;
using Flights.Flights.Errors;
using Microsoft.Extensions.DependencyInjection;

namespace Flights;

public static class ServiceCollectionExtension
{
    public static IServiceCollection AddFlights(this IServiceCollection services)
    {
        services.AddScoped<IErrorHandlerFactory, DefaultErrorHandlerFactory>();
        services.Decorate<IErrorHandlerFactory, FlightErrorFactory>();
        
        services.AddCQRS(typeof(ServiceCollectionExtension).Assembly);
        services.AddCarter();
        
        return services;
    }
}
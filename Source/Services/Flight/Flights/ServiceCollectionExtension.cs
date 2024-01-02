using Core.CQRS;
using Core.Endpoints;
using Flights.Flights.Errors;
using Microsoft.Extensions.DependencyInjection;

namespace Flights;

public static class ServiceCollectionExtension
{
    public static IServiceCollection AddFlights(this IServiceCollection services)
    {
        services.AddCQRS(typeof(ServiceCollectionExtension).Assembly);
        services.Decorate<IErrorHandlerFactory, FlightErrorFactory>();

        return services;
    }
}
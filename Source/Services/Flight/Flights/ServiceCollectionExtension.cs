using Carter;
using Core.CQRS;
using Microsoft.Extensions.DependencyInjection;

namespace Flights;

public static class ServiceCollectionExtension
{
    public static IServiceCollection AddFlights(this IServiceCollection services)
    {
        services.AddCQRS(typeof(ServiceCollectionExtension).Assembly);
        services.AddCarter();
        
        return services;
    }
}
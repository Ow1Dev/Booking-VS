using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Core.CQRS;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddCQRS(this IServiceCollection services, Assembly assembly)
    {
        services.Scan(selector =>
        {
            selector.FromAssemblies(assembly)
                .AddClasses(filter => { filter.AssignableTo(typeof(IQueryHandler<,>)); })
                .AsImplementedInterfaces()
                .WithTransientLifetime();
        });

        services.Scan(selector =>
        {
            selector.FromAssemblies(assembly)
                .AddClasses(filter => { filter.AssignableTo(typeof(ICommandHandler<,>)); })
                .AsImplementedInterfaces()
                .WithTransientLifetime();
        });

        services.TryAddSingleton<ICommandDispatcher, CommandDispatcher>();
        services.TryAddSingleton<IQueryDispatcher, QueryDispatcher>();
        
        return services;
    }
}
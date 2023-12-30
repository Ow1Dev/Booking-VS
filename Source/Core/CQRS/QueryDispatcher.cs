using Core.ResultTypes;
using Microsoft.Extensions.DependencyInjection;

namespace Core.CQRS;

public interface IQueryDispatcher
{
    Task<Result<TQueryResult>> Dispatch<TQuery, TQueryResult>(TQuery query, CancellationToken cancellation = default)
        where TQuery : IQuery<TQueryResult>;
}

internal class QueryDispatcher : IQueryDispatcher
{
    private readonly IServiceProvider _serviceProvider;

    public QueryDispatcher(IServiceProvider serviceProvider) => _serviceProvider = serviceProvider;

    public Task<Result<TQueryResult>> Dispatch<TQuery, TQueryResult>(TQuery query, CancellationToken cancellation = default)
        where TQuery : IQuery<TQueryResult>
    {
        var handler = _serviceProvider.GetRequiredService<IQueryHandler<TQuery, TQueryResult>>();
        return handler.Handle(query, cancellation);
    }
}
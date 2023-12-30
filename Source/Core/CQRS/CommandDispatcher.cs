using Core.ResultTypes;
using Microsoft.Extensions.DependencyInjection;

namespace Core.CQRS;

public interface ICommandDispatcher
{
    Task<Result<TCommandResult>> Dispatch<TCommand, TCommandResult>(TCommand command, CancellationToken cancellation = default)
        where TCommand : ICommand<TCommandResult>;
}

internal class CommandDispatcher : ICommandDispatcher
{
    private readonly IServiceProvider _serviceProvider;

    public CommandDispatcher(IServiceProvider serviceProvider) => _serviceProvider = serviceProvider;

    public Task<Result<TCommandResult>> Dispatch<TCommand, TCommandResult>(TCommand command, CancellationToken cancellation = default)
        where TCommand : ICommand<TCommandResult>
    {
        var handler = _serviceProvider.GetRequiredService<ICommandHandler<TCommand, TCommandResult>>();
        return handler.Handle(command, cancellation);
    }
}
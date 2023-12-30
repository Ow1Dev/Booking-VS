using Core.ResultTypes;

namespace Core.CQRS;

public interface ICommand<TResponse>
{
}

public interface ICommandHandler<in TCommand, TResponse>
    where TCommand : ICommand<TResponse>
{
    Task<Result<TResponse>> Handle(TCommand command, CancellationToken cancellation);
}
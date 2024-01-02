using Core.CQRS;
using Core.ResultTypes;
using Flights.Stations.Entities;

#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously

namespace Flights.Stations.Features.CreateStation.V1;

public sealed record CreateStationCommand(string Name): ICommand<CreateStationResult>;

public record CreateStationResult(Guid Id);

internal class CreateStationHandler : ICommandHandler<CreateStationCommand, CreateStationResult>
{
    public async Task<Result<CreateStationResult>> Handle(CreateStationCommand command, CancellationToken cancellation)
    {
        return Station
            .Create(command.Name)
            .Map(x => new CreateStationResult(x.Id.Value));
    }
}
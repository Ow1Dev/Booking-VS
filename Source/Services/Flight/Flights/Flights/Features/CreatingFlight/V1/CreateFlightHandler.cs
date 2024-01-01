using Core.CQRS;
using Core.ResultTypes;

namespace Flights.Api.Flights.Features.CreatingFlight.V1;

public record CreateFlightCommand(string FlightNumber): ICommand<CreateFlightResult>;
public record CreateFlightResult(Guid Id);

internal class CreateFlightHandler : ICommandHandler<CreateFlightCommand,CreateFlightResult>
{
    public async Task<Result<CreateFlightResult>> Handle(CreateFlightCommand command, CancellationToken cancellation)
    {
        return new CreateFlightResult(Guid.NewGuid());
    }
}
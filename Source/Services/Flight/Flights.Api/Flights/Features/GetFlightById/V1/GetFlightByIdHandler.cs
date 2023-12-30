using Core.CQRS;
using Core.ResultTypes;
using Flights.Api.Flights.Errors;

namespace Flights.Api.Flights.Features.GetFlightById.V1;

public record GetFlightByIdQuery(Guid FlightId): IQuery<GetFlightByIdResult>;

public record GetFlightByIdResult(string FlightNumber);

public class GetFlightByIdHandler : IQueryHandler<GetFlightByIdQuery, GetFlightByIdResult>
{
    private const string FlightId = "01bb20ef-4b4d-472a-9567-216074f5316b";

    public async Task<Result<GetFlightByIdResult>> Handle(GetFlightByIdQuery query, CancellationToken cancellation)
    {
        return Result.Create(query)
            .Ensure(
                id => id.FlightId == Guid.Parse(FlightId),
                FlightError.NotFound(query.FlightId))
            .Map(r => new GetFlightByIdResult("ABC123"));
    }
}
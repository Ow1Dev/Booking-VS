using Carter;
using Core.CQRS;
using Core.ResultTypes;
using Flights.Api.Flights.Features.CreatingFlight.V1;
using Flights.Api.Flights.Features.GetFlightById.V1;
using Flights.Flights.Errors;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace Flights.Flights;

public record CreateFlightRequest(string FlightNumber);

public class FlightsEndpoints(ICommandDispatcher commandDispatcher, IQueryDispatcher queryDispatcher)
    : CarterModule("")
{
    public override void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/",
            (CreateFlightRequest req, CancellationToken ct, [FromServices] IErrorHandlerFactory errorFac) =>
            {
                return Result.Create(req)
                    .Map(createFlightRequest => new CreateFlightCommand(createFlightRequest.FlightNumber))
                    .Bind(command => commandDispatcher.Dispatch<CreateFlightCommand, CreateFlightResult>(command, ct))
                    .Match(Results.Ok, errorFac.HandleFailure);
            });

        app.MapGet("/{id:guid}",
            ([FromRoute] Guid id, CancellationToken ct, [FromServices] IErrorHandlerFactory errorFac) =>
            {
                return Result.Create(new GetFlightByIdQuery(id))
                    .Bind(command => queryDispatcher.Dispatch<GetFlightByIdQuery, GetFlightByIdResult>(command, ct))
                    .Match(Results.Ok, errorFac.HandleFailure);
            });
    }
}
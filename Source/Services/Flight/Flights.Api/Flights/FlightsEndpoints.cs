using Carter;
using Core.CQRS;
using Core.Endpoints;
using Core.ResultTypes;
using Flights.Api.Contacts.Flights;
using Flights.Api.Flights.Features.CreatingFlight.V1;
using Flights.Api.Flights.Features.GetFlightById.V1;
using Flights.Api.Routes;
using Microsoft.AspNetCore.Mvc;

namespace Flights.Api.Flights;

public class FlightsEndpoints(ICommandDispatcher commandDispatcher, IQueryDispatcher queryDispatcher)
    : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost(FlightsRoutes.CreateFlight,
            (CreateFlightRequest req, CancellationToken ct, [FromServices] IErrorHandlerFactory errorFac) =>
            {
                return Result.Create(req)
                    .Map(createFlightRequest => new CreateFlightCommand(createFlightRequest.FlightNumber))
                    .Bind(command => commandDispatcher.Dispatch<CreateFlightCommand, CreateFlightResult>(command, ct))
                    .Match(Results.Ok, errorFac.HandleFailure);
            });

        app.MapGet(FlightsRoutes.GetFlightByIdQuery,
            ([FromRoute] Guid id, CancellationToken ct, [FromServices] IErrorHandlerFactory errorFac) =>
            {
                return Result.Create(new GetFlightByIdQuery(id))
                    .Bind(command => queryDispatcher.Dispatch<GetFlightByIdQuery, GetFlightByIdResult>(command, ct))
                    .Match(Results.Ok, errorFac.HandleFailure);
            });
    }
}
using Carter;
using Core.CQRS;
using Core.Endpoints;
using Core.ResultTypes;
using Flights.Api.Contacts.Stations;
using Flights.Api.Routes;
using Flights.Stations.Features.CreateStation.V1;
using Microsoft.AspNetCore.Mvc;

namespace Flights.Api.Stations;

public class StationsEndpoints(ICommandDispatcher commandDispatcher) : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost(StationRoutes.CreateStation,
            (CreateStationRequest req, CancellationToken ct, [FromServices] IErrorHandlerFactory errorFac) =>
            {
                return Result.Create(req)
                    .Map(request => new CreateStationCommand(request.Name))
                    .Bind(command => commandDispatcher.Dispatch<CreateStationCommand, CreateStationResult>(command, ct))
                    .Match(Results.Ok, errorFac.HandleFailure);
            });
    }
}
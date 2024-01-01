using Carter;
using Core.CQRS;
using Core.ResultTypes;
using Flights.Api.Flights.Features.CreatingFlight.V1;
using Flights.Api.Flights.Features.GetFlightById.V1;
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
        app.MapPost("/", (CreateFlightRequest req, CancellationToken ct) =>
        {
            return Result.Create(req)
                .Map(createFlightRequest => new CreateFlightCommand(createFlightRequest.FlightNumber))
                .Bind(command => commandDispatcher.Dispatch<CreateFlightCommand, CreateFlightResult>(command, ct))
                .Match(Results.Ok, HandleFailure);
        });

        app.MapGet("/{id:guid}", (Guid id, CancellationToken ct) =>
        {
            return Result.Create(new GetFlightByIdQuery(id))
                .Bind(command => queryDispatcher.Dispatch<GetFlightByIdQuery, GetFlightByIdResult>(command, ct))
                .Match(Results.Ok,
                    e => { return e.Any(error => error.Code == "Flight.NotFound") ? Results.NotFound() : HandleFailure(e); });
        });
    }

    private static IResult HandleFailure(Error[] errors)
    {
        return Results.BadRequest(
            CreateProblemDetails(
                "Bad Request",
                StatusCodes.Status400BadRequest,
                errors));
    }

    private static ProblemDetails CreateProblemDetails(
        string title,
        int status,
        Error[]? errors = null)
    {
        return new ProblemDetails
        {
            Title = title,
            Type = errors[0].Code,
            Detail = errors[1].Message,
            Status = status,
            Extensions = { { nameof(errors), errors } }
        };
    }
}
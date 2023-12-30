using Carter;
using Core.CQRS;
using Core.ResultTypes;
using Flights.Api.Flights.Features.CreatingFlight.V1;
using Flights.Api.Flights.Features.GetFlightById.V1;
using Microsoft.AspNetCore.Mvc;

namespace Flights.Api.Flights;

public record CreateFlightRequest(string FlightNumber);

public class FlightsEndpoints : CarterModule
{
    private readonly ICommandDispatcher _commandDispatcher;
    private readonly IQueryDispatcher _queryDispatcher;

    public FlightsEndpoints(ICommandDispatcher commandDispatcher, IQueryDispatcher queryDispatcher)
        : base("")
    {
        _commandDispatcher = commandDispatcher;
        _queryDispatcher = queryDispatcher;
    }

    public override void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/", (CreateFlightRequest req, CancellationToken ct) =>
        {
            return Result.Create(req)
                .Map(req => new CreateFlightCommand(req.FlightNumber))
                .Bind(command => _commandDispatcher.Dispatch<CreateFlightCommand, CreateFlightResult>(command, ct))
                .Match(Results.Ok, HandleFailure);
        });

        app.MapGet("/{Id}", (Guid Id, CancellationToken ct) =>
        {
            return Result.Create(new GetFlightByIdQuery(Id))
                .Bind(command => _queryDispatcher.Dispatch<GetFlightByIdQuery, GetFlightByIdResult>(command, ct))
                .Match(Results.Ok,
                    e => { return e.Any(e => e.Code == "Flight.NotFound") ? Results.NotFound() : HandleFailure(e); });
        });
    }

    public static IResult HandleFailure(Error[] errors)
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
        return new()
        {
            Title = title,
            Type = errors[0].Code,
            Detail = errors[1].Message,
            Status = status,
            Extensions = { { nameof(errors), errors } }
        };
    }
}
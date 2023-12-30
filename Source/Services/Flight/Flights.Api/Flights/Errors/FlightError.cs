using Core.ResultTypes;

namespace Flights.Api.Flights.Errors;

public static class FlightError
{
    public static Error NotFound(Guid id) => new(
        "Flight.NotFound", $"The flight with Id '{id}' was not found");
}
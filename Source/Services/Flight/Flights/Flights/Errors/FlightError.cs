using Core.Endpoints;
using Core.ResultTypes;
using Microsoft.AspNetCore.Http;

namespace Flights.Flights.Errors;

public static class FlightError
{
    public static Error NotFound(Guid id) => new(
        "Flight.NotFound", $"The flight with Id '{id}' was not found");
}

public class FlightErrorFactory(IErrorHandlerFactory next)
    : IErrorHandlerFactory
{
    public IResult HandleFailure(Error[] errors)
    {
        return errors.Any(error => error.Code == "Flight.NotFound") ? Results.NotFound() : next.HandleFailure(errors);
    }
}
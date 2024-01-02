namespace Flights.Api.Routes;

public static class FlightsRoutes
{
    private const string BaseEndpoint = "/";
    
    public const string CreateFlight = BaseEndpoint;
    public const string GetFlightByIdQuery = BaseEndpoint + "{id:guid}";
}
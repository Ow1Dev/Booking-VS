using Core.ResultTypes;

namespace Flights.Stations.Entities;

public record StationId(Guid Value);

public class Station
{
    private Station(StationId id, string name)
    {
        Name = name;
        Id = id;
    }

    public StationId Id { get; set; }

    public string Name { get; set; }


    public static Result<Station> Create(string name)
    {
        return Result.Success(new Station(new StationId(Guid.NewGuid()), name));
    }
}
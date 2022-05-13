#region

using NetTopologySuite.Geometries;

#endregion

namespace Foodbank_Project.Models;

public class Location
{
    // ReSharper disable once UnusedMember.Global
    public int LocationId { get; set; }

    // ReSharper disable once PropertyCanBeMadeInitOnly.Global
    public string? Name { get; set; }

    // ReSharper disable once PropertyCanBeMadeInitOnly.Global
    public string? Slug { get; set; }

    // ReSharper disable once PropertyCanBeMadeInitOnly.Global
    public string? Address { get; set; }

    // ReSharper disable once PropertyCanBeMadeInitOnly.Global
    public string? Postcode { get; set; }

    // ReSharper disable once PropertyCanBeMadeInitOnly.Global
    public Point? Coord { get; set; }

    // ReSharper disable once PropertyCanBeMadeInitOnly.Global
    public string? Phone { get; set; }

    // ReSharper disable once PropertyCanBeMadeInitOnly.Global
    public Foodbank? Foodbank { get; set; }

    public override bool Equals(object? obj)
    {
        return obj is Location location && location.LocationId == LocationId;
    }
}
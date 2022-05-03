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
    public string? LatLng { get; set; }

    // ReSharper disable once PropertyCanBeMadeInitOnly.Global
    public string? Phone { get; set; }

    // ReSharper disable once UnusedAutoPropertyAccessor.Global
    public int FoodbankId { get; set; }

    // ReSharper disable once PropertyCanBeMadeInitOnly.Global
    public Foodbank? Foodbank { get; set; }
}
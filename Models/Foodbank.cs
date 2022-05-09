namespace Foodbank_Project.Models;

public class Foodbank
{
    public ICollection<Location> Locations;

    // ReSharper disable once UnusedMember.Global
    public int? FoodbankId { get; set; }

    // ReSharper disable once PropertyCanBeMadeInitOnly.Global
    public string? Name { get; set; }

    // ReSharper disable once UnusedAutoPropertyAccessor.Global
    public string? AltName { get; set; }

    // ReSharper disable once UnusedMember.Global
    public string? Notes { get; set; }

    // ReSharper disable once AutoPropertyCanBeMadeGetOnly.Global
    public bool Protected { get; set; } = false;

    // ReSharper disable once PropertyCanBeMadeInitOnly.Global
    public string? Slug { get; set; }

    // ReSharper disable once PropertyCanBeMadeInitOnly.Global
    public string? Phone { get; set; }

    // ReSharper disable once UnusedAutoPropertyAccessor.Global
    public string? SecondaryPhone { get; set; }

    // ReSharper disable once PropertyCanBeMadeInitOnly.Global
    public string? Email { get; set; }

    // ReSharper disable once PropertyCanBeMadeInitOnly.Global
    public string? Address { get; set; }

    // ReSharper disable once PropertyCanBeMadeInitOnly.Global
    public string? Postcode { get; set; }

    // ReSharper disable once PropertyCanBeMadeInitOnly.Global
    public bool? Closed { get; set; }

    // ReSharper disable once PropertyCanBeMadeInitOnly.Global
    public string? Country { get; set; }

    // ReSharper disable once PropertyCanBeMadeInitOnly.Global
    public string? Lat { get; set; }
    public string? Lng { get; set; }

    // ReSharper disable once PropertyCanBeMadeInitOnly.Global
    public string? Network { get; set; }

    // ReSharper disable once PropertyCanBeMadeInitOnly.Global
    public DateTime? Created { get; set; }

    public string? Homepage { get; set; }

    public string? ShoppingList { get; set; }

    public string? CharityNumber { get; set; }

    public string? CharityRegisterUrl { get; set; }

    // ReSharper disable once UnusedAutoPropertyAccessor.Global
    public Provider Provider { get; set; }
    public ICollection<Need> Needs { get; set; }

    public override bool Equals(object? obj)
    {
        return obj is Foodbank foodbank && foodbank.FoodbankId == FoodbankId;
    }
}
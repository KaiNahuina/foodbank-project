#region

using System.ComponentModel.DataAnnotations;
using NetTopologySuite.Geometries;

#endregion

namespace Foodbank_Project.Models;

public class Foodbank
{
    public ICollection<Location>? Locations { get; set; }

    // ReSharper disable once UnusedMember.Global
    public int? FoodbankId { get; set; }

    // ReSharper disable once PropertyCanBeMadeInitOnly.Global
    [Required(ErrorMessage = "Please enter a Name")]
    public string? Name { get; set; }

    // ReSharper disable once UnusedAutoPropertyAccessor.Global
    public string? AltName { get; init; }

    // ReSharper disable once UnusedMember.Global
    public string? Notes { get; init; }

    // ReSharper disable once AutoPropertyCanBeMadeGetOnly.Global
    public bool Protected { get; set; }

    // ReSharper disable once PropertyCanBeMadeInitOnly.Global
    public string? Slug { get; set; }

    // ReSharper disable once PropertyCanBeMadeInitOnly.Global
    [Required(ErrorMessage = "Please enter a Phone Number")]
    public string? Phone { get; set; }

    // ReSharper disable once UnusedAutoPropertyAccessor.Global
    public string? SecondaryPhone { get; set; }

    // ReSharper disable once PropertyCanBeMadeInitOnly.Global
    [Required(ErrorMessage = "Please enter an Email")]
    public string? Email { get; set; }

    // ReSharper disable once PropertyCanBeMadeInitOnly.Global
    [Required(ErrorMessage = "Please enter a postcode into the map")]
    public string? Address { get; set; }

    // ReSharper disable once PropertyCanBeMadeInitOnly.Global
    public string? Postcode { get; set; }

    // ReSharper disable once PropertyCanBeMadeInitOnly.Global
    public bool Closed { get; set; }

    // ReSharper disable once PropertyCanBeMadeInitOnly.Global
    [Required(ErrorMessage = "Please select a Country")]
    public string? Country { get; set; }

    public Status Status { get; set; }

    // ReSharper disable once PropertyCanBeMadeInitOnly.Global
    public Point? Coord { get; set; }

    // ReSharper disable once PropertyCanBeMadeInitOnly.Global
    [Required(ErrorMessage = "Please select a Network")]

    public string? Network { get; set; }

    // ReSharper disable once PropertyCanBeMadeInitOnly.Global
    public DateTime? Created { get; set; }

    [Required(ErrorMessage = "Please enter a Homepage")]
    public string? Homepage { get; init; }

    // ReSharper disable once UnusedAutoPropertyAccessor.Global
    public string? ShoppingList { get; set; }

    public string? CharityNumber { get; init; }

    public string? CharityRegisterUrl { get; init; }

    // ReSharper disable once UnusedAutoPropertyAccessor.Global
    public Provider Provider { get; set; }
    public ICollection<Need>? Needs { get; set; }
}
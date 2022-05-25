#region

using Foodbank_Project.Data;
using Foodbank_Project.Util;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using NetTopologySuite.Geometries;
using Location = Foodbank_Project.Models.Location;

#endregion

namespace Foodbank_Project.Pages.Admin;

[Authorize(Roles = "FoodbanksAdmin,FoodbankAdmin,SiteAdmin")]
public class LocationModel : PageModel
{
    private readonly ApplicationContext _ctx;
    private readonly ILogger<LocationModel> _logger;

    public string? Action;

    public LocationModel(ApplicationContext ctx, ILogger<LocationModel> logger)
    {
        _ctx = ctx;
        _logger = logger;
    }

    [BindProperty] public Location? Location { get; set; }
    [BindProperty] public double Lat { get; set; }
    [BindProperty] public double Lng { get; set; }

    public async Task<IActionResult> OnGetAsync([FromQuery(Name = "Action")] string? action)
    {
        Action = action ?? "Update";
        if (Action != "Create")
        {
            var id = int.Parse(RouteData.Values["id"] as string ?? "");

            if (!User.IsInRole("FoodbanksAdmin") && !User.IsInRole("SiteAdmin"))
                if (!User.IsInRole("FoodbankAdmin") && !User.HasClaim("FoodbankClaim", id.ToString()))
                    return Forbid();

            var locations = from f in _ctx.Locations where f.LocationId == id select f;


            Location = await locations.AsNoTracking().Include(l => l.Foodbank).FirstAsync();
            Lat = Location.Coord!.Y;
            Lng = Location.Coord.X;
            return Page();
        }

        if (!User.IsInRole("FoodbanksAdmin") && !User.IsInRole("SiteAdmin"))
            if (!User.IsInRole("FoodbankAdmin") && !User.HasClaim("FoodbankClaim", Request.Query["target"]))
                return Forbid();

        Location = new Location
        {
            Foodbank = new Models.Foodbank
            {
                FoodbankId = int.Parse(Request.Query["target"])
            }
        };
        return Page();
    }


    public async Task<IActionResult> OnPostAsync([FromQuery(Name = "Action")] string? action)
    {
        Action = action ?? Request.Form["Action"].ToString() ?? "Update";
        switch (Action)
        {
            case "Delete":
                if (!User.IsInRole("FoodbanksAdmin") && !User.IsInRole("SiteAdmin"))
                    if (!User.IsInRole("FoodbankAdmin") &&
                        !User.HasClaim("FoodbankClaim", Location?.Foodbank?.FoodbankId.ToString()))
                        return Forbid();


                if (Location != null) _ctx.Remove(Location);

                _logger.Log(LogLevel.Warning, "User {UserName} deleted location {Name} for foodbank {Foodbank}",
                    User.Identity?.Name, Location?.Name, Location?.Foodbank?.Name);

                break;
            case "Create":
            {
                if (!User.IsInRole("FoodbanksAdmin") && !User.IsInRole("SiteAdmin"))
                    if (!User.IsInRole("FoodbankAdmin") &&
                        !User.HasClaim("FoodbankClaim", Location?.Foodbank?.FoodbankId.ToString()))
                        return Forbid();
                
                if (Location != null)
                {
                    Location.Coord = new Point(Lng, Lat) { SRID = 4326 };
                    Location = FoodbankHelpers.ApplySlug(Location);
                }
                
                ModelState.ClearValidationState(nameof(Location));
                TryValidateModel(Location, nameof(Location));
                foreach (var entry in ModelState.Where(entry => entry.Key.Contains("Location.Foodbank")))
                {
                    ModelState.Remove(entry.Key);
                }
                if (!ModelState.IsValid) return Page();
                
                _ctx.Foodbanks!.Attach(Location.Foodbank);
                _ctx.Locations?.Update(Location);

                _logger.Log(LogLevel.Information, "User {UserName} created location {Name} for foodbank {Foodbank}",
                    User.Identity?.Name, Location?.Name, Location?.Foodbank?.Name);

                break;
            }
            case "Update":
            {
                if (!User.IsInRole("FoodbanksAdmin") && !User.IsInRole("SiteAdmin"))
                    if (!User.IsInRole("FoodbankAdmin") &&
                        !User.HasClaim("FoodbankClaim", Location?.Foodbank?.FoodbankId.ToString()))
                        return Forbid();

                if (!ModelState.IsValid) return Page();
                if (Location != null)
                {
                    _ctx.Foodbanks!.Attach(Location.Foodbank);
                    Location.Coord = new Point(Lng, Lat) { SRID = 4326 };
                    _ctx.Locations?.Update(Location);
                }

                _logger.Log(LogLevel.Information, "User {UserName} updated location {Name} for foodbank {Foodbank}",
                    User.Identity?.Name, Location?.Name, Location?.Foodbank?.Name);


                break;
            }
        }

        await _ctx.SaveChangesAsync();
        return RedirectToPage("./Foodbank", routeValues: new { id = Location?.Foodbank?.FoodbankId ?? 0 },
            fragment: "locations", pageHandler: "");
    }
}
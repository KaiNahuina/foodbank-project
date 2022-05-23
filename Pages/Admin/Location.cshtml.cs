#region

using Foodbank_Project.Data;
using Foodbank_Project.Models;
using Foodbank_Project.Util;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using NetTopologySuite.Geometries;

#endregion

namespace Foodbank_Project.Pages.Admin;

public class LocationModel : PageModel
{
    private readonly ApplicationContext _ctx;

    public string? Action;

    public LocationModel(ApplicationContext ctx)
    {
        _ctx = ctx;
    }

    [BindProperty] public Models.Location? Location { get; set; }
    [BindProperty] public double Lat { get; set; }
    [BindProperty] public double Lng { get; set; }

    public async Task OnGetAsync([FromQuery(Name = "Action")] string? action)
    {
        Action = action ?? "Update";
        if (Action != "Create")
        {
            var id = int.Parse(RouteData.Values["id"] as string ?? "");
            var locations = from f in _ctx.Locations where f.LocationId == id select f;


            Location = await locations.AsNoTracking().Include(l => l.Foodbank).FirstAsync();
            Lat = Location.Coord!.Y;
            Lng = Location.Coord.X;
        }
        else
        {
            Location = new Models.Location
            {
                Foodbank = new Models.Foodbank
                {
                    FoodbankId = int.Parse(Request.Query["target"])
                }
            };
        }
    }


    public async Task<IActionResult> OnPostAsync([FromQuery(Name = "Action")] string? action)
    {
        Action = action ?? Request.Form["Action"].ToString() ?? "Update";
        switch (Action)
        {
            case "Delete":
                if (Location != null) _ctx.Remove(Location);

                break;
            case "Create":
            {
                if (!ModelState.IsValid) return Page();
                if (Location != null)
                {
                    Location.Coord = new Point(Lng, Lat) { SRID = 4326 };
                    Location = FoodbankHelpers.ApplySlug(Location);
                    _ctx.Locations?.Update(Location);
                }

                break;
            }
            case "Update":
            {
                if (!ModelState.IsValid) return Page();
                if (Location != null)
                {
                    Location.Coord = new Point(Lng, Lat) { SRID = 4326 };
                    _ctx.Locations?.Update(Location);
                }

                break;
            }

        }

        await _ctx.SaveChangesAsync();
        return RedirectToPage("./Foodbank", routeValues:new  {id=Location?.Foodbank?.FoodbankId ?? 0}, fragment:"locations", pageHandler:"");
    }
}
#region

using Foodbank_Project.Data;
using Foodbank_Project.Models;
using Foodbank_Project.Util;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using NetTopologySuite.Geometries;
using Foodbank = Foodbank_Project.Models.Foodbank;

#endregion

namespace Foodbank_Project.Pages.GiveHelp;

public class FindDistribution : PageModel
{
    private readonly ApplicationContext _ctx;

    public string? Location { get; set; }

    public FindDistribution(ApplicationContext ctx)
    {
        _ctx = ctx;
    }
    public void OnGetAsync([FromQuery(Name = "Location")] string location)
    {
        Location = location;
    }

    public async Task<JsonResult> OnPostAsync([FromBody] Coords obj)
    {
        var origin = new Point(obj.Lng, obj.Lat) { SRID = 4326 };

        var foodBankLocations = await _ctx
            .Foodbanks!.AsNoTracking().Where(f => f.Status == Status.Approved)
            .Select(f => new
            {
                Distance = (int)Math.Round(f.Coord!.ProjectTo(27700).Distance(origin.ProjectTo(27700))),
                f.Name,
                Id = f.FoodbankId,
                f.Address,
                f.Postcode,
                Coord = new Coords
                {
                    Lat = f.Coord!.Y,
                    Lng = f.Coord!.X
                }
            }).ToArrayAsync();

        var top5Locations = foodBankLocations.OrderBy(f => f.Distance).Take(5).ToList();

        return new JsonResult(top5Locations);
    }

    public class Coords
    {
        public double Lat { get; set; }
        public double Lng { get; set; }
    }
}
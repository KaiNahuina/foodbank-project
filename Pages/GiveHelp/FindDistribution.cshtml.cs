using Foodbank_Project.Data;
using Foodbank_Project.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using NetTopologySuite.Geometries;
using System.Collections.ObjectModel;
using Location = Foodbank_Project.Models.Location;
namespace Foodbank_Project.Pages.GiveHelp
{
    public class FindDistribution : PageModel
    {
        private readonly ApplicationContext _ctx;

        public FindDistribution(ApplicationContext ctx)
        {
            _ctx = ctx;
        }

        public string? Location { get; set; }

        public ICollection<Location> Locations { get; set; }

        public async Task OnGetAsync([FromQuery(Name = "Location")] string location)
        {
            Location = location;
        }

        public async Task<JsonResult> OnPostAsync([FromBody] Coords obj)
        {
            var origin = new Point(obj.Lng, obj.Lat) { SRID = 4326 };

            var foodBankLocations = await _ctx
                .Locations.AsNoTracking().Include(l => l.Foodbank).Where(l => l.Foodbank.Status == Status.Approved)
                .Select(l => new
                {
                    Distance = (int)Math.Round(l.Coord.Distance(origin)),
                    l.Name,
                    Id = l.LocationId,
                    l.Address,
                    Coord = new Coords
                    {
                        Lat = l.Coord.Y,
                        Lng = l.Coord.X
                    }
                }).ToArrayAsync();

            var top5Locations = foodBankLocations.OrderBy(l => l.Distance).Take(5).ToList();

            return new JsonResult(top5Locations);
        }
        public class Coords
        {
            public double Lat { get; set; }
            public double Lng { get; set; }
        }
    }
}

using Foodbank_Project.Data;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NetTopologySuite.Geometries;
using Location = Foodbank_Project.Models.Location;

namespace Foodbank_Project.Pages
{
    public class GetHelpLandingModel : PageModel
    {
        private readonly ApplicationContext _ctx;

        public string? Location { get; set; }

        public ICollection<Location> Locations { get; set; }

        public GetHelpLandingModel(ApplicationContext ctx)
        {
            _ctx = ctx;
        }

        public async Task OnGetAsync()
        {
            Location = RouteData.Values?["Location"]?.ToString() ?? null;


            var locations = from l in _ctx.Locations select l;

            Locations = await locations.AsNoTracking().ToListAsync();
        }

        // DTO
        public class Coords
        {
            public double Lat { get; set; }
            public double Lng { get; set; }
        }

        public async Task<JsonResult> OnPostCoordAsync([FromBody] Coords obj)
        {
            var origin = new Point(obj.Lng, obj.Lat) { SRID = 4326 };

            var foodBankLocations = await _ctx
                .Locations.AsNoTracking()
                .Select(l => new
                {
                    Distance = l.Coord.Distance(origin),
                    l.Name,
                    Id = l.LocationId,
                    l.Address,
                    Coord = new Coords
                    {
                        Lat = l.Coord.Y,
                        Lng = l.Coord.X
                    }
                }).ToListAsync();

            var top5Locations = foodBankLocations.OrderBy(l => l.Distance).Take(5).ToList();

            return new JsonResult(top5Locations);
        }
    }
}

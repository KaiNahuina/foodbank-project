using Foodbank_Project.Data;
using Foodbank_Project.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace Foodbank_Project.Pages
{
    public class testMapModel : PageModel
    {
        private readonly ApplicationContext _ctx;

        public string? Location { get; set; }

        public ICollection<Location> Locations { get; set; }


        public testMapModel(ApplicationContext ctx)
        {
            _ctx = ctx;
        }

        public async Task OnGetAsync()
        {
            Location = RouteData.Values?["Location"]?.ToString() ?? null;


            IQueryable<Location> locations = from l in _ctx.Locations select l;

            Locations = await locations.AsNoTracking().ToListAsync();
        }

        // DTO
        public class Coords { public double Lat { get; set; } public double Lng { get; set; } }
        public async Task<JsonResult> OnPostCoordAsync([FromBody] Coords obj)
        {
            var origin = new NetTopologySuite.Geometries.Point(obj.Lng, obj.Lat) { SRID = 4326 };
            //System.Diagnostics.Debug.WriteLine("This is the origin point" + origin);

            var foodBankLocations = await _ctx
                .Locations.AsNoTracking()
                .Select(l => new
                {
                    Distance = l.Coord.Distance(origin),
                    Name = l.Name,
                    Id = l.LocationId,
                    Address = l.Address,
                    Coord = new Coords
                    {
                        Lat = l.Coord.Y,
                        Lng = l.Coord.X
                    }
                }).ToListAsync();

            var top5Locations = foodBankLocations.OrderBy(l => l.Distance).Take(5).ToList();

            Debug.WriteLine("-----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------");

            Debug.WriteLine("This is the count for top5Locations size" + top5Locations.Count);
            for (int i = 0; i < top5Locations.Count; i++)
            {
                System.Diagnostics.Debug.WriteLine(top5Locations[i]);
            }

            //return Json(new { list = top5Locations }, JsonRequestBehavior.AllowGet);
            return new JsonResult(top5Locations);
            //return (IActionResult)top5Locations;
            //return new Json(new { list = top5Locations }, JsonRequestBehavior.AllowGet);
        }

    }
}

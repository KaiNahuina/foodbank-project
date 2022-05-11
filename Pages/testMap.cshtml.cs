using Foodbank_Project.Data;
using Foodbank_Project.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

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

        public class Coords { public double Lat { get; set; } public double Lng { get; set; } }
        public IActionResult OnPostCoord([FromBody] Coords obj)
        {
            var origin = Point(obj.Lat, obj.Lng);
            return new JsonResult("Hi there!");
        }
    }
}

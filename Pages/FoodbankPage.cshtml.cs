#region

using Foodbank_Project.Data;
using Foodbank_Project.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using NetTopologySuite.Geometries;
using Location = Foodbank_Project.Models.Location;
#endregion

namespace Foodbank_Project.Pages;

public class FoodbankPageModel : PageModel
{
    private readonly ApplicationContext _ctx;

    public Location Location { get; set; }

    public FoodbankPageModel(ApplicationContext ctx)
    {
        _ctx = ctx;
    }

    public async Task OnGetAsync([FromRoute(Name = "id")]int location)
    {
        Location = await _ctx
            .Locations.AsNoTracking().Include(l => l.Foodbank).Where(l => l.Foodbank.Status == Status.Approved)
            .Where(l => l.LocationId == location).FirstAsync();

    }

}

#region

using Foodbank_Project.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Location = Foodbank_Project.Models.Location;
#endregion

namespace Foodbank_Project.Pages.Foodbank;

public class DistributionPageModel : PageModel
{
    private readonly ApplicationContext _ctx;

    public Models.Foodbank Foodbank { get; set; }
    public Location Location { get; set; }

    public DistributionPageModel(ApplicationContext ctx)
    {
        _ctx = ctx;
    }

    public async Task OnGetAsync([FromRoute(Name = "id")] int locationid)
    {
        var locale = _ctx.Locations.AsNoTracking().Where(l => l.LocationId == locationid);
        Location = await locale.FirstAsync();

        var foodbank = locale.Include(l => l.Foodbank).ThenInclude(f => f.Locations).Select(l => l.Foodbank);
        Foodbank = await foodbank.FirstAsync();

    }


}
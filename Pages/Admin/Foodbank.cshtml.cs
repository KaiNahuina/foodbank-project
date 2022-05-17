#region

using Foodbank_Project.Data;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

#endregion

namespace Foodbank_Project.Pages.Admin;

public class FoodbankModel : PageModel
{
    private readonly ApplicationContext _ctx;

    public FoodbankModel(ApplicationContext ctx)
    {
        _ctx = ctx;
    }

    public Models.Foodbank Foodbank { get; set; }

    public async Task OnGetAsync()
    {
        var id = int.Parse(RouteData.Values["id"] as string);
        var foodbanks = from f in _ctx.Foodbanks where f.FoodbankId == id select f;

        Foodbank = await foodbanks.AsNoTracking().Include(f => f.Locations).Include(f => f.Needs).FirstAsync();
    }
}
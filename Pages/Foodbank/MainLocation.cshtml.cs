#region

using Foodbank_Project.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

#endregion

namespace Foodbank_Project.Pages.Foodbank;

public class MainLocationModel : PageModel
{
    private readonly ApplicationContext _ctx;

    public MainLocationModel(ApplicationContext ctx)
    {
        _ctx = ctx;
    }

    public Models.Foodbank? Foodbank { get; set; }

    public async Task OnGetAsync([FromRoute(Name = "id")] int foodbankId)
    {
        var foodbank = _ctx.Foodbanks!.AsNoTracking().Where(l => l.FoodbankId == foodbankId).Include(f => f.Locations)
            .Include(f => f.Needs);
        Foodbank = await foodbank.FirstAsync();
    }
}
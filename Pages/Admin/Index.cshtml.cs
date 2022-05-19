#region

using Foodbank_Project.Data;
using Foodbank_Project.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

#endregion

namespace Foodbank_Project.Pages.Admin;

public class IndexModel : PageModel
{
    private readonly ApplicationContext _ctx;

    public IList<Models.Foodbank>? Foodbanks;

    public IndexModel(ApplicationContext ctx)
    {
        _ctx = ctx;
    }

    public async Task OnGetAsync()
    {
        var foodbankQue = from f in _ctx.Foodbanks
            where f.Status == Status.UnConfirmed
            select f;


        Foodbanks = await foodbankQue.AsNoTracking().ToListAsync();
    }
}
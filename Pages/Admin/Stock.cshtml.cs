using Foodbank_Project.Data;
using Foodbank_Project.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace Foodbank_Project.Pages.Admin;

public class Stock : PageModel
{
    private readonly ApplicationContext _ctx;
        
    public IList<Need> Needs;

    public Stock(ApplicationContext ctx)
    {
        _ctx = ctx;
    }
        
    public async Task OnGetAsync()
    {
        IQueryable<Need> needQue = from f in _ctx.Needs
            select f;

        // do sort and filter shizzle here

        Needs = await needQue.AsNoTracking().Where(n => n.NeedStr != null).Include(n => n.Foodbanks).OrderByDescending(n => n.Foodbanks.Count).Take(100).ToListAsync();

    }
}
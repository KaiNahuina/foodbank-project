#region

using Foodbank_Project.Data;
using Foodbank_Project.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

#endregion

namespace Foodbank_Project.Pages.Admin;

public class FoodbankModel : PageModel
{
    private readonly ApplicationContext _ctx;

    public string Action;
    public FoodbankModel(ApplicationContext ctx)
    {
        _ctx = ctx;
    }

    [BindProperty]
    public Models.Foodbank Foodbank { get; set; }

    public async Task OnGetAsync([FromQuery(Name = "Action")]string action)
    {
        if (action != "Create")
        {
            var id = int.Parse(RouteData.Values["id"] as string);
            var foodbanks = from f in _ctx.Foodbanks where f.FoodbankId == id select f;


            Foodbank = await foodbanks.AsNoTracking().Include(f => f.Locations).Include(f => f.Needs).FirstAsync();
        }
        else
        {
            Action = "Create";
            Foodbank = new Models.Foodbank();
        }

    }


    public async Task<RedirectToPageResult> OnPostAsync([FromQuery(Name = "Action")] string action)
    {
        if (Action != "Delete")
        {
            _ctx.Remove(Foodbank);
            await _ctx.SaveChangesAsync();
        }
        return RedirectToPage("./Foodbanks");
    }
}
#region

using Foodbank_Project.Data;
using Foodbank_Project.Models;
using Foodbank_Project.Util;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using NetTopologySuite.Geometries;

#endregion

namespace Foodbank_Project.Pages.Admin;

public class FoodbankModel : PageModel
{
    private readonly ApplicationContext _ctx;

    public string? Action;

    public FoodbankModel(ApplicationContext ctx)
    {
        _ctx = ctx;
    }

    [BindProperty] public Models.Foodbank? Foodbank { get; set; }
    [BindProperty] public double Lat { get; set; }
    [BindProperty] public double Lng { get; set; }

    public async Task OnGetAsync([FromQuery(Name = "Action")] string? action)
    {
        Action = action ?? "Update";
        if (Action != "Create")
        {
            var id = int.Parse(RouteData.Values["id"] as string ?? "");
            var foodbanks = from f in _ctx.Foodbanks where f.FoodbankId == id select f;


            Foodbank = await foodbanks.AsNoTracking().Include(f => f.Locations).Include(f => f.Needs).FirstAsync();
            Lat = Foodbank.Coord!.Y;
            Lng = Foodbank.Coord.X;
        }
        else
        {
            Foodbank = new Models.Foodbank
            {
                Created = DateTime.Now
            };
        }
    }


    public async Task<RedirectToPageResult> OnPostAsync([FromQuery(Name = "Action")] string? action)
    {
        Action = action ?? Request.Form["Action"].ToString() ?? "Update";
        switch (Action)
        {
            case "Delete":
                if (Foodbank != null) _ctx.Remove(Foodbank);

                break;
            case "Create":
            {
                if (Foodbank != null)
                {
                    Foodbank.Coord = new Point(Lng, Lat) { SRID = 4326 };
                    Foodbank.Provider = Provider.Internal;
                    Foodbank = FoodbankHelpers.ApplyFinalize(Foodbank);
                    _ctx.Foodbanks?.Update(Foodbank);
                }

                break;
            }
            case "Update":
            {
                if (Foodbank != null)
                {
                    Foodbank.Coord = new Point(Lng, Lat) { SRID = 4326 };
                    _ctx.Foodbanks?.Update(Foodbank);
                }

                break;
            }
        }

        await _ctx.SaveChangesAsync();
        return RedirectToPage("./Foodbanks");
    }
}
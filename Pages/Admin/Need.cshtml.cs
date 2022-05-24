using Foodbank_Project.Data;
using Foodbank_Project.Models;
using Foodbank_Project.Util;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using NetTopologySuite.Geometries;
using Location = Foodbank_Project.Models.Location;

namespace Foodbank_Project.Pages.Admin;

public class NeedModel : PageModel
{
    private readonly ApplicationContext _ctx;

    public string? Action { get; set; }

    public string? Name { get; set; }
    public int? Target { get; set; }

    public List<Need>? Needs { get; set; }

    public NeedModel(ApplicationContext ctx)
    {
        _ctx = ctx;
    }
    
    public async Task OnGetAsync([FromQuery(Name = "Action")] string? action)
    {
        Action = action ?? "Update";
        switch (Action)
        {
            case "Search":
            {
                Name = Request.Query["Name"];
                Target = int.Parse(Request.Query["Target"]);
                Needs = await _ctx.Needs!.AsNoTracking().Where(n => n.NeedStr!.Contains(Name)).
                    OrderByDescending(n => n.Foodbanks.Count).Take(25).ToListAsync();
                break;
            }
            default:
            {
                Target = int.Parse(Request.Query["Target"]);
                break;
            }
        }
    }
    public async Task<IActionResult> OnPostAsync()
    {
        Action = Request.Form["Action"].ToString() ?? "Update";
        switch (Action)
        {
            case "Remove":
            {
                Target = int.Parse(Request.Form["Target"]);
                var id = int.Parse(Request.RouteValues["id"]?.ToString());
                var foodbank = await _ctx.Foodbanks!.Where(f => f.FoodbankId == Target)
                    .Include(f => f.Needs.Where(n => n.NeedId == id))
                    .FirstAsync();
                
                foodbank.Needs!.Clear();

                await _ctx.SaveChangesAsync();
                
                return RedirectToPage("./Foodbank", routeValues:new  {id=Target}, fragment:"needs", pageHandler:"");
            }
            case "Add":
            {
                Target = int.Parse(Request.Form["Target"]);
                var id = int.Parse(Request.RouteValues["id"]?.ToString());
                var foodbank = await _ctx.Foodbanks!.Where(f => f.FoodbankId == Target).Include(f => f.Needs)
                    .FirstAsync();

                var need = await _ctx.Needs!.Where(n => n.NeedId == id).FirstAsync();
                
                foodbank.Needs!.Add(need);
                
                await _ctx.SaveChangesAsync();
                
                return RedirectToPage("./Foodbank", routeValues:new  {id=Target}, fragment:"needs", pageHandler:"");
            }
            case "Create":
            {
                Target = int.Parse(Request.Form["Target"]);
                var id = int.Parse(Request.RouteValues["id"]?.ToString());
                Name = Request.Form["Name"];
                var foodbank = await _ctx.Foodbanks!.Where(f => f.FoodbankId == Target).Include(f => f.Needs)
                    .FirstAsync();
                
                foodbank.Needs!.Add(new Need
                {
                    NeedStr = Name
                });
                
                await _ctx.SaveChangesAsync();
                
                return RedirectToPage("./Foodbank", routeValues:new  {id=Target}, fragment:"needs", pageHandler:"");
            }
        }

        await _ctx.SaveChangesAsync();
        return Page();
    }
}
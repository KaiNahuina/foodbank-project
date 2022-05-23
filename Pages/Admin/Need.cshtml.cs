using Foodbank_Project.Data;
using Foodbank_Project.Util;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using NetTopologySuite.Geometries;
using Location = Foodbank_Project.Models.Location;

namespace Foodbank_Project.Pages.Admin;

public class AddNeed : PageModel
{
    private readonly ApplicationContext _ctx;
    
    public string? Action;

    public string? Name;
    public AddNeed(ApplicationContext ctx)
    {
        _ctx = ctx;
    }
    
    public void OnGet()
    {
        
    }

    public async Task<IActionResult> OnPostAsync([FromQuery(Name = "Action")] string? action)
    {
        Action = action ?? Request.Form["Action"].ToString() ?? "Update";
        switch (Action)
        {
            case "Remove":
                var target = int.Parse(Request.Form["Target"]);
                var id = int.Parse(Request.RouteValues["id"]?.ToString());
                var foodbank = await _ctx.Foodbanks!.Where(f => f.FoodbankId == target)
                    .Include(f => f.Needs.Where(n => n.NeedId == id))
                    .FirstAsync();
                
                foodbank.Needs!.Clear();

                await _ctx.SaveChangesAsync();
                
                return RedirectToPage("./Foodbank", routeValues:new  {id=target}, fragment:"needs", pageHandler:"");
                break;
            case "Create":
            {
                

                break;
            }
            case "Update":
            {
                

                break;
            }

        }

        await _ctx.SaveChangesAsync();
        return Forbid();
    }
}
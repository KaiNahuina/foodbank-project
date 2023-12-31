﻿#region

using Foodbank_Project.Data;
using Foodbank_Project.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

#endregion

namespace Foodbank_Project.Pages.Admin;

[Authorize(Roles = "FoodbanksAdmin,FoodbankAdmin,SiteAdmin")]
public class NeedModel : PageModel
{
    private readonly ApplicationContext _ctx;
    private readonly ILogger<NeedModel> _logger;

    public NeedModel(ApplicationContext ctx, ILogger<NeedModel> logger)
    {
        _ctx = ctx;
        _logger = logger;
    }

    public string? Action { get; set; }

    public string? Name { get; set; }
    public int? Target { get; set; }

    public List<Need>? Needs { get; set; }

    public async Task<IActionResult> OnGetAsync([FromQuery(Name = "Action")] string? action)
    {
        Action = action ?? "Update";
        switch (Action)
        {
            case "Search":
            {
                Name = Request.Query["Name"];
                Target = int.Parse(Request.Query["Target"]);

                if (!User.IsInRole("FoodbanksAdmin") || !User.IsInRole("SiteAdmin"))
                    if (!User.IsInRole("FoodbankAdmin") && !User.HasClaim("FoodbankClaim", Target.ToString()))
                        return Unauthorized();

                Needs = await _ctx.Needs!.AsNoTracking().Where(n => n.NeedStr!.Contains(Name))
                    .OrderByDescending(n => n.Foodbanks.Count).Take(25).ToListAsync();
                break;
            }
            default:
            {
                Target = int.Parse(Request.Query["Target"]);
                if (!User.IsInRole("FoodbanksAdmin") && !User.IsInRole("SiteAdmin"))
                    if (!User.IsInRole("FoodbankAdmin") && !User.HasClaim("FoodbankClaim", Target.ToString()))
                        return Forbid();

                break;
            }
        }

        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        Action = Request.Form["Action"].ToString() ?? "Update";
        switch (Action)
        {
            case "Remove":
            {
                Target = int.Parse(Request.Form["Target"]);
                if (!User.IsInRole("FoodbanksAdmin") && !User.IsInRole("SiteAdmin"))
                    if (!User.IsInRole("FoodbankAdmin") && !User.HasClaim("FoodbankClaim", Target.ToString()))
                        return Forbid();

                var id = int.Parse(Request.RouteValues["id"]?.ToString());
                var foodbank = await _ctx.Foodbanks!.Where(f => f.FoodbankId == Target)
                    .Include(f => f.Needs.Where(n => n.NeedId == id))
                    .FirstAsync();

                foodbank.Needs!.Clear();

                await _ctx.SaveChangesAsync();

                _logger.Log(LogLevel.Information, "User {UserName} removed need {Need} id from foodbank {Foodbank} id",
                    User.Identity?.Name, id, foodbank.Name);

                return RedirectToPage("./Foodbank", routeValues: new { id = Target }, fragment: "needs",
                    pageHandler: "");
            }
            case "Add":
            {
                Target = int.Parse(Request.Form["Target"]);
                if (!User.IsInRole("FoodbanksAdmin") && !User.IsInRole("SiteAdmin"))
                    if (!User.IsInRole("FoodbankAdmin") && !User.HasClaim("FoodbankClaim", Target.ToString()))
                        return Forbid();

                var id = int.Parse(Request.RouteValues["id"]?.ToString());
                var foodbank = await _ctx.Foodbanks!.Where(f => f.FoodbankId == Target).Include(f => f.Needs)
                    .FirstAsync();

                var need = await _ctx.Needs!.Where(n => n.NeedId == id).FirstAsync();

                foodbank.Needs!.Add(need);

                _logger.Log(LogLevel.Information, "User {UserName} added need {Need} id from foodbank {Foodbank}",
                    User.Identity?.Name, id, foodbank.Name);

                await _ctx.SaveChangesAsync();

                return RedirectToPage("./Foodbank", routeValues: new { id = Target }, fragment: "needs",
                    pageHandler: "");
            }
            case "Create":
            {
                Target = int.Parse(Request.Form["Target"]);
                if (!User.IsInRole("FoodbanksAdmin") && !User.IsInRole("SiteAdmin"))
                    if (User.IsInRole("FoodbankAdmin") && !User.HasClaim("FoodbankClaim", Target.ToString()))
                        return Forbid();

                Name = Request.Form["Name"];
                var foodbank = await _ctx.Foodbanks!.Where(f => f.FoodbankId == Target).Include(f => f.Needs)
                    .FirstAsync();

                foodbank.Needs!.Add(new Need
                {
                    NeedStr = Name
                });

                await _ctx.SaveChangesAsync();

                _logger.Log(LogLevel.Information, "User {UserName} added need {Need} from foodbank {Foodbank}",
                    User.Identity?.Name, Name, foodbank.Name);

                return RedirectToPage("./Foodbank", routeValues: new { id = Target }, fragment: "needs",
                    pageHandler: "");
            }
        }

        await _ctx.SaveChangesAsync();
        return Page();
    }
}
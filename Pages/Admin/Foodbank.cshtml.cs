#region

using System.ComponentModel;
using System.Security.Claims;
using Foodbank_Project.Data;
using Foodbank_Project.Models;
using Foodbank_Project.Util;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using NetTopologySuite.Geometries;
using Location = Foodbank_Project.Models.Location;

#endregion

namespace Foodbank_Project.Pages.Admin;

[Authorize(Roles =
    "FoodbanksAdmin,FoodbankAdmin,SiteAdmin,ApprovalAdmin")]
public class FoodbankModel : PageModel
{
    private readonly ApplicationContext _ctx;
    private readonly ILogger<FoodbankModel> _logger;
    private readonly UserManager<IdentityUser> _userManager;

    public string? Action;

    public bool HasNextPage;
    public bool HasPrevPage;

    public IList<Location>? Locations;
    public int MaxPages;

    public IList<Need>? Needs;
    public string? OrderBy;
    public string? OrderDirection;
    public new int Page;
    public string? Search;
    public int TotalItems;


    public FoodbankModel(ApplicationContext ctx, ILogger<FoodbankModel> logger, UserManager<IdentityUser> userManager)
    {
        _ctx = ctx;
        _logger = logger;
        _userManager = userManager;
    }

    [BindProperty] public Models.Foodbank? Foodbank { get; set; }
    [DisplayName("Latitude")]
    [BindProperty] public double Lat { get; set; }
    [DisplayName("Longitude")]
    [BindProperty] public double Lng { get; set; }

    public async Task<IActionResult> OnGetAsync([FromQuery(Name = "Action")] string? action,
        [FromQuery(Name = "OrderBy")] string? orderBy,
        [FromQuery(Name = "OrderDirection")] string? orderDirection,
        [FromQuery(Name = "Search")] string? search, [FromQuery(Name = "Page")] string? page)
    {
        Action = action ?? "Update";
        if (Action != "Create")
        {
            var id = int.Parse(RouteData.Values["id"] as string ?? "");
            if (!User.IsInRole("FoodbanksAdmin") && !User.IsInRole("SiteAdmin"))
                if (User.IsInRole("FoodbankAdmin") && !User.HasClaim("FoodbankClaim", id.ToString()))
                    return Forbid();

            OrderBy = string.IsNullOrEmpty(orderBy) ? "Name" : orderBy;
            OrderDirection = string.IsNullOrEmpty(orderDirection) ? "Desc" : orderDirection;
            if (!int.TryParse(page, out Page)) Page = 1;
            if (string.IsNullOrEmpty(search))
            {
                if (string.IsNullOrEmpty(Search)) Search = "";
            }
            else
            {
                Search = search;
            }

            var foodbankQue = _ctx.Foodbanks!.Where(f => f.FoodbankId == id);

            var locationQue = _ctx.Locations!.AsNoTracking().Include(l => l.Foodbank)
                .Where(l => l.Foodbank!.FoodbankId == id)
                .OrderByDescending(n => n.Name)
                .Where(n =>
                    string.IsNullOrEmpty(Search) || n.Name!.Contains(Search) || n.Address!.Contains(Search) ||
                    n.Postcode!.Contains(Search)
                    || n.LocationId.ToString() == Search);


            var needQue = _ctx.Needs.AsNoTracking().Include(n => n.Foodbanks.Where(f => f.FoodbankId == id))
                .Where(n => n.Foodbanks.Any(f => f.FoodbankId == id)) // huh
                .OrderByDescending(n => n.NeedStr)
                .Where(n =>
                    string.IsNullOrEmpty(Search) || n.NeedStr!.Contains(Search)
                                                 || n.NeedId.ToString() == Search);

            switch (OrderDirection)
            {
                case "Asc":
                {
                    locationQue = OrderBy switch
                    {
                        "Name" => locationQue.OrderBy(n => n.Name),
                        "Address" => locationQue.OrderBy(n => n.Address),
                        _ => locationQue
                    };

                    needQue = OrderBy switch
                    {
                        "Name" => needQue.OrderBy(n => n.NeedStr),
                        _ => needQue
                    };

                    break;
                }
                case "Desc":
                {
                    locationQue = OrderBy switch
                    {
                        "Name" => locationQue.OrderByDescending(n => n.Name),
                        "Address" => locationQue.OrderByDescending(n => n.Address),
                        _ => locationQue
                    };

                    needQue = OrderBy switch
                    {
                        "Name" => needQue.OrderByDescending(n => n.NeedStr),
                        _ => needQue
                    };

                    break;
                }
            }

            HasPrevPage = Page > 1;

            TotalItems = Math.Max(await locationQue.CountAsync(), await needQue.CountAsync());
            MaxPages = (int)Math.Ceiling(TotalItems / 25d);

            HasNextPage = Page < MaxPages;

            Locations = await locationQue.Skip((Page - 1) * 25).Take(25).ToListAsync();

            Foodbank = await foodbankQue.FirstAsync();

            Needs = await needQue.Skip((Page - 1) * 25).Take(25).ToListAsync();
            Lat = Foodbank.Coord!.Y;
            Lng = Foodbank.Coord.X;
            return Page();
        }

        if (!User.IsInRole("FoodbanksAdmin") && !User.IsInRole("SiteAdmin")) return Forbid();
        Foodbank = new Models.Foodbank
        {
            Created = DateTime.Now
        };
        return Page();
    }


    public async Task<IActionResult> OnPostAsync([FromQuery(Name = "Action")] string? action)
    {
        Action = action ?? Request.Form["Action"].ToString() ?? "Update";
        switch (Action)
        {
            case "Delete":

                if (!User.IsInRole("FoodbanksAdmin") && !User.IsInRole("SiteAdmin"))
                    if (User.IsInRole("FoodbankAdmin") &&
                        !User.HasClaim("FoodbankClaim", Foodbank?.FoodbankId.ToString()))
                        return Forbid();

                if (Foodbank != null) _ctx.Remove(Foodbank);

                _logger.Log(LogLevel.Warning, "User {UserName} deleted foodbank {Name}", User.Identity?.Name,
                    Foodbank?.Name);

                break;
            case "Create":
            {
                if (!User.IsInRole("FoodbanksAdmin") && !User.IsInRole("SiteAdmin")) return Forbid();
                foreach (var entry in ModelState.Where(entry => entry.Key.Contains("Foodbank.Location")))
                    ModelState.Remove(entry.Key);
                if (!ModelState.IsValid) return Page();
                if (Foodbank != null)
                {
                    Foodbank.Coord = new Point(Lng, Lat) { SRID = 4326 };
                    Foodbank.Provider = Provider.Internal;
                    Foodbank = FoodbankHelpers.ApplyFinalize(Foodbank);
                    _ctx.Foodbanks?.Update(Foodbank);
                }

                _logger.Log(LogLevel.Information, "User {UserName} created foodbank {Name}", User.Identity?.Name,
                    Foodbank?.Name);

                break;
            }
            case "Update":
            {
                if (!User.IsInRole("FoodbanksAdmin") && !User.IsInRole("SiteAdmin"))
                    if (User.IsInRole("FoodbankAdmin") &&
                        !User.HasClaim("FoodbankClaim", Foodbank?.FoodbankId.ToString()))
                        return Forbid();

                foreach (var entry in ModelState.Where(entry => entry.Key.Contains("Foodbank.Location")))
                    ModelState.Remove(entry.Key);

                if (!ModelState.IsValid) return Page();
                if (Foodbank != null)
                {
                    Foodbank.Coord = new Point(Lng, Lat) { SRID = 4326 };
                    _ctx.Foodbanks?.Update(Foodbank);
                }

                _logger.Log(LogLevel.Information, "User {UserName} updated foodbank {Name}", User.Identity?.Name,
                    Foodbank?.Name);

                if (User.IsInRole("FoodbankAdmin"))
                {
                    await _ctx.SaveChangesAsync();
                    return RedirectToPage("/Admin/Index");
                }

                break;
            }
            case "Approve":
            {
                if (!User.IsInRole("ApprovalAdmin") && !User.IsInRole("SiteAdmin")) return Forbid();
                var id = int.Parse(RouteData.Values["id"]?.ToString() ?? "");

                Foodbank = await _ctx.Foodbanks.Where(f => f.FoodbankId == id).FirstOrDefaultAsync();

                if (Foodbank != null) Foodbank.Status = Status.Approved;

                await _ctx.SaveChangesAsync();

                var u = new IdentityUser
                {
                    UserName = Foodbank?.Email,
                    Email = Foodbank?.Email,
                    EmailConfirmed = true,
                    PhoneNumberConfirmed = true
                };
                var result = await _userManager.CreateAsync(u);
                if (!result.Succeeded)
                {
                    foreach (var identityError in result.Errors)
                        ModelState.AddModelError(string.Empty, identityError.Code + " :: " + identityError.Description);

                    return Page();
                }

                u = await _userManager.FindByEmailAsync(Foodbank?.Email);

                result = await _userManager.AddToRoleAsync(u, "FoodbankAdmin");
                if (!result.Succeeded)
                {
                    foreach (var identityError in result.Errors)
                        ModelState.AddModelError(string.Empty, identityError.Code + " :: " + identityError.Description);

                    return Page();
                }

                result = await _userManager.AddClaimAsync(u,
                    new Claim("FoodbankClaim", Foodbank?.FoodbankId.ToString()));
                if (!result.Succeeded)
                {
                    foreach (var identityError in result.Errors)
                        ModelState.AddModelError(string.Empty, identityError.Code + " :: " + identityError.Description);

                    return Page();
                }

                _logger.Log(LogLevel.Information, "User {UserName} approved foodbank {Name}", User.Identity?.Name,
                    Foodbank?.Name);


                return RedirectToPage("/Admin/Index");
            }

            case "Deny":
            {
                if (!User.IsInRole("ApprovalAdmin") && !User.IsInRole("SiteAdmin")) return Forbid();
                var id = int.Parse(RouteData.Values["id"]?.ToString() ?? "");

                var fb = await _ctx.Foodbanks.Where(f => f.FoodbankId == id).FirstOrDefaultAsync();

                if (fb != null) fb.Status = Status.Denied;

                await _ctx.SaveChangesAsync();

                _logger.Log(LogLevel.Information, "User {UserName} denied foodbank {Name}", User.Identity?.Name,
                    Foodbank?.Name);

                return RedirectToPage("/Admin/Index");
            }
        }

        await _ctx.SaveChangesAsync();
        return RedirectToPage("./Foodbanks");
    }
}
#region

using Foodbank_Project.Data;
using Foodbank_Project.Models;
using Foodbank_Project.Util;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using NetTopologySuite.Geometries;

#endregion

namespace Foodbank_Project.Pages.Admin;

[Authorize(Roles = "FoodbanksAdmin,FoodbankAdmin,SiteAdmin")] // TODO: add access for FoodbankAdmin based on claim!
public class FoodbankModel : PageModel
{
    private readonly ApplicationContext _ctx;

    public string? Action;
    
    public IList<Models.Location>? Locations;
    
    public IList<Models.Need>? Needs;
    
    public bool HasNextPage;
    public bool HasPrevPage;
    public int MaxPages;
    public string? OrderBy;
    public string? OrderDirection;
    public new int Page;
    public string? Search;
    public int TotalItems;
    
    

    public FoodbankModel(ApplicationContext ctx)
    {
        _ctx = ctx;
    }

    [BindProperty] public Models.Foodbank? Foodbank { get; set; }
    [BindProperty] public double Lat { get; set; }
    [BindProperty] public double Lng { get; set; }

    public async Task OnGetAsync([FromQuery(Name = "Action")] string? action, [FromQuery(Name = "OrderBy")] string? orderBy,
        [FromQuery(Name = "OrderDirection")] string? orderDirection,
        [FromQuery(Name = "Search")] string? search, [FromQuery(Name = "Page")] string? page)
    {
        Action = action ?? "Update";
        if (Action != "Create")
        {
            var id = int.Parse(RouteData.Values["id"] as string ?? "");
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

            var locationQue = _ctx.Locations!.AsNoTracking().Include(l => l.Foodbank).Where(l => l.Foodbank!.FoodbankId == id)
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
        }
        else
        {
            Foodbank = new Models.Foodbank
            {
                Created = DateTime.Now
            };
        }
    }


    public async Task<IActionResult> OnPostAsync([FromQuery(Name = "Action")] string? action)
    {
        Action = action ?? Request.Form["Action"].ToString() ?? "Update";
        switch (Action)
        {
            case "Delete":
                if (Foodbank != null) _ctx.Remove(Foodbank);

                break;
            case "Create":
            {
                if (!ModelState.IsValid) return Page();
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
                if (!ModelState.IsValid) return Page();
                if (Foodbank != null)
                {
                    Foodbank.Coord = new Point(Lng, Lat) { SRID = 4326 };
                    _ctx.Foodbanks?.Update(Foodbank);
                }

                break;
            }
            case "Approve":
            {
                if (!ModelState.IsValid) return Page();
                int id = int.Parse(RouteData.Values["id"]?.ToString() ?? "");

                Models.Foodbank? fb = await _ctx.Foodbanks.Where(f => f.FoodbankId == id).FirstOrDefaultAsync();

                if (fb != null)
                {
                    fb.Status = Status.Approved;
                }

                await _ctx.SaveChangesAsync();
                
                return RedirectToPage("/Admin/Index");

                
            }

            case "Deny":
            {
                if (!ModelState.IsValid) return Page();
                int id = int.Parse(RouteData.Values["id"]?.ToString() ?? "");

                Models.Foodbank? fb = await _ctx.Foodbanks.Where(f => f.FoodbankId == id).FirstOrDefaultAsync();

                if (fb != null)
                {
                    fb.Status = Status.Denied;
                }
                
                await _ctx.SaveChangesAsync();
                
                return RedirectToPage("/Admin/Index");
                
            }
                
        }

        await _ctx.SaveChangesAsync();
        return RedirectToPage("./Foodbanks");
    }
}
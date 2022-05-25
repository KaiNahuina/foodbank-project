#region

using Foodbank_Project.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

#endregion

namespace Foodbank_Project.Pages.Admin;

[Authorize(Roles = "FoodbanksAdmin,SiteAdmin")]
public class FoodbanksModel : PageModel
{
    private readonly ApplicationContext _ctx;

    public IList<Models.Foodbank>? Foodbanks;

    public bool HasNextPage;
    public bool HasPrevPage;
    public int MaxPages;
    public string? OrderBy;
    public string? OrderDirection;
    public new int Page;
    public string? Search;
    public int TotalItems;

    public FoodbanksModel(ApplicationContext ctx)
    {
        _ctx = ctx;
    }

    public async Task<IActionResult> OnGetAsync([FromQuery(Name = "OrderBy")] string? orderBy,
        [FromQuery(Name = "OrderDirection")] string? orderDirection,
        [FromQuery(Name = "Search")] string? search, [FromQuery(Name = "Page")] string? page)
    {
        if (!User.IsInRole("FoodbanksAdmin") && !User.IsInRole("SiteAdmin")) return Forbid();
        OrderBy = string.IsNullOrEmpty(orderBy) ? "Locations" : orderBy;
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

        var foodbankQue = (from f in _ctx.Foodbanks
                select f).AsNoTracking().Include(f => f.Locations)
            .OrderByDescending(n => n.Name)
            .Where(n =>
                string.IsNullOrEmpty(Search) || n.Name!.Contains(Search) || n.Address!.Contains(Search) ||
                n.Postcode!.Contains(Search)
                || n.FoodbankId.ToString() == Search);

        switch (OrderDirection)
        {
            case "Asc":
            {
                foodbankQue = OrderBy switch
                {
                    "Name" => foodbankQue.OrderBy(n => n.Name),
                    "Address" => foodbankQue.OrderBy(n => n.Address),
                    "Submitted" => foodbankQue.OrderBy(n => n.Created),
                    "Locations" => foodbankQue.OrderBy(n => n.Locations!.Count),
                    _ => foodbankQue
                };

                break;
            }
            case "Desc":
            {
                foodbankQue = OrderBy switch
                {
                    "Name" => foodbankQue.OrderByDescending(n => n.Name),
                    "Address" => foodbankQue.OrderByDescending(n => n.Address),
                    "Submitted" => foodbankQue.OrderByDescending(n => n.Created),
                    "Locations" => foodbankQue.OrderByDescending(n => n.Locations!.Count),
                    _ => foodbankQue
                };

                break;
            }
        }

        HasPrevPage = Page > 1;

        TotalItems = await foodbankQue.CountAsync();
        MaxPages = (int)Math.Ceiling(TotalItems / 25d);

        HasNextPage = Page < MaxPages;

        Foodbanks = await foodbankQue.Skip((Page - 1) * 25).Take(25).ToListAsync();

        return Page();
    }
}
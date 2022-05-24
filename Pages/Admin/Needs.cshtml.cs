#region

using Foodbank_Project.Data;
using Foodbank_Project.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

#endregion

namespace Foodbank_Project.Pages.Admin;

[Authorize(Roles = "FoodbanksAdmin,FoodbankAdmin,SiteAdmin")] // TODO: add access for FoodbankAdmin based on claim!
public class StockModel : PageModel
{
    private readonly ApplicationContext _ctx;
    public bool HasNextPage;
    public bool HasPrevPage;
    public int MaxPages;

    public IList<Need>? Needs;
    public string? OrderBy;


    public string? OrderDirection;
    public new int Page;
    public string? Search;
    public int TotalItems;


    public StockModel(ApplicationContext ctx)
    {
        _ctx = ctx;
    }

    public async Task OnGetAsync([FromQuery(Name = "OrderBy")] string? orderBy,
        [FromQuery(Name = "OrderDirection")] string? orderDirection,
        [FromQuery(Name = "Search")] string? search, [FromQuery(Name = "Page")] string? page)
    {
        OrderBy = string.IsNullOrEmpty(orderBy) ? "Count" : orderBy;
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

        var needQue = (from f in _ctx.Needs
                select f).AsNoTracking()
            .Include(n => n.Foodbanks)
            .OrderByDescending(n => n.Foodbanks.Count)
            .Where(n => n.NeedStr != null).Where(n =>
                string.IsNullOrEmpty(Search) || n.NeedStr!.Contains(Search) || n.NeedId.ToString() == Search);


        switch (OrderDirection)
        {
            case "Asc":
            {
                needQue = OrderBy switch
                {
                    "Name" => needQue.OrderBy(n => n.NeedStr),
                    "Count" => needQue.OrderBy(n => n.Foodbanks.Count),
                    _ => needQue
                };

                break;
            }
            case "Desc":
            {
                needQue = OrderBy switch
                {
                    "Name" => needQue.OrderByDescending(n => n.NeedStr),
                    "Count" => needQue.OrderByDescending(n => n.Foodbanks.Count),
                    _ => needQue
                };

                break;
            }
        }

        HasPrevPage = Page > 1;

        TotalItems = await needQue.CountAsync();
        MaxPages = (int)Math.Ceiling(TotalItems / 25d);

        HasNextPage = Page < MaxPages;

        Needs = await needQue.Skip((Page - 1) * 25).Take(25).ToListAsync();
    }
}
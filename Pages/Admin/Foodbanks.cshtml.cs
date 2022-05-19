#region

using Foodbank_Project.Data;
using Foodbank_Project.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

#endregion

namespace Foodbank_Project.Pages.Admin;

public class FoodbanksModel : PageModel
{
    private readonly ApplicationContext _ctx;

    public IList<Models.Foodbank> Foodbanks;
    public bool HasNextPage;
    public bool HasPrevPage;
    public int MaxPages;
    public string OrderBy;

    public string OrderDirection;
    public int Page;
    public string Search;
    public int TotalItems;

    public FoodbanksModel(ApplicationContext ctx)
    {
        _ctx = ctx;
    }

    public async Task OnGetAsync([FromQuery(Name = "OrderBy")] string? orderBy,
        [FromQuery(Name = "OrderDirection")] string? orderDirection,
        [FromQuery(Name = "Search")] string? search, [FromQuery(Name = "Page")] string? page)
    {
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
                string.IsNullOrEmpty(Search) || n.Name.Contains(Search) || n.Address.Contains(Search) ||
                n.Postcode.Contains(Search)
                || n.FoodbankId.ToString() == Search);

        // do sort and filter shizzle here

        switch (OrderDirection)
        {
            case "Asc":
            {
                switch (OrderBy)
                {
                    case "Name":
                    {
                        foodbankQue = foodbankQue.OrderBy(n => n.Name);
                        break;
                    }
                    case "Address":
                    {
                        foodbankQue = foodbankQue.OrderBy(n => n.Address);
                        break;
                    }
                    case "Submitted":
                    {
                        foodbankQue = foodbankQue.OrderBy(n => n.Created);
                        break;
                    }
                    case "Locations":
                    {
                        foodbankQue = foodbankQue.OrderBy(n => n.Locations.Count);
                        break;
                    }
                }

                break;
            }
            case "Desc":
            {
                switch (OrderBy)
                {
                    case "Name":
                    {
                        foodbankQue = foodbankQue.OrderByDescending(n => n.Name);
                        break;
                    }
                    case "Address":
                    {
                        foodbankQue = foodbankQue.OrderByDescending(n => n.Address);
                        break;
                    }
                    case "Submitted":
                    {
                        foodbankQue = foodbankQue.OrderByDescending(n => n.Created);
                        break;
                    }
                    case "Locations":
                    {
                        foodbankQue = foodbankQue.OrderByDescending(n => n.Locations.Count);
                        break;
                    }
                }

                break;
            }
        }

        HasPrevPage = Page > 1;

        TotalItems = await foodbankQue.CountAsync();
        MaxPages = (int)Math.Ceiling(TotalItems / 25d);

        HasNextPage = Page < MaxPages;

        Foodbanks = await foodbankQue.Skip((Page - 1) * 25).Take(25).ToListAsync();
    }
    
}
﻿#region

using Foodbank_Project.Data;
using Foodbank_Project.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

#endregion

namespace Foodbank_Project.Pages.Admin;

public class StockModel : PageModel
{
    private readonly ApplicationContext _ctx;

    public IList<Need> Needs;


    public string OrderDirection;
    public string OrderBy;
    public int Page;
    public string Search;
    public bool HasPrevPage;
    public bool HasNextPage;
    public int TotalItems;
    public int MaxPages;
    

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
            if (string.IsNullOrEmpty(Search))
            {
                Search = "";
            }
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
                string.IsNullOrEmpty(Search) || n.NeedStr.Contains(Search) || n.NeedId.ToString() == Search);

        // do sort and filter shizzle here

        switch (OrderDirection)
        {
            case "Asc":
            {
                switch (OrderBy)
                {
                    case "Name":
                    {
                        needQue = needQue.OrderBy(n => n.NeedStr);
                        break;
                    }
                    case "Count":
                    {
                        needQue = needQue.OrderBy(n => n.Foodbanks.Count);
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
                        needQue = needQue.OrderByDescending(n => n.NeedStr);
                        break;
                    }
                    case "Count":
                    {
                        needQue = needQue.OrderByDescending(n => n.Foodbanks.Count);
                        break;
                    }
                }

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
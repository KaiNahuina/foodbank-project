#region

using Foodbank_Project.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

#endregion

namespace Foodbank_Project.Pages.Admin;

[Authorize(Roles = "RecipeAdmin,SiteAdmin")]
public class RecipesModel : PageModel
{
    
    private readonly ApplicationContext _ctx;

    public IList<Models.Recipe>? Recipes;

    public bool HasNextPage;
    public bool HasPrevPage;
    public int MaxPages;
    public string? OrderBy;
    public string? OrderDirection;
    public new int Page;
    public string? Search;
    public int TotalItems;

    public RecipesModel(ApplicationContext ctx)
    {
        _ctx = ctx;
    }
    
    public async Task OnGetAsync([FromQuery(Name = "OrderBy")] string? orderBy,
        [FromQuery(Name = "OrderDirection")] string? orderDirection,
        [FromQuery(Name = "Search")] string? search, [FromQuery(Name = "Page")] string? page)
    {
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

        var recipeQue = (from f in _ctx.Recipes
                select f).AsNoTracking()
            .OrderByDescending(n => n.Name)
            .Where(n =>
                string.IsNullOrEmpty(Search) || n.Name!.Contains(Search) || n.Notes!.Contains(Search) ||
                n.Method!.Contains(Search)
                || n.Ingredients! == Search);

        switch (OrderDirection)
        {
            case "Asc":
            {
                recipeQue = OrderBy switch
                {
                    "Name" => recipeQue.OrderBy(n => n.Name),
                    "Method" => recipeQue.OrderBy(n => n.Method),
                    "Ingredients" => recipeQue.OrderBy(n => n.Ingredients),
                    "Notes" => recipeQue.OrderBy(n => n.Notes),
                    _ => recipeQue
                };

                break;
            }
            case "Desc":
            {
                recipeQue = OrderBy switch
                {
                    "Name" => recipeQue.OrderByDescending(n => n.Name),
                    "Method" => recipeQue.OrderByDescending(n => n.Method),
                    "Ingredients" => recipeQue.OrderByDescending(n => n.Ingredients),
                    "Notes" => recipeQue.OrderByDescending(n => n.Notes),
                    _ => recipeQue
                };

                break;
            }
        }

        HasPrevPage = Page > 1;

        TotalItems = await recipeQue.CountAsync();
        MaxPages = (int)Math.Ceiling(TotalItems / 25d);

        HasNextPage = Page < MaxPages;

        Recipes = await recipeQue.Skip((Page - 1) * 25).Take(25).ToListAsync();
    }
    public string TrimBlob(string? blob)
    {
        if (blob is null) return "";
        if (blob.Length > 32)
            return blob[..32] + " ...";
        return blob;
    }
}
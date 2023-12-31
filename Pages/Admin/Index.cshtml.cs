#region

using Foodbank_Project.Data;
using Foodbank_Project.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

#endregion

namespace Foodbank_Project.Pages.Admin;

public class IndexModel : PageModel
{
    private readonly ApplicationContext _ctx;

    public IList<Models.Foodbank>? Foodbanks;

    public bool HasNextPage;
    public bool HasPrevPage;
    public int MaxPages;
    public string? OrderBy;
    public string? OrderDirection;
    public new int Page;
    public IList<Recipe>? Recipes;
    public string? Search;
    public int TotalItems;

    public IndexModel(ApplicationContext ctx)
    {
        _ctx = ctx;
    }

    public async Task<IActionResult> OnGetAsync([FromQuery(Name = "OrderBy")] string? orderBy,
        [FromQuery(Name = "OrderDirection")] string? orderDirection,
        [FromQuery(Name = "Search")] string? search, [FromQuery(Name = "Page")] string? page)
    {
        if (!User.IsInRole("SiteAdmin") && !User.IsInRole("ApprovalAdmin"))
        {
            if (User.IsInRole("FoodbankAdmin"))
            {
                var id = User.Claims.Where(c => c.Type == "FoodbankClaim").Select(c => c.Value).First();
                return RedirectToPage("./Foodbank",
                    new { id });
            }

            if (User.IsInRole("FoodbanksAdmin")) return RedirectToPage("./Foodbanks");
            if (User.IsInRole("UsersAdmin")) return RedirectToPage("./Users");
            if (User.IsInRole("RecipesAdmin")) return RedirectToPage("./Recipes");
            if (User.IsInRole("NeedsAdmin")) return RedirectToPage("./Needs");
        }


        if (!User.IsInRole("SiteAdmin") && !User.IsInRole("ApprovalAdmin")) return Forbid();
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
                select f).AsNoTracking().Where(f => f.Status == Status.UnConfirmed).Include(f => f.Locations)
            .OrderByDescending(n => n.Name)
            .Where(n =>
                string.IsNullOrEmpty(Search) || n.Name!.Contains(Search) || n.Address!.Contains(Search) ||
                n.Postcode!.Contains(Search)
                || n.FoodbankId.ToString() == Search);

        var recipeQue = (from f in _ctx.Recipes
                select f).AsNoTracking().Where(f => f.Status == Status.UnConfirmed).Include(f => f.Categories)
            .OrderByDescending(n => n.Name)
            .Where(n =>
                string.IsNullOrEmpty(Search) || n.Name!.Contains(Search) || n.Ingredients!.Contains(Search) ||
                n.Method!.Contains(Search) || n.Notes!.Contains(Search) || n.Serves!.Contains(Search)
                || n.RecipeId.ToString() == Search);

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
                recipeQue = OrderBy switch
                {
                    "Name" => recipeQue.OrderBy(n => n.Name),
                    "Ingredients" => recipeQue.OrderBy(n => n.Ingredients),
                    "Method" => recipeQue.OrderBy(n => n.Method),
                    "Notes" => recipeQue.OrderBy(n => n.Notes),
                    _ => recipeQue
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

                recipeQue = OrderBy switch
                {
                    "Name" => recipeQue.OrderByDescending(n => n.Name),
                    "Ingredients" => recipeQue.OrderByDescending(n => n.Ingredients),
                    "Method" => recipeQue.OrderByDescending(n => n.Method),
                    "Notes" => recipeQue.OrderByDescending(n => n.Notes),
                    _ => recipeQue
                };

                break;
            }
        }

        HasPrevPage = Page > 1;

        TotalItems = Math.Max(await foodbankQue.CountAsync(), await recipeQue.CountAsync());
        MaxPages = (int)Math.Ceiling(TotalItems / 25d);

        HasNextPage = Page < MaxPages;

        Foodbanks = await foodbankQue.Skip((Page - 1) * 25).Take(25).ToListAsync();
        Recipes = await recipeQue.Skip((Page - 1) * 25).Take(25).ToListAsync();
        return Page();
    }

#pragma warning disable CA1822
    public string TrimBlob(string? blob)
#pragma warning restore CA1822
    {
        if (blob is null) return "";
        if (blob.Length > 32)
            return blob[..32] + " ...";
        return blob;
    }
}
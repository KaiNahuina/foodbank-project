#region

using Foodbank_Project.Data;
using Foodbank_Project.Models;
using Foodbank_Project.Util;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using NetTopologySuite.Geometries;

#endregion

namespace Foodbank_Project.Pages.Admin;

public class RecipeModel : PageModel
{
    private readonly ApplicationContext _ctx;

    public string? Action;
    
    public bool HasNextPage;
    public bool HasPrevPage;
    public int MaxPages;
    public string? OrderBy;
    public string? OrderDirection;
    public new int Page;
    public string? Search;
    public int TotalItems;
    
    public IList<Models.RecipeCategory>? Categories;

    public RecipeModel(ApplicationContext ctx)
    {
        _ctx = ctx;
    }

    [BindProperty] public Recipe? Recipe { get; set; }
    
    [BindProperty] public IFormFile? Upload { get; set; }

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
            
            var recipeQue = from f in _ctx.Recipes where f.RecipeId == id select f;

            var categoryQue = _ctx.RecipeCategories!.AsNoTracking().Include(r => r.Recipes.Where(c => c.RecipeId == id))
                .Where(c => c.Recipes.Any(r => r.RecipeId == id))
                .OrderByDescending(n => n.Name)
                .Where(n => 
                    string.IsNullOrEmpty(Search) || n.Name!.Contains(Search)
                    || n.RecipeCategoryId.ToString() == Search);


            switch (OrderDirection)
            {
                case "Asc":
                {
                    categoryQue = OrderBy switch
                    {
                        "Name" => categoryQue.OrderBy(n => n.Name),
                        _ => categoryQue
                    };
                    break;
                }
                case "Desc":
                {
                    categoryQue = OrderBy switch
                    {
                        "Name" => categoryQue.OrderByDescending(n => n.Name),
                        _ => categoryQue
                    };
                    break;
                }
            }
            
            HasPrevPage = Page > 1;

            TotalItems = await categoryQue.CountAsync();
            MaxPages = (int)Math.Ceiling(TotalItems / 25d);

            HasNextPage = Page < MaxPages;

            Categories = await categoryQue.Skip((Page - 1) * 25).Take(25).ToListAsync();

            Recipe = await recipeQue.AsNoTracking().Include(l => l.Category).FirstAsync();
        }
        else
        {
            Recipe = new Recipe();
        }
    }


    public async Task<IActionResult> OnPostAsync([FromQuery(Name = "Action")] string? action)
    {
        Action = action ?? Request.Form["Action"].ToString() ?? "Update";
        switch (Action)
        {
            case "Delete":
                if (Recipe != null) _ctx.Remove(Recipe);
                break;
            case "Create":
            {
                if (!ModelState.IsValid) return Page();
                if (Recipe != null)
                {
                    if (Upload is not null)
                    {
                        MemoryStream ms = new MemoryStream();
                        await Upload.CopyToAsync(ms);
                        Recipe.Image = ms.ToArray();
                    }
                    _ctx.Recipes?.Update(Recipe);
                }

                break;
            }
            case "Update":
            {
                if (!ModelState.IsValid) return Page();
                if (Recipe != null)
                {
                    if (Upload is not null)
                    {
                        MemoryStream ms = new MemoryStream();
                        await Upload.CopyToAsync(ms);
                        Recipe.Image = ms.ToArray();
                    }
                    _ctx.Recipes?.Update(Recipe);
                }

                break;
            }
            case "Approve":
            {
                if (!ModelState.IsValid) return Page();
                int id = int.Parse(RouteData.Values["id"]?.ToString() ?? "");

                Recipe? fb = await _ctx.Recipes.Where(f => f.RecipeId == id).FirstOrDefaultAsync();

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

                Recipe? fb = await _ctx.Recipes.Where(f => f.RecipeId == id).FirstOrDefaultAsync();

                if (fb != null)
                {
                    fb.Status = Status.Denied;
                }
                
                await _ctx.SaveChangesAsync();
                
                return RedirectToPage("/Admin/Index");
                
            }

        }

        await _ctx.SaveChangesAsync();
        return RedirectToPage("./Recipes");
    }
}
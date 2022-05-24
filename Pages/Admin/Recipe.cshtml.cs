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

[Authorize(Roles = "RecipesAdmin,ApprovalAdmin,SiteAdmin")]
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
    private readonly ILogger<RecipeModel> _logger;

    public RecipeModel(ApplicationContext ctx, ILogger<RecipeModel> logger)
    {
        _ctx = ctx;
        _logger = logger;
    }

    [BindProperty] public Recipe? Recipe { get; set; }

    [BindProperty] public IFormFile? Upload { get; set; }

    public async Task<IActionResult> OnGetAsync([FromQuery(Name = "Action")] string? action,
        [FromQuery(Name = "OrderBy")] string? orderBy,
        [FromQuery(Name = "OrderDirection")] string? orderDirection,
        [FromQuery(Name = "Search")] string? search, [FromQuery(Name = "Page")] string? page)
    {
        if (User.IsInRole("ApprovalAdmin")) return Unauthorized();
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

            Recipe = await recipeQue.AsNoTracking().Include(l => l.Categories).FirstAsync();
            return Page();
        }

        Recipe = new Recipe();
        return Page();
    }


    public async Task<IActionResult> OnPostAsync([FromQuery(Name = "Action")] string? action)
    {
        Action = action ?? Request.Form["Action"].ToString() ?? "Update";
        switch (Action)
        {
            case "Delete":
                if (User.IsInRole("ApprovalAdmin")) return Forbid();
                if (Recipe != null) _ctx.Remove(Recipe);
                _logger.Log(LogLevel.Warning, "User {UserName} deleted recipe {Recipe}",
                    User.Identity?.Name, Recipe?.Name);
                break;
            case "Create":
            {
                if (User.IsInRole("ApprovalAdmin")) return Forbid();
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
                    
                    _logger.Log(LogLevel.Information, "User {UserName} created recipe {Recipe}",
                        User.Identity?.Name, Recipe?.Name);
                }

                break;
            }
            case "Update":
            {
                if (User.IsInRole("ApprovalAdmin")) return Forbid();
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
                    
                    _logger.Log(LogLevel.Information, "User {UserName} updated recipe {Recipe}",
                        User.Identity?.Name, Recipe?.Name);
                }

                break;
            }
            case "Approve":
            {
                if (!User.IsInRole("ApprovalAdmin") && !User.IsInRole("SiteAdmin")) return Forbid();
                if (!ModelState.IsValid) return Page();
                int id = int.Parse(RouteData.Values["id"]?.ToString() ?? "");

                Recipe? fb = await _ctx.Recipes.Where(f => f.RecipeId == id).FirstOrDefaultAsync();

                if (fb != null)
                {
                    fb.Status = Status.Approved;
                }

                await _ctx.SaveChangesAsync();
                
                _logger.Log(LogLevel.Information, "User {UserName} approved recipe {Recipe}",
                    User.Identity?.Name, Recipe?.Name);

                return RedirectToPage("/Admin/Index");
            }

            case "Deny":
            {
                if (!User.IsInRole("ApprovalAdmin") && !User.IsInRole("SiteAdmin")) return Forbid();
                if (!ModelState.IsValid) return Page();
                int id = int.Parse(RouteData.Values["id"]?.ToString() ?? "");

                Recipe? fb = await _ctx.Recipes.Where(f => f.RecipeId == id).FirstOrDefaultAsync();

                if (fb != null)
                {
                    fb.Status = Status.Denied;
                }

                await _ctx.SaveChangesAsync();
                
                _logger.Log(LogLevel.Information, "User {UserName} denied recipe {Recipe}",
                    User.Identity?.Name, Recipe?.Name);

                return RedirectToPage("/Admin/Index");
            }
        }

        await _ctx.SaveChangesAsync();
        return RedirectToPage("./Recipes");
    }
}
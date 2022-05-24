using Foodbank_Project.Data;
using Foodbank_Project.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace Foodbank_Project.Pages.Admin;

[Authorize(Roles = "RecipesAdmin,SiteAdmin")]
public class RecipeCategoryModel : PageModel
{
    private readonly ApplicationContext _ctx;
    private readonly ILogger<RecipeCategoryModel> _logger;

    public string? Action { get; set; }

    public string? Name { get; set; }
    public int? Target { get; set; }

    public List<RecipeCategory>? Categories { get; set; }

    public RecipeCategoryModel(ApplicationContext ctx, ILogger<RecipeCategoryModel> logger)
    {
        _ctx = ctx;
        _logger = logger;
    }

    public async Task OnGetAsync([FromQuery(Name = "Action")] string? action)
    {
        Action = action ?? "Update";
        switch (Action)
        {
            case "Search":
            {
                Name = Request.Query["Name"];
                Target = int.Parse(Request.Query["Target"]);
                Categories = await _ctx.RecipeCategories!.AsNoTracking().Where(n => n.Name!.Contains(Name))
                    .ToListAsync();
                break;
            }
            default:
            {
                Target = int.Parse(Request.Query["Target"]);
                Categories = await _ctx.RecipeCategories!.AsNoTracking().ToListAsync();
                break;
            }
        }
    }

    public async Task<IActionResult> OnPostAsync()
    {
        Action = Request.Form["Action"].ToString() ?? "Update";
        switch (Action)
        {
            case "Remove":
            {
                Target = int.Parse(Request.Form["Target"]);
                var id = int.Parse(Request.RouteValues["id"]?.ToString());
                var recipe = await _ctx.Recipes!.Where(f => f.RecipeId == Target)
                    .Include(f => f.Categories.Where(n => n.RecipeCategoryId == id))
                    .FirstAsync();

                recipe.Categories!.Clear();

                await _ctx.SaveChangesAsync();
                
                _logger.Log(LogLevel.Information, "User {UserName} removed category id {Category} from {Recipe}",
                    User.Identity?.Name, id, recipe.Name);

                return RedirectToPage("./Recipe", routeValues: new { id = Target }, fragment: "categories",
                    pageHandler: "");
            }
            case "Add":
            {
                Target = int.Parse(Request.Form["Target"]);
                var id = int.Parse(Request.RouteValues["id"]?.ToString());
                var recipe = await _ctx.Recipes!.Where(f => f.RecipeId == Target).Include(f => f.Categories)
                    .FirstAsync();

                var category = await _ctx.RecipeCategories!.Where(n => n.RecipeCategoryId == id).FirstAsync();

                recipe.Categories!.Add(category);

                await _ctx.SaveChangesAsync();
                
                _logger.Log(LogLevel.Information, "User {UserName} added category id {Category} from {Recipe}",
                    User.Identity?.Name, id, recipe.Name);

                return RedirectToPage("./Recipe", routeValues: new { id = Target }, fragment: "categories",
                    pageHandler: "");
            }
        }

        await _ctx.SaveChangesAsync();
        return Page();
    }
}
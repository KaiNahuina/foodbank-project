#region

using Foodbank_Project.Data;
using Foodbank_Project.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

#endregion

namespace Foodbank_Project.Pages.Recipes;

public class IndexModel : PageModel
{
    [BindProperty]
    public Models.Recipe Recipe { get; set; } = new();
    [BindProperty]
    public IFormFile Image { get; set; }
    [BindProperty]
    public Dictionary<int, Pair<RecipeCategory, bool>> SelectedCategories { get; set; } = new();


    private ApplicationContext _ctx;

    public IndexModel(ApplicationContext ctx)
    {
        _ctx = ctx;
    }

    

    public void OnGet()
    {
        foreach (var category in _ctx.RecipeCategories.ToArray())
        {
            
        }
    }

}